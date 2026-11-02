<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { api, type CurrentUser, type Report, type User } from './api'
import DirectoryPage from './pages/DirectoryPage.vue'
import LoginPage from './pages/LoginPage.vue'
import ReportDetailPage from './pages/ReportDetailPage.vue'
import ReportEditPage from './pages/ReportEditPage.vue'
import ReportsPage from './pages/ReportsPage.vue'
import UserDetailPage from './pages/UserDetailPage.vue'
import UserEditPage from './pages/UserEditPage.vue'
import UsersPage from './pages/UsersPage.vue'

type Section = 'reports' | 'report-detail' | 'report-edit' | 'users' | 'user-detail' | 'user-edit' | 'directory'

const currentUser = ref<CurrentUser | null>(null)
const activeSection = ref<Section>('reports')
const loading = ref(false)
const submitting = ref(false)
const error = ref('')
const notice = ref('')
const reports = ref<Report[]>([])
const selectedReport = ref<Report | null>(null)
const selectedUser = ref<User | null>(null)
const users = ref<User[]>([])
const specialities = ref<Awaited<ReturnType<typeof api.specialities>>>([])
const services = ref<Awaited<ReturnType<typeof api.services>>>([])
const reportsPage = ref<InstanceType<typeof ReportsPage> | null>(null)
const usersPage = ref<InstanceType<typeof UsersPage> | null>(null)

const isAdmin = computed(() => currentUser.value?.role === 'Admin')
const pageTitle = computed(() => {
  if (activeSection.value === 'report-detail') return `Отчёт №${selectedReport.value?.id ?? ''}`
  if (activeSection.value === 'report-edit') return 'Редактирование отчёта'
  if (activeSection.value === 'users') return 'Пользователи'
  if (activeSection.value === 'user-detail') return 'Карточка пользователя'
  if (activeSection.value === 'user-edit') return 'Изменение пользователя'
  if (activeSection.value === 'directory') return 'Справочники'
  return 'Отчёты'
})

function showError(value: unknown) {
  error.value = value instanceof Error ? value.message : 'Произошла неизвестная ошибка'
  notice.value = ''
}

async function run<T>(action: () => Promise<T>) {
  loading.value = true
  error.value = ''
  notice.value = ''
  try {
    return await action()
  } catch (reason) {
    showError(reason)
    return undefined
  } finally {
    loading.value = false
  }
}

async function loadReports(name = '') {
  const result = await run(() => api.reports(currentUser.value?.role ?? '', name))
  if (result) reports.value = result
}

async function loadUsers(text = '') {
  const result = await run(() => api.users(text))
  if (result) users.value = result
}

async function loadDirectory() {
  const result = await run(async () => Promise.all([api.specialities(), api.services()]))
  if (result) [specialities.value, services.value] = result
}

async function loadCurrentUser() {
  try {
    currentUser.value = await api.me()
  } catch {
    currentUser.value = null
  }
  if (currentUser.value) await Promise.all([loadReports(), loadDirectory()])
}

async function signIn(credentials: { code: string; password: string }) {
  submitting.value = true
  error.value = ''
  try {
    await api.login(credentials.code, credentials.password)
    currentUser.value = await api.me()
    await Promise.all([loadReports(), loadDirectory()])
  } catch (reason) {
    showError(reason)
  } finally {
    submitting.value = false
  }
}

async function signOut() {
  await run(async () => {
    await api.logout()
    currentUser.value = null
    reports.value = []
    users.value = []
    activeSection.value = 'reports'
  })
}

async function createReport(report: {
  pacientname: string
  price: number
  lechids: number[]
  period: number
}) {
  submitting.value = true
  const created = await run(() => api.addReport(report))
  if (created !== undefined) {
    reportsPage.value?.resetForm()
    notice.value = 'Отчёт создан.'
    await loadReports()
  }
  submitting.value = false
}

async function removeReport(report: Report) {
  if (!window.confirm(`Удалить отчёт для пациента «${report.pacientname}»?`)) return
  const result = await run(() => api.deleteReport(report.id))
  if (result !== undefined) {
    notice.value = 'Отчёт удалён.'
    await loadReports()
  }
}

async function openReportById(idValue: string | number) {
  const id = Number(idValue)
  if (!Number.isInteger(id) || id < 1) {
    error.value = 'Введите корректный ID отчёта.'
    return
  }
  selectedReport.value = null
  const result = await run(() => api.reportById(id))
  if (result) {
    selectedReport.value = result
    activeSection.value = 'report-detail'
  } else if (!error.value) {
    error.value = 'Отчёт с таким ID не найден.'
  }
}

function startReportEdit() {
  if (selectedReport.value) activeSection.value = 'report-edit'
}

async function saveReport(report: {
  pacientname: string
  price: number
  lechids: number[]
  period: number
}) {
  if (!selectedReport.value) return
  const id = selectedReport.value.id
  submitting.value = true
  const result = await run(() => api.updateReport(id, report))
  if (result !== undefined) {
    await openReportById(id)
    notice.value = 'Изменения сохранены.'
  }
  submitting.value = false
}

async function findUserById(id: string) {
  const userId = id.trim()
  if (!userId) {
    error.value = 'Введите ID пользователя.'
    return
  }
  selectedUser.value = null
  const result = await run(() => api.userById(userId))
  if (result) {
    selectedUser.value = result
    activeSection.value = 'user-detail'
  } else if (!error.value) {
    error.value = 'Пользователь с таким ID не найден.'
  }
}

async function backToUsers() {
  activeSection.value = 'users'
  await loadUsers()
}

function startUserEdit() {
  if (!selectedUser.value || !isAdmin.value) return
  if (!specialities.value.some((item) => item.name === selectedUser.value?.spec)) {
    error.value = 'Не удалось определить специальность пользователя.'
    return
  }
  activeSection.value = 'user-edit'
  error.value = ''
  notice.value = ''
}

async function saveUser(data: FormData) {
  const user = selectedUser.value
  if (!user || !isAdmin.value) return

  submitting.value = true
  const updatedUser = await run(async () => {
    await api.updateUser(user.userid, data)
    return api.userById(user.userid)
  })
  if (updatedUser) {
    selectedUser.value = updatedUser
    users.value = users.value.map((item) =>
      item.userid === updatedUser.userid ? updatedUser : item,
    )
    activeSection.value = 'user-detail'
    notice.value = 'Данные пользователя обновлены.'
  }
  submitting.value = false
}

async function createUser(data: FormData) {
  submitting.value = true
  const created = await run(() => api.addUser(data))
  if (created !== undefined) {
    usersPage.value?.resetForm()
    notice.value = 'Пользователь создан.'
    await loadUsers()
  }
  submitting.value = false
}

async function removeUser(user: User) {
  if (!window.confirm(`Удалить пользователя ${user.surname} ${user.name}?`)) return
  const result = await run(() => api.deleteUser(user.userid))
  if (result !== undefined) {
    notice.value = 'Пользователь удалён.'
    await loadUsers()
  }
}

async function createSpeciality(name: string) {
  const result = await run(() => api.addSpeciality(name))
  if (result !== undefined) {
    notice.value = 'Специальность добавлена.'
    await loadDirectory()
  }
}

async function createService(service: { name: string; spec: number }) {
  const result = await run(() => api.addService(service.name, service.spec))
  if (result !== undefined) {
    notice.value = 'Услуга добавлена.'
    await loadDirectory()
  }
}

async function removeDirectoryItem(item: { kind: 'speciality' | 'service'; id: number }) {
  const result = await run(() =>
    item.kind === 'speciality'
      ? api.deleteSpeciality(item.id)
      : api.deleteService(item.id),
  )
  if (result !== undefined) {
    notice.value = 'Запись удалена.'
    await loadDirectory()
  }
}

async function selectSection(section: 'reports' | 'users' | 'directory') {
  activeSection.value = section
  if (section === 'reports') await loadReports()
  if (section === 'users') await loadUsers()
  if (section === 'directory') await loadDirectory()
}

onMounted(loadCurrentUser)
</script>

<template>
  <LoginPage
    v-if="!currentUser"
    :submitting="submitting"
    :error="error"
    @submit="signIn"
  />

  <div v-else class="app-shell">
    <aside class="sidebar">
      <a class="brand" href="#" @click.prevent="selectSection('reports')">
        <span class="brand-mark small">P</span><span>Pacient<span class="brand-accent">IK</span></span>
      </a>
      <p class="nav-caption">РАБОЧИЙ СТОЛ</p>
      <nav class="navigation">
        <button :class="{ active: activeSection === 'reports' || activeSection.startsWith('report-') }" @click="selectSection('reports')">
          <span class="nav-icon">▤</span>Отчёты
        </button>
        <button v-if="isAdmin" :class="{ active: activeSection === 'users' || activeSection.startsWith('user-') }" @click="selectSection('users')">
          <span class="nav-icon">♙</span>Пользователи
        </button>
        <button v-if="isAdmin" :class="{ active: activeSection === 'directory' }" @click="selectSection('directory')">
          <span class="nav-icon">⌘</span>Справочники
        </button>
      </nav>
      <div class="sidebar-bottom">
        <div class="profile">
          <span class="avatar">{{ currentUser.role.slice(0, 1) }}</span>
          <span><strong>{{ currentUser.role }}</strong><small>Авторизован</small></span>
        </div>
        <button class="logout" @click="signOut">Выйти <span>↗</span></button>
      </div>
    </aside>

    <main class="main-content">
      <header class="topbar">
        <div><p class="eyebrow">Управление клиникой</p><h1>{{ pageTitle }}</h1></div>
        <span class="role-badge">{{ currentUser.role }}</span>
      </header>

      <div v-if="error" class="message error"><span>{{ error }}</span><button aria-label="Закрыть" @click="error = ''">×</button></div>
      <div v-if="notice" class="message success"><span>{{ notice }}</span><button aria-label="Закрыть" @click="notice = ''">×</button></div>

      <ReportsPage
        v-if="activeSection === 'reports'"
        ref="reportsPage"
        :reports="reports"
        :services="services"
        :role="currentUser.role"
        :is-admin="isAdmin"
        :loading="loading"
        :submitting="submitting"
        @load-reports="loadReports"
        @open-report="openReportById"
        @delete-report="removeReport"
        @create-report="createReport"
        @lookup-report="openReportById"
        @error="error = $event"
      />
      <ReportDetailPage
        v-else-if="activeSection === 'report-detail' && selectedReport"
        :report="selectedReport"
        @back="selectSection('reports')"
        @edit="startReportEdit"
      />
      <ReportEditPage
        v-else-if="activeSection === 'report-edit' && selectedReport"
        :report="selectedReport"
        :services="services"
        :submitting="submitting"
        @cancel="openReportById(selectedReport.id)"
        @save="saveReport"
        @error="error = $event"
      />
      <UsersPage
        v-else-if="activeSection === 'users' && isAdmin"
        ref="usersPage"
        :users="users"
        :specialities="specialities"
        :loading="loading"
        :submitting="submitting"
        @load-users="loadUsers"
        @open-user="findUserById"
        @delete-user="removeUser"
        @find-user="findUserById"
        @create-user="createUser"
        @error="error = $event"
      />
      <UserDetailPage
        v-else-if="activeSection === 'user-detail' && isAdmin && selectedUser"
        :user="selectedUser"
        @back="backToUsers"
        @edit="startUserEdit"
      />
      <UserEditPage
        v-else-if="activeSection === 'user-edit' && isAdmin && selectedUser"
        :user="selectedUser"
        :specialities="specialities"
        :submitting="submitting"
        @cancel="activeSection = 'user-detail'"
        @save="saveUser"
        @error="error = $event"
      />
      <DirectoryPage
        v-else-if="activeSection === 'directory' && isAdmin"
        :specialities="specialities"
        :services="services"
        @add-speciality="createSpeciality"
        @add-service="createService"
        @delete-item="removeDirectoryItem"
      />
    </main>
  </div>
</template>

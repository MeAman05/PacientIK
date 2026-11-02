<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { api, type CurrentUser, type Report, type Service, type Speciality, type User } from './api'

type Section = 'reports' | 'report-detail' | 'report-edit' | 'users' | 'user-detail' | 'user-edit' | 'directory'

const currentUser = ref<CurrentUser | null>(null)
const activeSection = ref<Section>('reports')
const loading = ref(false)
const submitting = ref(false)
const error = ref('')
const notice = ref('')

const loginForm = reactive({ code: '', password: '' })
const reportForm = reactive({ pacientname: '', price: '', period: '', lechids: [] as number[] })
const editForm = reactive({ pacientname: '', price: '', period: '', lechids: [] as number[] })
const userForm = reactive({
  name: '',
  surname: '',
  lastname: '',
  code: '',
  pwd: '',
  spec: '',
  age: '',
  role: 'User',
  photo: null as File | null,
})
const editUserForm = reactive({
  name: '',
  surname: '',
  lastname: '',
  spec: '',
  age: '',
})
const reportSearch = ref('')
const reportIdSearch = ref('')
const userSearch = ref('')
const userIdSearch = ref('')
const specialityName = ref('')
const serviceForm = reactive({ name: '', spec: '' })

const reports = ref<Report[]>([])
const selectedReport = ref<Report | null>(null)
const selectedUser = ref<User | null>(null)
const users = ref<User[]>([])
const specialities = ref<Speciality[]>([])
const services = ref<Service[]>([])
const isAdmin = computed(() => currentUser.value?.role === 'Admin')
const isReviewRole = computed(() => ['Admin', 'Checker'].includes(currentUser.value?.role ?? ''))
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

async function run<T>(action: () => Promise<T>, onSuccess?: (value: T) => void) {
  loading.value = true
  error.value = ''
  notice.value = ''
  try {
    const result = await action()
    onSuccess?.(result)
    return result
  } catch (reason) {
    showError(reason)
    return undefined
  } finally {
    loading.value = false
  }
}

async function loadReports() {
  const result = await run(() => api.reports(currentUser.value?.role ?? '', reportSearch.value))
  if (result) reports.value = result
}

async function loadUsers() {
  const result = await run(() => api.users(userSearch.value))
  if (result) users.value = result
}

async function findUserById() {
  const id = userIdSearch.value.trim()
  if (!id) {
    error.value = 'Введите ID пользователя.'
    return
  }
  selectedUser.value = null
  const result = await run(() => api.userById(id))
  if (result) selectedUser.value = result
  else if (!error.value) error.value = 'Пользователь с таким ID не найден.'
}

async function openUserById(id: string) {
  selectedUser.value = null
  const result = await run(() => api.userById(id))
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
  const user = selectedUser.value
  if (!user || !isAdmin.value) return

  const speciality = specialities.value.find(
    (item) => item.name === user.spec,
  )
  if (!speciality) {
    error.value = 'Не удалось определить специальность пользователя.'
    return
  }

  Object.assign(editUserForm, {
    name: user.name,
    surname: user.surname,
    lastname: user.lastname,
    spec: String(speciality.id),
    age: String(user.age),
  })
  activeSection.value = 'user-edit'
  error.value = ''
  notice.value = ''
}

async function saveUser() {
  const user = selectedUser.value
  if (!user || !isAdmin.value) return
  if (!editUserForm.name.trim() || !editUserForm.surname.trim() || !editUserForm.spec || !editUserForm.age) {
    error.value = 'Заполните имя, фамилию, возраст и специальность.'
    return
  }

  const data = new FormData()
  data.append('Name', editUserForm.name.trim())
  data.append('Surname', editUserForm.surname.trim())
  data.append('Lastname', editUserForm.lastname.trim())
  data.append('Spec', editUserForm.spec)
  data.append('Age', editUserForm.age)

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

function cancelUserEdit() {
  activeSection.value = 'user-detail'
  error.value = ''
}

async function openReportById(idValue: string | number = reportIdSearch.value) {
  const id = Number(idValue)
  if (!Number.isInteger(id) || id < 1) {
    error.value = 'Введите корректный ID отчёта.'
    return
  }
  selectedReport.value = null
  const result = await run(() => api.reportById(id))
  if (result) {
    selectedReport.value = result
    reportIdSearch.value = String(id)
    activeSection.value = 'report-detail'
  } else if (!error.value) error.value = 'Отчёт с таким ID не найден.'
}

function startReportEdit() {
  if (!selectedReport.value) return
  Object.assign(editForm, {
    pacientname: selectedReport.value.pacientname,
    price: String(selectedReport.value.price),
    period: String(selectedReport.value.period),
    lechids: [...selectedReport.value.lechids],
  })
  activeSection.value = 'report-edit'
  error.value = ''
  notice.value = ''
}

async function saveReport() {
  const report = selectedReport.value
  if (!report) return
  if (!editForm.pacientname.trim() || !editForm.price || !editForm.period) {
    error.value = 'Заполните имя пациента, цену и период.'
    return
  }
  submitting.value = true
  const result = await run(() =>
    api.updateReport(report.id, {
      pacientname: editForm.pacientname.trim(),
      price: Number(editForm.price),
      period: Number(editForm.period),
      lechids: editForm.lechids,
    }),
  )
  if (result !== undefined) {
    await openReportById(report.id)
    notice.value = 'Изменения сохранены.'
  }
  submitting.value = false
}

async function backToReports() {
  activeSection.value = 'reports'
  await loadReports()
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

async function signIn() {
  submitting.value = true
  error.value = ''
  try {
    await api.login(loginForm.code, loginForm.password)
    currentUser.value = await api.me()
    loginForm.password = ''
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

async function createReport() {
  if (!reportForm.pacientname.trim() || !reportForm.price || !reportForm.period) {
    error.value = 'Заполните имя пациента, цену и период.'
    return
  }
  submitting.value = true
  const created = await run(() =>
    api.addReport({
      pacientname: reportForm.pacientname.trim(),
      price: Number(reportForm.price),
      period: Number(reportForm.period),
      lechids: reportForm.lechids,
    }),
  )
  if (created !== undefined) {
    Object.assign(reportForm, { pacientname: '', price: '', period: '', lechids: [] })
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

async function createUser() {
  if (!userForm.name || !userForm.surname || !userForm.code || !userForm.pwd || !userForm.spec || !userForm.age || !userForm.photo) {
    error.value = 'Заполните обязательные поля и выберите фотографию.'
    return
  }
  const data = new FormData()
  for (const [key, value] of Object.entries(userForm)) {
    if (value !== null) data.append(key, value)
  }
  submitting.value = true
  const created = await run(() => api.addUser(data))
  if (created !== undefined) {
    Object.assign(userForm, {
      name: '', surname: '', lastname: '', code: '', pwd: '', spec: '', age: '', role: 'User', photo: null,
    })
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

async function createSpeciality() {
  if (!specialityName.value.trim()) return
  const result = await run(() => api.addSpeciality(specialityName.value.trim()))
  if (result !== undefined) {
    specialityName.value = ''
    notice.value = 'Специальность добавлена.'
    await loadDirectory()
  }
}

async function createService() {
  if (!serviceForm.name.trim() || !serviceForm.spec) return
  const result = await run(() => api.addService(serviceForm.name.trim(), Number(serviceForm.spec)))
  if (result !== undefined) {
    Object.assign(serviceForm, { name: '', spec: '' })
    notice.value = 'Услуга добавлена.'
    await loadDirectory()
  }
}

async function removeDirectoryItem(kind: 'speciality' | 'service', id: number) {
  const result = await run(() =>
    kind === 'speciality' ? api.deleteSpeciality(id) : api.deleteService(id),
  )
  if (result !== undefined) {
    notice.value = 'Запись удалена.'
    await loadDirectory()
  }
}

async function selectSection(section: Section) {
  activeSection.value = section
  if (section === 'reports') await loadReports()
  if (section === 'users') await loadUsers()
  if (section === 'directory') await loadDirectory()
}

function setPhoto(event: Event) {
  const input = event.target as HTMLInputElement
  userForm.photo = input.files?.[0] ?? null
}

onMounted(loadCurrentUser)
</script>

<template>
  <main v-if="!currentUser" class="login-page">
    <section class="login-card">
      <div class="brand-mark">P</div>
      <p class="eyebrow">Рабочее пространство</p>
      <h1>PacientIK</h1>
      <p class="muted">Войдите, чтобы продолжить</p>
      <form class="stack" @submit.prevent="signIn">
        <label>Код пользователя<input v-model="loginForm.code" autocomplete="username" required /></label>
        <label>Пароль<input v-model="loginForm.password" type="password" autocomplete="current-password" required /></label>
        <button class="primary" :disabled="submitting">{{ submitting ? 'Входим…' : 'Войти' }}</button>
      </form>
      <p v-if="error" class="message error">{{ error }}</p>
      <p class="login-footnote">Доступ защищён авторизацией API.</p>
    </section>
  </main>

  <div v-else class="app-shell">
    <aside class="sidebar">
      <a class="brand" href="#" @click.prevent="selectSection('reports')">
        <span class="brand-mark small">P</span><span>Pacient<span class="brand-accent">IK</span></span>
      </a>
      <p class="nav-caption">РАБОЧИЙ СТОЛ</p>
      <nav class="navigation">
        <button :class="{ active: activeSection === 'reports' }" @click="selectSection('reports')">
          <span class="nav-icon">▤</span>Отчёты
        </button>
        <button v-if="isAdmin" :class="{ active: activeSection === 'users' || activeSection === 'user-detail' }" @click="selectSection('users')">
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

      <template v-if="activeSection === 'reports'">
        <section class="summary-row">
          <article class="summary-card"><span class="summary-icon violet">▤</span><div><small>{{ isReviewRole ? 'Всего отчётов' : 'Мои отчёты' }}</small><strong>{{ reports.length }}</strong></div></article>
          <article class="summary-card"><span class="summary-icon green">₽</span><div><small>Сумма по списку</small><strong>{{ reports.reduce((sum, item) => sum + item.price, 0).toLocaleString('ru-RU') }} ₽</strong></div></article>
          <article class="summary-card"><span class="summary-icon amber">✳</span><div><small>Доступные услуги</small><strong>{{ services.length || '—' }}</strong></div></article>
        </section>

        <section class="panel lookup-panel">
          <div class="panel-heading"><div><p class="eyebrow">Быстрый доступ</p><h2>Открыть отчёт по ID</h2></div></div>
          <form class="inline-form lookup-form" @submit.prevent="openReportById()">
            <input v-model="reportIdSearch" type="number" min="1" placeholder="ID отчёта" required />
            <button class="primary">Открыть</button>
          </form>
        </section>

        <section v-if="!isAdmin" class="panel">
          <div class="panel-heading"><div><p class="eyebrow">Новая запись</p><h2>Создать отчёт</h2></div></div>
          <form class="form-grid" @submit.prevent="createReport">
            <label class="span-two">Имя пациента<input v-model="reportForm.pacientname" placeholder="Например, Иван Иванов" required /></label>
            <label>Стоимость, ₽<input v-model="reportForm.price" type="number" min="0" step="0.01" required /></label>
            <label>Период<input v-model="reportForm.period" type="number" min="1" required /></label>
            <fieldset class="span-two service-picker"><legend>Услуги</legend>
              <label v-for="service in services" :key="service.id" class="check-option">
                <input v-model="reportForm.lechids" type="checkbox" :value="service.id" />{{ service.name }}<small>{{ service.spec }}</small>
              </label>
              <span v-if="!services.length" class="muted">Список услуг не загружен.</span>
            </fieldset>
            <div class="span-two form-actions"><button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Создать отчёт' }}</button></div>
          </form>
        </section>

        <section class="panel">
          <div class="panel-heading list-heading"><div><p class="eyebrow">История</p><h2>{{ isReviewRole ? 'Все отчёты' : 'Ваши отчёты' }}</h2></div>
            <form class="search" @submit.prevent="loadReports"><input v-model="reportSearch" placeholder="Поиск по пациенту" /><button aria-label="Найти">⌕</button></form>
          </div>
          <div v-if="loading" class="empty-state">Загружаем отчёты…</div>
          <div v-else-if="!reports.length" class="empty-state"><span>▤</span><strong>Отчётов пока нет</strong><small>Когда появятся записи, они отобразятся здесь.</small></div>
          <div v-else class="table-wrap"><table><thead><tr><th>Пациент</th><th>Специалист</th><th>Услуги</th><th>Период</th><th>Стоимость</th><th></th></tr></thead>
            <tbody><tr v-for="report in reports" :key="report.id"><td><strong>{{ report.pacientname }}</strong><small class="cell-subtitle">Отчёт №{{ report.id }}</small></td><td>{{ report.sender }}</td><td>{{ report.leches.join(', ') || '—' }}</td><td>{{ report.period }}</td><td class="price">{{ report.price.toLocaleString('ru-RU') }} ₽</td><td class="row-actions"><button class="text-button" @click="openReportById(report.id)">Открыть</button><button v-if="!isAdmin" class="icon-button danger-text" title="Удалить отчёт" @click="removeReport(report)">×</button></td></tr></tbody>
          </table></div>
        </section>
      </template>

      <template v-else-if="activeSection === 'report-detail' && selectedReport">
        <section class="panel detail-panel">
          <div class="panel-heading"><div><p class="eyebrow">Информация по отчёту</p><h2>{{ selectedReport.pacientname }}</h2></div>
            <div class="form-actions"><button class="secondary" @click="backToReports">К списку</button><button class="primary" @click="startReportEdit">Редактировать</button></div>
          </div>
          <dl class="detail-grid">
            <div><dt>ID отчёта</dt><dd>{{ selectedReport.id }}</dd></div>
            <div><dt>Пациент</dt><dd>{{ selectedReport.pacientname }}</dd></div>
            <div><dt>Специалист</dt><dd>{{ selectedReport.sender }}</dd></div>
            <div><dt>Период</dt><dd>{{ selectedReport.period }}</dd></div>
            <div><dt>Стоимость</dt><dd>{{ selectedReport.price.toLocaleString('ru-RU') }} ₽</dd></div>
            <div class="span-two"><dt>Услуги</dt><dd>{{ selectedReport.leches.join(', ') || 'Не указаны' }}</dd></div>
          </dl>
        </section>
      </template>

      <template v-else-if="activeSection === 'report-edit' && selectedReport">
        <section class="panel">
          <div class="panel-heading"><div><p class="eyebrow">Отчёт №{{ selectedReport.id }}</p><h2>Изменить данные отчёта</h2></div><button class="secondary" @click="openReportById(selectedReport.id)">Отмена</button></div>
          <form class="form-grid" @submit.prevent="saveReport">
            <label class="span-two">Имя пациента<input v-model="editForm.pacientname" required /></label>
            <label>Стоимость, ₽<input v-model="editForm.price" type="number" min="0" step="0.01" required /></label>
            <label>Период<input v-model="editForm.period" type="number" min="1" required /></label>
            <fieldset class="span-two service-picker"><legend>Услуги</legend>
              <label v-for="service in services" :key="service.id" class="check-option">
                <input v-model="editForm.lechids" type="checkbox" :value="service.id" />{{ service.name }}<small>{{ service.spec }}</small>
              </label>
              <span v-if="!services.length" class="muted">Список услуг не загружен.</span>
            </fieldset>
            <div class="span-two form-actions"><button class="secondary" type="button" @click="openReportById(selectedReport.id)">Отмена</button><button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Сохранить изменения' }}</button></div>
          </form>
        </section>
      </template>

      <template v-else-if="activeSection === 'users' && isAdmin">
        <section class="panel">
          <div class="panel-heading"><div><p class="eyebrow">Карточка пользователя</p><h2>Найти пользователя по ID</h2></div></div>
          <form class="inline-form lookup-form" @submit.prevent="findUserById">
            <input v-model="userIdSearch" placeholder="UUID пользователя" required />
            <button class="primary">Найти</button>
          </form>
          <article v-if="selectedUser" class="user-detail">
            <img v-if="selectedUser.photourl" :src="selectedUser.photourl" :alt="`${selectedUser.name} ${selectedUser.surname}`" />
            <div class="user-detail-info">
              <p class="eyebrow">ID: {{ selectedUser.userid }}</p>
              <h3>{{ selectedUser.surname }} {{ selectedUser.name }} {{ selectedUser.lastname }}</h3>
              <p>{{ selectedUser.spec }} <span class="role-badge">{{ selectedUser.role }}</span></p>
              <small>Возраст: {{ selectedUser.age }}</small>
            </div>
          </article>
        </section>
        <section class="panel">
          <div class="panel-heading"><div><p class="eyebrow">Новая учётная запись</p><h2>Добавить пользователя</h2></div></div>
          <form class="form-grid" @submit.prevent="createUser">
            <label>Фамилия<input v-model="userForm.surname" required /></label><label>Имя<input v-model="userForm.name" required /></label>
            <label>Отчество<input v-model="userForm.lastname" /></label><label>Возраст<input v-model="userForm.age" type="number" min="1" required /></label>
            <label>Код входа<input v-model="userForm.code" required /></label><label>Пароль<input v-model="userForm.pwd" type="password" required /></label>
            <label>Специальность<select v-model="userForm.spec" required><option value="" disabled>Выберите специальность</option><option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option></select></label>
            <label>Роль<select v-model="userForm.role"><option>User</option><option>Checker</option><option>Admin</option></select></label>
            <label class="span-two">Фотография<input type="file" accept="image/*" @change="setPhoto" /></label>
            <div class="span-two form-actions"><button class="primary" :disabled="submitting">Добавить пользователя</button></div>
          </form>
        </section>
        <section class="panel">
          <div class="panel-heading list-heading"><div><p class="eyebrow">Учётные записи</p><h2>Пользователи</h2></div><form class="search" @submit.prevent="loadUsers"><input v-model="userSearch" placeholder="Поиск пользователя" /><button aria-label="Найти">⌕</button></form></div>
          <div v-if="loading" class="empty-state">Загружаем пользователей…</div>
          <div v-else-if="!users.length" class="empty-state">Пользователи не найдены.</div>
          <div v-else class="table-wrap"><table><thead><tr><th>Имя</th><th>Возраст</th><th>Специальность</th><th>Роль</th><th></th></tr></thead>
            <tbody><tr v-for="user in users" :key="user.userid"><td><strong>{{ user.surname }} {{ user.name }} {{ user.lastname }}</strong></td><td>{{ user.age }}</td><td>{{ user.spec }}</td><td><span class="role-badge">{{ user.role }}</span></td><td class="row-actions"><button class="text-button" @click="openUserById(user.userid)">Просмотр</button><button class="icon-button danger-text" title="Удалить пользователя" @click="removeUser(user)">×</button></td></tr></tbody>
          </table></div>
        </section>
      </template>

      <template v-else-if="activeSection === 'user-detail' && isAdmin && selectedUser">
        <section class="panel detail-panel">
          <div class="panel-heading">
            <div><p class="eyebrow">Пользователь по ID</p><h2>{{ selectedUser.surname }} {{ selectedUser.name }} {{ selectedUser.lastname }}</h2></div>
            <div class="form-actions">
              <button class="secondary" @click="backToUsers">К списку пользователей</button>
              <button class="primary" @click="startUserEdit">Изменить</button>
            </div>
          </div>
          <article class="user-detail">
            <img v-if="selectedUser.photourl" :src="selectedUser.photourl" :alt="`${selectedUser.name} ${selectedUser.surname}`" />
            <div class="user-detail-info">
              <p class="eyebrow">User ID: {{ selectedUser.userid }}</p>
              <h3>{{ selectedUser.surname }} {{ selectedUser.name }} {{ selectedUser.lastname }}</h3>
              <p>{{ selectedUser.spec }} <span class="role-badge">{{ selectedUser.role }}</span></p>
              <small>Возраст: {{ selectedUser.age }}</small>
            </div>
          </article>
        </section>
      </template>

      <template v-else-if="activeSection === 'user-edit' && isAdmin && selectedUser">
        <section class="panel">
          <div class="panel-heading">
            <div><p class="eyebrow">User ID: {{ selectedUser.userid }}</p><h2>Изменить данные пользователя</h2></div>
            <button class="secondary" @click="cancelUserEdit">Отмена</button>
          </div>
          <form class="form-grid" @submit.prevent="saveUser">
            <label>Фамилия<input v-model="editUserForm.surname" autocomplete="family-name" required /></label>
            <label>Имя<input v-model="editUserForm.name" autocomplete="given-name" required /></label>
            <label>Отчество<input v-model="editUserForm.lastname" /></label>
            <label>Возраст<input v-model="editUserForm.age" type="number" min="1" required /></label>
            <label>Специальность
              <select v-model="editUserForm.spec" required>
                <option value="" disabled>Выберите специальность</option>
                <option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option>
              </select>
            </label>
            <div class="span-two form-actions">
              <button class="secondary" type="button" @click="cancelUserEdit">Отмена</button>
              <button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Сохранить изменения' }}</button>
            </div>
          </form>
        </section>
      </template>

      <template v-else-if="activeSection === 'directory' && isAdmin">
        <section class="directory-grid">
          <article class="panel">
            <div class="panel-heading"><div><p class="eyebrow">Команда</p><h2>Специальности</h2></div></div>
            <form class="inline-form" @submit.prevent="createSpeciality"><input v-model="specialityName" placeholder="Название специальности" required /><button class="primary" aria-label="Добавить">+</button></form>
            <div v-if="!specialities.length" class="empty-state compact">Список пуст.</div>
            <ul v-else class="item-list"><li v-for="spec in specialities" :key="spec.id"><span>{{ spec.name }}</span><button class="icon-button danger-text" title="Удалить" @click="removeDirectoryItem('speciality', spec.id)">×</button></li></ul>
          </article>
          <article class="panel">
            <div class="panel-heading"><div><p class="eyebrow">Каталог</p><h2>Услуги</h2></div></div>
            <form class="stack inline-stack" @submit.prevent="createService"><input v-model="serviceForm.name" placeholder="Название услуги" required /><select v-model="serviceForm.spec" required><option value="" disabled>Специальность</option><option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option></select><button class="primary">Добавить услугу</button></form>
            <div v-if="!services.length" class="empty-state compact">Список пуст.</div>
            <ul v-else class="item-list"><li v-for="service in services" :key="service.id"><span><strong>{{ service.name }}</strong><small>{{ service.spec }}</small></span><button class="icon-button danger-text" title="Удалить" @click="removeDirectoryItem('service', service.id)">×</button></li></ul>
          </article>
        </section>
      </template>
    </main>
  </div>
</template>

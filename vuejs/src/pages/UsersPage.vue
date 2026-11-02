<script setup lang="ts">
import { reactive, ref } from 'vue'
import type { Speciality, User } from '../api'

defineProps<{
  users: User[]
  specialities: Speciality[]
  loading: boolean
  submitting: boolean
}>()

const emit = defineEmits<{
  'load-users': [text: string]
  'open-user': [id: string]
  'delete-user': [user: User]
  'find-user': [id: string]
  'create-user': [data: FormData]
  error: [message: string]
}>()

const search = ref('')
const userId = ref('')
const form = reactive({
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

function setPhoto(event: Event) {
  form.photo = (event.target as HTMLInputElement).files?.[0] ?? null
}

function createUser() {
  if (!form.name || !form.surname || !form.code || !form.pwd || !form.spec || !form.age || !form.photo) {
    emit('error', 'Заполните обязательные поля и выберите фотографию.')
    return
  }
  const data = new FormData()
  for (const [key, value] of Object.entries(form)) {
    if (value !== null) data.append(key, value)
  }
  emit('create-user', data)
}

function resetForm() {
  Object.assign(form, {
    name: '', surname: '', lastname: '', code: '', pwd: '', spec: '', age: '', role: 'User', photo: null,
  })
}

defineExpose({ resetForm })
</script>

<template>
  <section class="panel">
    <div class="panel-heading"><div><p class="eyebrow">Карточка пользователя</p><h2>Найти пользователя по ID</h2></div></div>
    <form class="inline-form lookup-form" @submit.prevent="emit('find-user', userId)">
      <input v-model="userId" placeholder="UUID пользователя" required />
      <button class="primary">Найти</button>
    </form>
  </section>

  <section class="panel">
    <div class="panel-heading"><div><p class="eyebrow">Новая учётная запись</p><h2>Добавить пользователя</h2></div></div>
    <form class="form-grid" @submit.prevent="createUser">
      <label>Фамилия<input v-model="form.surname" required /></label><label>Имя<input v-model="form.name" required /></label>
      <label>Отчество<input v-model="form.lastname" /></label><label>Возраст<input v-model="form.age" type="number" min="1" required /></label>
      <label>Код входа<input v-model="form.code" required /></label><label>Пароль<input v-model="form.pwd" type="password" required /></label>
      <label>Специальность<select v-model="form.spec" required><option value="" disabled>Выберите специальность</option><option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option></select></label>
      <label>Роль<select v-model="form.role"><option>User</option><option>Checker</option><option>Admin</option></select></label>
      <label class="span-two">Фотография<input type="file" accept="image/*" @change="setPhoto" /></label>
      <div class="span-two form-actions"><button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Добавить пользователя' }}</button></div>
    </form>
  </section>

  <section class="panel">
    <div class="panel-heading list-heading"><div><p class="eyebrow">Учётные записи</p><h2>Пользователи</h2></div><form class="search" @submit.prevent="emit('load-users', search)"><input v-model="search" placeholder="Поиск пользователя" /><button aria-label="Найти">⌕</button></form></div>
    <div v-if="loading" class="empty-state">Загружаем пользователей…</div>
    <div v-else-if="!users.length" class="empty-state">Пользователи не найдены.</div>
    <div v-else class="table-wrap"><table><thead><tr><th>Имя</th><th>Возраст</th><th>Специальность</th><th>Роль</th><th></th></tr></thead>
      <tbody><tr v-for="user in users" :key="user.userid"><td><strong>{{ user.surname }} {{ user.name }} {{ user.lastname }}</strong></td><td>{{ user.age }}</td><td>{{ user.spec }}</td><td><span class="role-badge">{{ user.role }}</span></td><td class="row-actions"><button class="text-button" @click="emit('open-user', user.userid)">Просмотр</button><button class="icon-button danger-text" title="Удалить пользователя" @click="emit('delete-user', user)">×</button></td></tr></tbody>
    </table></div>
  </section>
</template>

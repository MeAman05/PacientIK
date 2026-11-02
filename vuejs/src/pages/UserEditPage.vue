<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { Speciality, User } from '../api'

const props = defineProps<{
  user: User
  specialities: Speciality[]
  submitting: boolean
}>()

const emit = defineEmits<{
  cancel: []
  save: [data: FormData]
  error: [message: string]
}>()

const form = reactive({ name: '', surname: '', lastname: '', spec: '', age: '' })

watch(
  () => [props.user, props.specialities] as const,
  ([user, specialities]) => {
    const speciality = specialities.find((item) => item.name === user.spec)
    if (!speciality) {
      emit('error', 'Не удалось определить специальность пользователя.')
      return
    }
    Object.assign(form, {
      name: user.name,
      surname: user.surname,
      lastname: user.lastname,
      spec: String(speciality.id),
      age: String(user.age),
    })
  },
  { immediate: true },
)

function save() {
  if (!form.name.trim() || !form.surname.trim() || !form.spec || !form.age) {
    emit('error', 'Заполните имя, фамилию, возраст и специальность.')
    return
  }
  const data = new FormData()
  data.append('Name', form.name.trim())
  data.append('Surname', form.surname.trim())
  data.append('Lastname', form.lastname.trim())
  data.append('Spec', form.spec)
  data.append('Age', form.age)
  emit('save', data)
}
</script>

<template>
  <section class="panel">
    <div class="panel-heading">
      <div><p class="eyebrow">User ID: {{ user.userid }}</p><h2>Изменить данные пользователя</h2></div>
      <button class="secondary" @click="emit('cancel')">Отмена</button>
    </div>
    <form class="form-grid" @submit.prevent="save">
      <label>Фамилия<input v-model="form.surname" autocomplete="family-name" required /></label>
      <label>Имя<input v-model="form.name" autocomplete="given-name" required /></label>
      <label>Отчество<input v-model="form.lastname" /></label>
      <label>Возраст<input v-model="form.age" type="number" min="1" required /></label>
      <label>Специальность
        <select v-model="form.spec" required>
          <option value="" disabled>Выберите специальность</option>
          <option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option>
        </select>
      </label>
      <div class="span-two form-actions">
        <button class="secondary" type="button" @click="emit('cancel')">Отмена</button>
        <button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Сохранить изменения' }}</button>
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import type { Service, Speciality } from '../api'

defineProps<{
  specialities: Speciality[]
  services: Service[]
}>()

const emit = defineEmits<{
  'add-speciality': [name: string]
  'add-service': [service: { name: string; spec: number }]
  'delete-item': [item: { kind: 'speciality' | 'service'; id: number }]
}>()

const specialityName = ref('')
const serviceForm = reactive({ name: '', spec: '' })

function addSpeciality() {
  const name = specialityName.value.trim()
  if (!name) return
  emit('add-speciality', name)
  specialityName.value = ''
}

function addService() {
  const name = serviceForm.name.trim()
  if (!name || !serviceForm.spec) return
  emit('add-service', { name, spec: Number(serviceForm.spec) })
  Object.assign(serviceForm, { name: '', spec: '' })
}
</script>

<template>
  <section class="directory-grid">
    <article class="panel">
      <div class="panel-heading"><div><p class="eyebrow">Команда</p><h2>Специальности</h2></div></div>
      <form class="inline-form" @submit.prevent="addSpeciality"><input v-model="specialityName" placeholder="Название специальности" required /><button class="primary" aria-label="Добавить">+</button></form>
      <div v-if="!specialities.length" class="empty-state compact">Список пуст.</div>
      <ul v-else class="item-list"><li v-for="spec in specialities" :key="spec.id"><span>{{ spec.name }}</span><button class="icon-button danger-text" title="Удалить" @click="emit('delete-item', { kind: 'speciality', id: spec.id })">×</button></li></ul>
    </article>
    <article class="panel">
      <div class="panel-heading"><div><p class="eyebrow">Каталог</p><h2>Услуги</h2></div></div>
      <form class="stack inline-stack" @submit.prevent="addService"><input v-model="serviceForm.name" placeholder="Название услуги" required /><select v-model="serviceForm.spec" required><option value="" disabled>Специальность</option><option v-for="spec in specialities" :key="spec.id" :value="String(spec.id)">{{ spec.name }}</option></select><button class="primary">Добавить услугу</button></form>
      <div v-if="!services.length" class="empty-state compact">Список пуст.</div>
      <ul v-else class="item-list"><li v-for="service in services" :key="service.id"><span><strong>{{ service.name }}</strong><small>{{ service.spec }}</small></span><button class="icon-button danger-text" title="Удалить" @click="emit('delete-item', { kind: 'service', id: service.id })">×</button></li></ul>
    </article>
  </section>
</template>

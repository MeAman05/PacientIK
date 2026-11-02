<script setup lang="ts">
import { reactive, ref } from 'vue'
import type { Report, Service } from '../api'

defineProps<{
  reports: Report[]
  services: Service[]
  role: string
  isAdmin: boolean
  loading: boolean
  submitting: boolean
}>()

const emit = defineEmits<{
  'load-reports': [name: string]
  'open-report': [id: number]
  'delete-report': [report: Report]
  'create-report': [report: { pacientname: string; price: number; lechids: number[]; period: number }]
  'lookup-report': [id: string]
  error: [message: string]
}>()

const search = ref('')
const reportId = ref('')
const form = reactive({ pacientname: '', price: '', period: '', lechids: [] as number[] })

function createReport() {
  if (!form.pacientname.trim() || !form.price || !form.period) {
    emit('error', 'Заполните имя пациента, цену и период.')
    return
  }
  emit('create-report', {
    pacientname: form.pacientname.trim(),
    price: Number(form.price),
    lechids: [...form.lechids],
    period: Number(form.period),
  })
}

function resetForm() {
  Object.assign(form, { pacientname: '', price: '', period: '', lechids: [] })
}

defineExpose({ resetForm })
</script>

<template>
  <section class="summary-row">
    <article class="summary-card"><span class="summary-icon violet">▤</span><div><small>{{ isAdmin || role === 'Checker' ? 'Всего отчётов' : 'Мои отчёты' }}</small><strong>{{ reports.length }}</strong></div></article>
    <article class="summary-card"><span class="summary-icon green">₽</span><div><small>Сумма по списку</small><strong>{{ reports.reduce((sum, item) => sum + item.price, 0).toLocaleString('ru-RU') }} ₽</strong></div></article>
    <article class="summary-card"><span class="summary-icon amber">✳</span><div><small>Доступные услуги</small><strong>{{ services.length || '—' }}</strong></div></article>
  </section>

  <section class="panel lookup-panel">
    <div class="panel-heading"><div><p class="eyebrow">Быстрый доступ</p><h2>Открыть отчёт по ID</h2></div></div>
    <form class="inline-form lookup-form" @submit.prevent="emit('lookup-report', reportId)">
      <input v-model="reportId" type="number" min="1" placeholder="ID отчёта" required />
      <button class="primary">Открыть</button>
    </form>
  </section>

  <section v-if="!isAdmin" class="panel">
    <div class="panel-heading"><div><p class="eyebrow">Новая запись</p><h2>Создать отчёт</h2></div></div>
    <form class="form-grid" @submit.prevent="createReport">
      <label class="span-two">Имя пациента<input v-model="form.pacientname" placeholder="Например, Иван Иванов" required /></label>
      <label>Стоимость, ₽<input v-model="form.price" type="number" min="0" step="0.01" required /></label>
      <label>Период<input v-model="form.period" type="number" min="1" required /></label>
      <fieldset class="span-two service-picker"><legend>Услуги</legend>
        <label v-for="service in services" :key="service.id" class="check-option">
          <input v-model="form.lechids" type="checkbox" :value="service.id" />{{ service.name }}<small>{{ service.spec }}</small>
        </label>
        <span v-if="!services.length" class="muted">Список услуг не загружен.</span>
      </fieldset>
      <div class="span-two form-actions"><button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Создать отчёт' }}</button></div>
    </form>
  </section>

  <section class="panel">
    <div class="panel-heading list-heading"><div><p class="eyebrow">История</p><h2>{{ isAdmin || role === 'Checker' ? 'Все отчёты' : 'Ваши отчёты' }}</h2></div>
      <form class="search" @submit.prevent="emit('load-reports', search)"><input v-model="search" placeholder="Поиск по пациенту" /><button aria-label="Найти">⌕</button></form>
    </div>
    <div v-if="loading" class="empty-state">Загружаем отчёты…</div>
    <div v-else-if="!reports.length" class="empty-state"><span>▤</span><strong>Отчётов пока нет</strong><small>Когда появятся записи, они отобразятся здесь.</small></div>
    <div v-else class="table-wrap"><table><thead><tr><th>Пациент</th><th>Специалист</th><th>Услуги</th><th>Период</th><th>Стоимость</th><th></th></tr></thead>
      <tbody><tr v-for="report in reports" :key="report.id"><td><strong>{{ report.pacientname }}</strong><small class="cell-subtitle">Отчёт №{{ report.id }}</small></td><td>{{ report.sender }}</td><td>{{ report.leches.join(', ') || '—' }}</td><td>{{ report.period }}</td><td class="price">{{ report.price.toLocaleString('ru-RU') }} ₽</td><td class="row-actions"><button class="text-button" @click="emit('open-report', report.id)">Открыть</button><button v-if="!isAdmin" class="icon-button danger-text" title="Удалить отчёт" @click="emit('delete-report', report)">×</button></td></tr></tbody>
    </table></div>
  </section>
</template>

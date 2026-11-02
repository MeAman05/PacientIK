<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { Report, Service } from '../api'

const props = defineProps<{
  report: Report
  services: Service[]
  submitting: boolean
}>()

const emit = defineEmits<{
  cancel: []
  save: [report: { pacientname: string; price: number; lechids: number[]; period: number }]
  error: [message: string]
}>()

const form = reactive({ pacientname: '', price: '', period: '', lechids: [] as number[] })

watch(() => props.report, (report) => {
  Object.assign(form, {
    pacientname: report.pacientname,
    price: String(report.price),
    period: String(report.period),
    lechids: [...report.lechids],
  })
}, { immediate: true })

function save() {
  if (!form.pacientname.trim() || !form.price || !form.period) {
    emit('error', 'Заполните имя пациента, цену и период.')
    return
  }
  emit('save', {
    pacientname: form.pacientname.trim(),
    price: Number(form.price),
    lechids: [...form.lechids],
    period: Number(form.period),
  })
}
</script>

<template>
  <section class="panel">
    <div class="panel-heading"><div><p class="eyebrow">Отчёт №{{ report.id }}</p><h2>Изменить данные отчёта</h2></div><button class="secondary" @click="emit('cancel')">Отмена</button></div>
    <form class="form-grid" @submit.prevent="save">
      <label class="span-two">Имя пациента<input v-model="form.pacientname" required /></label>
      <label>Стоимость, ₽<input v-model="form.price" type="number" min="0" step="0.01" required /></label>
      <label>Период<input v-model="form.period" type="number" min="1" required /></label>
      <fieldset class="span-two service-picker"><legend>Услуги</legend>
        <label v-for="service in services" :key="service.id" class="check-option">
          <input v-model="form.lechids" type="checkbox" :value="service.id" />{{ service.name }}<small>{{ service.spec }}</small>
        </label>
        <span v-if="!services.length" class="muted">Список услуг не загружен.</span>
      </fieldset>
      <div class="span-two form-actions"><button class="secondary" type="button" @click="emit('cancel')">Отмена</button><button class="primary" :disabled="submitting">{{ submitting ? 'Сохраняем…' : 'Сохранить изменения' }}</button></div>
    </form>
  </section>
</template>

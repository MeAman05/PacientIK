<script setup lang="ts">
import type { Report } from '../api'

defineProps<{ report: Report }>()

const emit = defineEmits<{
  back: []
  edit: []
}>()
</script>

<template>
  <section class="panel detail-panel">
    <div class="panel-heading"><div><p class="eyebrow">Информация по отчёту</p><h2>{{ report.pacientname }}</h2></div>
      <div class="form-actions"><button class="secondary" @click="emit('back')">К списку</button><button class="primary" @click="emit('edit')">Редактировать</button></div>
    </div>
    <dl class="detail-grid">
      <div><dt>ID отчёта</dt><dd>{{ report.id }}</dd></div>
      <div><dt>Пациент</dt><dd>{{ report.pacientname }}</dd></div>
      <div><dt>Специалист</dt><dd>{{ report.sender }}</dd></div>
      <div><dt>Период</dt><dd>{{ report.period }}</dd></div>
      <div><dt>Стоимость</dt><dd>{{ report.price.toLocaleString('ru-RU') }} ₽</dd></div>
      <div class="span-two"><dt>Услуги</dt><dd>{{ report.leches.join(', ') || 'Не указаны' }}</dd></div>
    </dl>
  </section>
</template>

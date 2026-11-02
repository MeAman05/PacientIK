<script setup lang="ts">
import type { User } from '../api'

defineProps<{ user: User }>()

const emit = defineEmits<{
  back: []
  edit: []
}>()
</script>

<template>
  <section class="panel detail-panel">
    <div class="panel-heading">
      <div><p class="eyebrow">Пользователь по ID</p><h2>{{ user.surname }} {{ user.name }} {{ user.lastname }}</h2></div>
      <div class="form-actions"><button class="secondary" @click="emit('back')">К списку пользователей</button><button class="primary" @click="emit('edit')">Изменить</button></div>
    </div>
    <article class="user-detail">
      <img v-if="user.photourl" :src="user.photourl" :alt="`${user.name} ${user.surname}`" />
      <div class="user-detail-info">
        <p class="eyebrow">User ID: {{ user.userid }}</p>
        <h3>{{ user.surname }} {{ user.name }} {{ user.lastname }}</h3>
        <p>{{ user.spec }} <span class="role-badge">{{ user.role }}</span></p>
        <small>Возраст: {{ user.age }}</small>
      </div>
    </article>
  </section>
</template>

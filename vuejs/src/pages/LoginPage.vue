<script setup lang="ts">
import { ref } from 'vue'

defineProps<{
  submitting: boolean
  error: string
}>()

const emit = defineEmits<{
  submit: [credentials: { code: string; password: string }]
}>()

const code = ref('')
const password = ref('')

function submit() {
  emit('submit', { code: code.value, password: password.value })
}
</script>

<template>
  <main class="login-page">
    <section class="login-card">
      <div class="brand-mark">P</div>
      <p class="eyebrow">Рабочее пространство</p>
      <h1>PacientIK</h1>
      <p class="muted">Войдите, чтобы продолжить</p>
      <form class="stack" @submit.prevent="submit">
        <label>Код пользователя<input v-model="code" autocomplete="username" required /></label>
        <label>Пароль<input v-model="password" type="password" autocomplete="current-password" required /></label>
        <button class="primary" :disabled="submitting">{{ submitting ? 'Входим…' : 'Войти' }}</button>
      </form>
      <p v-if="error" class="message error">{{ error }}</p>
      <p class="login-footnote">Доступ защищён авторизацией API.</p>
    </section>
  </main>
</template>

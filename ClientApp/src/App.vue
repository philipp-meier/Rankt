<script lang="ts" setup>
import { RouterView, useRouter } from 'vue-router';
import Menubar from 'primevue/menubar';
import { computed } from 'vue';
import { config } from './config';
import Toast from 'primevue/toast';

import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores/authStore';

const router = useRouter();

const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore);

const menuItems = computed(() => {
  let items = [
    {
      label: 'Home',
      icon: 'pi pi-home',
      command: () => router.push({ name: 'home' }),
      requiresAuth: false
    },
    {
      label: 'Admin',
      icon: 'pi pi-shield',
      command: () => router.push({ name: 'admin' }),
      requiresAuth: true
    },
    {
      label: 'Logout',
      icon: 'pi pi-sign-out',
      command: () => router.push({ name: 'logout' }),
      requiresAuth: true
    }
  ].filter((x) => !x.requiresAuth || isAuthenticated.value);

  if (!isAuthenticated.value) {
    items.push({
      label: 'Login',
      icon: 'pi pi-user',
      command: () => router.push({ name: 'login' }),
      requiresAuth: false
    });
  }

  return items;
});
</script>

<template>
  <Menubar :model="menuItems" />
  <Toast position="top-right" />
  <main>
    <RouterView />
  </main>
  <footer>
    <div class="footer-container">
      <div class="github">
        <i class="pi pi-github" />
        <span>{{ config.AppTitle }}</span>
      </div>
    </div>
  </footer>
</template>

<style scoped>
footer {
  width: 100%;
  bottom: 0;
  position: fixed;
  background: white;

  div.footer-container {
    margin: 0.5em;

    div.github {
      display: flex;
      align-items: center;

      .pi {
        margin-right: 0.25em;
      }
    }
  }
}
</style>

import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import LogoutView from '@/views/LogoutView.vue';
import QuestionView from '@/views/QuestionView.vue';
import AdminView from '@/views/AdminView.vue';
import { useAuthStore } from '@/stores/authStore';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/logout',
      name: 'logout',
      component: LogoutView
    },
    {
      path: '/question',
      name: 'question',
      component: QuestionView
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminView
    }
  ]
});

router.beforeEach(async (to, from, next) => {
  const publicRoutes = ['home', 'login', 'question'];

  const authStore = useAuthStore();
  await authStore.refreshState();

  if (authStore.isAuthenticated) next();
  else if (to.name != null && publicRoutes.indexOf(to.name.toString()) >= 0) next();
  else next({ name: 'login' });
});

export default router;

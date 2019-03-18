import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'
import router from './router'
import EventBus from './services/EventBus'


Vue.config.productionTip = false

Vue.prototype.$bus = EventBus

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')


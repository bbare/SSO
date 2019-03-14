import Vue from 'vue'
import Vuex from 'vuex'
import CreatePersistedState from 'vuex-persistedstate'
import User from './modules/User'
Vue.use(Vuex)

const crtPersState = new CreatePersistedState({
    key: 'KFCStore',// The key to store the state on in the storage provider.
    storage: window.sessionStorage,
    reducer: state => ({
      User: state.User
    })
  })

const store = new Vuex.Store({
    modules:{
        User
    },
    plugins: [crtPersState]
  })
  export default store
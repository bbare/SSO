const state = {
    email: '',
    token: '',
    isLogin: false
  }
  
  const getters = {
    getEmail: function (state) {
      return state.email
    },
    getToken: function (state) {
      return state.token
    },
    getIsLogin: function (state) {
      return state.isLogin
    }
  }
  
  const actions = {
    emailAction (context, payload) {
      context.commit('mutateEmail', payload)
    },
    tokenAction (context, payload) {
      context.commit('mutateToken', payload)
    },
    isLoginAction (context, payload) {
      context.commit('mutateIsLogin', payload)
    }
  }
  
  const mutations = {
    mutateEmail (state, payload) {
      state.email = payload.Email
    },
    mutateToken (state, payload) {
      state.token = payload.Token
    },
    mutateIsLogin (state, payload) {
      state.isLogin = payload.IsLogin
    }
  }
  
  export default {
    state,
    getters,
    actions,
    mutations
  }
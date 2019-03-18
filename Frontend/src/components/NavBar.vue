<template>
  <v-toolbar app>
    <v-toolbar-title class="headline text-uppercase">
      <span>KFC SSO</span>
    </v-toolbar-title>
    <v-spacer></v-spacer>

    <v-btn to="home" flat>Home</v-btn>
    <v-btn to="register" flat>Register</v-btn>
    <v-btn to="about" flat>About</v-btn>
    <v-btn to="add" flat>Register</v-btn>
    <v-btn to="key" flat>Generate</v-btn>
    <v-btn to="delete" flat>Delete</v-btn>
    <v-btn to="login" flat v-if="isLogged !== true">Login</v-btn>
  </v-toolbar>
</template>

<script>
export default {
  name: 'NavBar',
  data () {
    return{
      isLogged: this.checkIfIsLogged()
    }
  },
  created () {
    this.$bus.$on('logged', () => {
      this.isLogged = this.checkIfIsLogged()
    })
  },
  methods: {
    checkIfIsLogged () {
      let token = localStorage.getItem('token')
      if (token) {
        return true
      } else {
        return false
      }
    }
  }
}
</script>


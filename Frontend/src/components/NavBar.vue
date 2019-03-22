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
    <v-btn to="login" flat v-if="!isLogged">Login</v-btn>
    <v-menu
      offset-y
      content-class="dropdown-menu"
      transition="slide-y-transition" v-if="isLogged">

      <v-btn slot="activator" fab dark color="teal">
        <v-avatar dark>
          <span class="white--text headline">fg</span>
        </v-avatar>
      </v-btn>
      
      <v-card>
        <v-list dense>
          <v-list-tile
            v-for="link in links"
            :key="link"
          >
          <v-list-tile-title
            v-text="link"
          />
          </v-list-tile>
        </v-list>
      </v-card>
    </v-menu>

  </v-toolbar>
</template>

<script>
export default {
  name: 'NavBar',
  data () {
    return{
      links: [],
      emailInitial: "",
      isLogged: this.checkIfIsLogged()
    }
  },
  created () {
    this.$bus.$on('logged', () => {
      this.isLogged = this.checkIfIsLogged()
      var email = localStorage.getItem('email')
      this.emailInitial = email[0] + email[1] 
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


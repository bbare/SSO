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
    <v-btn to="login" flat v-if="!this.$data.isLoggedIn">Login</v-btn>
    <v-menu
      offset-y
      content-class="dropdown-menu"
      transition="slide-y-transition" v-if="this.$data.isLoggedIn">

      <v-btn slot="activator" fab dark color="teal">
        <v-avatar dark>
          <span class="white--text headline">{{this.$data.emailInitial}}</span>
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
import axios from "axios"
import { apiURL } from '@/const.js'

export default {
  name: 'NavBar',
  data () {
    return{
      links: [],
      emailInitial: '',
      isLoggedIn: false
    }
  },
  watch: {
    isLoggedIn: function() {
      if(localStorage.getItem('token')){
            this.$data.isLoggedIn = true
            const url = `${apiURL}/users/${localStorage.token}`
            axios.get(url,
            {
                params: {
                  token: localStorage.getItem('token')
                }      
            })
            .then(resp => {
                this.$data.emailInitial = resp.data
                console.log(this.$data.emailInitial)
                console.log(localStorage.getItem('token'))
            })
            .catch(e => {console.log(e);
            })
          }
          else{
            this.isLoggedIn = false
          }
      }
  },
  mounted(){
    /*if(localStorage.getItem('token')){
      this.$data.isLoggedIn = true
      const url = `${apiURL}/users/${localStorage.token}`
      axios.get(url,
      {
          params: {
            token: localStorage.getItem('token')
          }      
      })
      .then(resp => {
          //store.isLogin()
          //store.state.emailIni = resp.data
          this.$data.emailInitial = resp.data
          console.log(this.$data.emailInitial)
          console.log(localStorage.getItem('token'))
      })
      .catch(e => {console.log(e);
      })
    }
    else{
      this.isLoggedIn = false
    }*/
  },
  created () {
  },
  methods: {
  }
}
</script>


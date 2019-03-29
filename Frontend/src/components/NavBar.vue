<template>
  <v-toolbar app>
    <v-toolbar-title class="headline text-uppercase">
      <span>KFC SSO</span>
    </v-toolbar-title>
    <v-spacer></v-spacer>

    <v-btn to="home" flat>Home</v-btn>
    <v-btn to="register" flat v-if="!isLoggedIn.isLogin">Register</v-btn>
    <v-btn to="about" flat>About</v-btn>
    <v-btn to="add" flat >App Register</v-btn>
    <v-btn to="key" flat>Generate Key</v-btn>
    <v-btn to="delete" flat>App Delete</v-btn>
    <v-btn to="login" flat v-if="!isLoggedIn.isLogin">Login</v-btn>
    <v-menu
      offset-y
      content-class="dropdown-menu"
      transition="slide-y-transition" v-if="isLoggedIn.isLogin">
      <v-btn slot="activator" fab dark color="teal">
        <v-avatar dark>
          <span class="white--text headline">{{isLoggedIn.email[0]}}</span>
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
import { store } from '@/services/request'

export default {
  name: 'NavBar',
  data () {
    return{
      links: [],
      isLoggedIn: store.state
    }
  },
  mounted(){
    store.isUserLogin()
    if(store.state.isLogin === true){
      store.getEmail()
    }
  }
}
</script>


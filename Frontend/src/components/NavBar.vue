<template>
  <v-toolbar app>
    <v-toolbar-title class="headline text-uppercase">
      <span>KFC SSO</span>
    </v-toolbar-title>
    <v-spacer></v-spacer>

    <v-btn to="home" flat>Home</v-btn>
    <v-btn to="register" flat v-if="!isLoggedIn.isLogin">Register</v-btn>
    <v-btn to="about" flat>About</v-btn>
    <v-menu open-on-hover top offset-y id="appDropDown">
      <template slot="activator">
        <v-btn v-on="on"
                flat>
          <span>Application</span>
          <v-icon>expand_more</v-icon>
        </v-btn>
      </template>
      <v-list>
        <v-list-tile v-for="link in appLinks"
                      :key="link.text"
                      router :to="link.route">
          <v-list-tile-title>{{link.text}}</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-menu>
    <v-btn to="login" flat v-if="!isLoggedIn.isLogin">Login</v-btn>
    <v-menu v-else offset-y
            content-class="dropdown-menu"
            transition="slide-y-transition" v-if="isLoggedIn.isLogin">
      <v-btn slot="activator" fab dark color="teal">
        <v-avatar dark>
          <span class="white--text headline">{{isLoggedIn.email[0]}}</span>
        </v-avatar>
      </v-btn>
      <v-card>
        <v-list dense>
          <v-list-tile v-for="link in links"
                        :key="link">
            <v-list-tile-title v-text="link" />
          </v-list-tile>
        </v-list>
      </v-card>
    </v-menu>
  </v-toolbar>
</template>

<script>
  import { apiURL } from '@/const.js'
  import { store } from '@/services/request'
  export default {
    name: 'NavBar',
    data() {
      return {
        appLinks: [
            { text: 'Register', route: '/add' },
            { text: 'Generate Key', route: '/key' },
            { text: 'Delete', route: '/delete' },
        ],
        links: [],
        isLoggedIn: store.state
      }
    },
    mounted() {
      store.isUserLogin()
      if (store.state.isLogin === true) {
          store.getEmail()
      }
    }
  }
</script>
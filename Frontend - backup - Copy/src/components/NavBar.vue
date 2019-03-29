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
    <!--<v-btn to="login" flat v-if="!this.$data.isLoggedIn">Login</v-btn>-->
    <!--<v-btn to="login" flat v-show="!this.$data.isLoggedIn">Login</v-btn>-->
    <v-btn to="login" flat v-if="!isLoggedIn.isLogin">Login</v-btn>
    <!--isLogin2===true-->
    <v-menu
      offset-y
      content-class="dropdown-menu"
      transition="slide-y-transition" v-if="isLoggedIn.isLogin">
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
import { store } from '@/services/request'

export default {
  name: 'NavBar',
  data () {
    return{
      links: [],
      emailInitial: '',
      //isLoggedIn: false
      isLoggedIn: store.state
    }
  },
/*  computed: {
    isLogin2 : function(){
      if(localStorage.getItem('token') !== null){
        this.isLoggedIn = true
        return this.isLoggedIn
      }
      else{
        this.isLoggedIn = false
        return this.isLoggedIn
      }
    }
  },*/
  mounted(){
    //store.isLoginNavbar()
    console.log(localStorage.getItem('token'))
    console.log('Logged in: ' + this.isLoggedIn.isLogin)
    //hideMenu({token: localStorage.getItem('token')}).then(() => )
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
    /*
    isLogin2: function () {
      console.log(localStorage.getItem('token'))
      if(localStorage.getItem('token')){
        return true
      }
      else{
        return false
      }
    }
    */
  }
}
</script>


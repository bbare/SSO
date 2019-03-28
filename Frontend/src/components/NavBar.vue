<template>
  <v-toolbar app>
    <v-toolbar-title class="headline text-uppercase">
      <span>KFC SSO</span>
    </v-toolbar-title>
    <v-spacer></v-spacer>

    <v-btn to="home" flat>Home</v-btn>
    <v-btn to="register" flat>Register</v-btn>
    <v-btn to="about" flat>About</v-btn>
    <v-btn to="login" flat>Login</v-btn>

    <v-menu open-on-hover top offset-y id="appDropDown">
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          flat
        >
          <span>Application</span>
          <v-icon>expand_more</v-icon>
        </v-btn>
      </template>
  
      <v-list>
        <v-list-tile 
          v-for="link in appLinks" 
          :key="link.text" 
          router :to="link.route"
        >
          <v-list-tile-title>{{appLinks.text}}</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-menu>
  </v-toolbar>
</template>

<script>
export default {
  name: 'NavBar',
  data(){
    return {
      appLinks: [
        {text: 'Register', route: '/add'},
        {text: 'Generate Key', route: '/key'},
        {text: 'Delete', route: '/delete'},
      ]
    }
  }
}
</script>



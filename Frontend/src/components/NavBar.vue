<template>
  <v-toolbar app>
    <v-toolbar-title class="headline text-uppercase">
      <span>KFC SSO</span>
    </v-toolbar-title>
    <v-spacer></v-spacer>

    <v-btn to="home" flat>Home</v-btn>
    <v-btn to="register" flat>Register</v-btn>
    <v-btn to="about" flat>About</v-btn>
    <v-btn to="add" flat>App Register</v-btn>
    <v-btn to="key" flat>Generate Key</v-btn>
    <v-btn to="delete" flat>App Delete</v-btn>
    <v-btn to="login" flat>Login</v-btn>
  </v-toolbar>
</template>

<script>
export default {
  name: 'NavBar'
}
</script>


<template>
  <div class="nav-container">
    <span class="title">KFC SSO</span>
    <router-link to="home">Home</router-link>
    <router-link to="about">About</router-link>
    <router-link to="app-register">Register App</router-link>
  </div>
</template>

<script>
export default {
  name: 'NavBar'
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  .nav-container {
    width: 100%;
    background-color: rgb(34,34,34);
    height: 50px;
    line-height: 50px;
    color: white;
  }
  .nav-container > .title {
    margin-left: 1rem;
    margin-right: 1rem;
  }
  .nav-container > a {
    display: inline-block;
    color: whitesmoke;
    text-decoration: none;
    padding-left: 1rem;
    padding-right: 1rem;
  }
  .nav-container > a.router-link-active {
    background-color: rgb(9,9,9);
    color: white;
  }
</style>

<template>
  <v-flex xs12 sm6 offset-sm3>
    <img src="@/assets/KFC_Logo_v2.png" id="logo"/>
    <h1 align="center" id="text">Welcome to your single sign on portal!</h1>
  </v-flex>
</template>

<script>
export default {
  name: 'Home'
}
</script>

<style scoped>
#logo{
  width: 100%;
  height: 100%;
  align: center;
}

#text{
  align: center;
  padding: 10px;
  width: 100%;
}
</style>
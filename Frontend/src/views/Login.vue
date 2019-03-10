<template>
    <div id="login">
        <h1>Login</h1>
        <br/>
        <input type="text" name="username" v-model="input.username" placeholder="Username" />
        <br/><br/>
        <input type="password" name="password" v-model="input.password" placeholder="Password" />
        <br/><br/>
        <button type="button" v-on:click="login()">Login</button>
    </div>
</template>

<script>
    import axios from "axios"
    
    export default {
        name: 'login',
        data() {
            return {
                input: {
                    username: "",
                    password: ""
                },
                pageError: ''
            }
        },
        methods: {
            login() {
               axios.post('http://localhost:50803/api/users/login',
               {input: this.input})
               .then(input => {this.input = input.data; console.log("sent!"); this.$router.push('/dashboard')
                })
               .catch(e => {console.log(e); this.$router.push('/home')
            })
        }
    }
}
</script>

<style>

#login {
  padding: 70px 0;
  text-align: center;
}

</style>

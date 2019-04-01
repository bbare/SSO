<template>
   <div class="login-wrapper">

        <h1>Login</h1>

        <br />
        <v-form>
        <v-text-field
            name="email"
            id="email"
            v-model="email"
            type="email"
            label="email" 
            /><br />
        <v-text-field
            name="password"
            id="password"
            type="password"
            v-model="password"
            label="Password" 
            /><br />

        
        <v-alert
            :value="error"
            type="error"
            transition="scale-transition"
        >
            {{error}}
        </v-alert>

        <br />

        <v-btn color="success" v-on:click="login">Login</v-btn>

        <v-btn color="success" v-on:click="goToResetPassword">Reset Password</v-btn>
        </v-form>

    </div>
</template>

<script>
    import axios from "axios"
    import { apiURL } from '@/const.js'
    import { store } from '@/services/request'
    
    export default {
        name: 'login',
        data() {
            return {
                email: "",
                password: "",
                error: ""
            }
        },
        methods: {
            login() {
               const url = `${apiURL}/users/login`
               axios.post(url,
               {
                    email: this.$data.email,
                    password: this.$data.password
               })
               .then(resp => {
                   let respData = resp.data
                   localStorage.setItem('token', respData)
                   store.state.isLogin = true
                   store.getEmail()
                   this.$router.push('/dashboard')
                })
               .catch(e => {console.log(e);
                    if(e.response.status === 400){
                        this.error = "Invalid Username/Password"
                    }
                    else if(e.response.status === 401){
                        this.error = "User is Disabled"
                    }
                    else{
                        this.error = e.response.data
                    }
            })
        },
        goToResetPassword(){
            this.$router.push('/sendresetlink')
        }
    }
}
</script>

<style>
    .login-wrapper {
        width: 70%;
        margin: 1px auto;
    }
</style>

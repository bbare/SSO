<template>
    <div class="login-wrapper">
        <form class="form-login" @submit.prevent="login">
            <h2 class="form-login-heading">Login</h2>
            <input v-model="email" id="email" type="email" class="form-control" placeholder="Email" required>
            <input v-model="password" id="password" type="password" class="form-control" placeholder="Password" required>
            <button class="button-login" type="submit">Login</button>
        </form>
        <button @click="goToResetPassword()" type="submit" >Reset Password</button>
    </div>
</template>

<script>
    import axios from "axios"
    import { apiURL } from '@/const.js'
    
    export default {
        name: 'login',
        data() {
            return {
                email: "",
                password: ""
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
                   localStorage.email = this.email;
                   localStorage.token = resp.data; 
                   this.$router.push('/dashboard')
                })
               .catch(e => {console.log(e);
                    if(e.response.status === 404){
                        alert("User Not Found")
                    }
                    else if(e.response.status === 401){
                        alert("User is Disabled")
                    }
                    else if(e.response.status === 400){ 
                        alert("Invalid Password")
                    }
                    else{
                        alert("Bad Reqiest or Conflict")
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
        background: #fff;
        width: 100%;
        height: 100%;
        margin: 1px auto;
        text-align: center;
    }

    .form-login {
        max-width: 330px;
        padding: 5% 10px;
        margin: 0 auto;
    }

    .form-login .form-control {
        position: relative;
        height: auto;
        -webkit-box-sizing: border-box;
            box-sizing: border-box;
        padding: 10px;
        font-size: 16px;
    }

    .form-login .form-control:focus {
        z-index: 2;
    }

    .form-login input {
        border: .5px solid #555;
        width: 100%;
        padding: 12px 20px;
        margin: 8px 0;
        box-sizing: border-box;
    }

    .form-login button {
        background: #778899;
        width: 100%;
        height: 40px;
    }
</style>

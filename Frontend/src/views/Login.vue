<template>
    <div id="login">
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
                }
            }
        },
        mounted(){

        },
        methods: {
            login() {
               axios.post('http://localhost:50803/api/users/login').then(input => {this.input = input.data; console.log("sent!");})
            }
        }
    }
</script>
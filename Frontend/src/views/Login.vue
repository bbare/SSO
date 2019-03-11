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
                }
            }
        },
        methods: {
            login() {
               axios.post('http://localhost:60461/api/users/login',
               {
                    email: this.input.username,
                    password: this.input.password
               })
               .then(i => {this.input = i.data; alert("Login Succesful"); console.log("Login Succesful"); this.$router.push('/dashboard')
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

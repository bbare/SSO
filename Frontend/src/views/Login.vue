<template>
    <div id="login">
        <h1>Login</h1>
        <br/>
        <input type="text" name="email" v-model="email" placeholder="Email" />
        <br/><br/>
        <input type="password" name="password" v-model="password" placeholder="Password" />
        <br/><br/>
        <button type="button" v-on:click="login()">Login</button>
    </div>
</template>

<script>
    import axios from "axios"
    //import dev_const from '../const.js'
    
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
               axios.post('http://localhost:61348/api/users/login',
               {
                    email: this.$data.email,
                    password: this.$data.password
               })
               .then(resp => {
                   //this.input = resp.data; 
                   this.$store.dispatch('emailAction',{Email: this.$data.email}) //ADD MORE STUFF HERE 
                   this.$store.dispatch('tokenAction',{Token: resp.data})
                   this.$store.dispatch('isLoginAction',{IsLogin: true})
                   console.log("Login Succesful"); 
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

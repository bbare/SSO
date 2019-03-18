<template>
  <div class="sendLink">
    <h1>Reset Password</h1>
    <br />
    {{message}}
    <br /><br />
    {{errorMessage}}
    <br/>
    <input name="email" type="text" v-model="email" placeholder="Email"/>
    <br />
    <br />
    <button type="submit" v-on:click="submitEmail">Send Email</button>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'SendResetLink',
  data () {
    return {
      errorMessage: null,
      message: 'Input email to send the reset link to:',
      email: null
    }
  },
  methods: {
    submitEmail: function () {
      if (!this.email) {
        alert('Email field cannot be empty')
      } else if (!this.validEmail(this.email)) {
        alert('Valid email required.')
      } else {
        axios({
          method: 'POST',
          url: 'http://localhost:61348/api/reset/send',
          data: {email: this.$data.email, url: this.$data.email},
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
          .then(response => {this.message = response.data})
          .catch(e => { this.errorMessage = e }, response => {this.message = response.data})
      }
    },
    validEmail: function (email) {
      var re = /^(([^<>()\\\\.,;:\s@"]+(\.[^<>()\\\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return re.test(email)
    }

  }
}
</script>

<style>
.sendLink{
  padding: 70px 0;
  text-align: center;
}

input[type=text] {
  border: 2px solid  rgb(69, 72, 75);
  border-radius: 4px;
}
</style>

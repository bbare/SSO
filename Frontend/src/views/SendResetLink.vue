<template>
  <div class="reset">
     <h1>Reset Password</h1>
    <br />
    {{message}}
    <br /><br />
      <p v-if="errors.length">
        <b>Please correct the following error(s):</b>
          <ul>
            <li v-for="(error, index) in errors" :key="index">
              {{ error }}
            </li>
          </ul>
      </p>
    <input
      name="email"
      type="text"
      v-model="email"
      placeholder="Email"
      />
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
      errors: [],
      message: 'Input email to send the reset link to:',
      email: null
    }
  },
  methods: {
    submitEmail: function (e) {
      if (!this.email) {
        this.errors.push('Email required.')
      } else if (!this.validEmail(this.email)) {
        this.errors.push('Valid email required.')
      } else {
        this.errors = []
        this.message = 'An email with further instructions has been sent to the email address inputted'

        axios({
          method: 'POST',
          url: 'http://localhost:61348/api/reset/send',
          data: {email: this.$data.email, url: this.$data.email},
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
          .then(response => {})
          .catch(e => { this.errors.push(e) })
      }
    },
    validEmail: function (email) {
      var re = /^(([^<>()\\\\.,;:\s@"]+(\.[^<>()\\\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return re.test(email)
    }

  }
}
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>

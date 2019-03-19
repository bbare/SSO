<template>
  <div id="update">
     <h1>Update Password</h1>
    <br />
    {{message}}
    <br />
    <br />
    <div class="submitPasswords">
        Old Password
        <input name="oldPassword" type="text" v-model="oldPassword"/>
        <br/>
        New Password
        <input name="newPassword" type="text" v-model="newPassword" />
        <br/>
        Confirm New Password
        <input name="confirmNewPassword" type="text" v-model="confirmNewPassword"/>
        <br />
        <br/>
        <button type="submit" v-on:click="submitPasswords">Update Password</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { apiURL } from '@/const.js';

export default {
  name: 'UpdatePassword',
  data () {
    return {
      message: 'Enter the new password:',
      oldPassword: null,
      newPassword: null,
      confirmNewPassword: null
    }
  },
  methods: {
    submitPasswords: function () {
      if (this.newPassword.length < 12 || this.oldPassword.length < 12) {
        alert('Password does not meet minimum length of 12')
      } else if (this.newPassword.length > 2000 || this.oldPassword.length > 2000) {
        alert('Password exceeds maximum length of 2000')
      } else if (this.confirmNewPassword !== this.newPassword) {
        alert('Passwords do not match')
      } else {
        this.message = 'Updating Password'
        axios({
          method: 'POST',
          url: `${apiURL}/users/updatepassword`,
          data: {
            emailAddress: localStorage.email,
            sessionToken: localStorage.token,
            oldPassword: this.$data.oldPassword,
            newPassword: this.$data.confirmNewPassword
          },
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
          .then(response => {alert(response.data)})
          .catch(e => {alert(e.response.data)})
      }
    }
  }
}
</script>

<style>

#update{
  padding: 70px 0;
  text-align: center;
}

input[type=text] {
  border: 2px solid rgb(123, 171, 226);
  border-radius: 4px;
}

</style>

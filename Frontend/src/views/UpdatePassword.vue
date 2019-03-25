<template>
  <div class="register-wrapper">
    <h1>Update Password</h1>
    <br />
    {{message}}
    <br />
    {{errorMessage}}
    <br />
    <div class="submitPasswords">
    <v-form>
      <br/>
        <v-text-field 
          label="Old Password"
          id="oldPassword" 
          type="oldPassword"
          v-model="oldPassword"/>
        <br/>
        <v-text-field 
          label="New Password"
          id="newPassword" 
          type="newPassword" 
          v-model="newPassword" />
        <br/>
        <v-text-field 
          label="Confirm New Password"
          id="confirmNewPassword" 
          type="confirmNewPassword" 
          v-model="confirmNewPassword"/>
        <br />
        <br/>
        <v-btn color="success" class="button-submit-password" type="submit" v-on:click="submitPasswords">Update Password</v-btn>
    
    </v-form>
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
      errorMessage: null,
      oldPassword: null,
      newPassword: null,
      confirmNewPassword: null,
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
      } else if (this.oldPassword === this.newPassword){
        alert('Cannot use the same password to update')
      } else {
        this.message = 'Updating Password'
        axios({
          method: 'POST',
          url: `${apiURL}/users/updatepassword`,
          data: {
            sessionToken: localStorage.token,
            emailAddress: localStorage.email,
            oldPassword: this.$data.oldPassword,
            newPassword: this.$data.confirmNewPassword
          },
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
          .then(response => {this.message = response.data}, this.errorMessage = '')
          .catch(e => { this.errorMessage = e.data })
      }
    }
  }
}
</script>

<style>
/*
#update{
  padding: 70px 0;
  text-align: center;
}

input[type=text] {
  border: 2px solid rgb(123, 171, 226);
  border-radius: 4px;
}
*/
.register-wrapper {
    width: 70%;
    margin: 1px auto;
}
</style>

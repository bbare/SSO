<template>
  <div id="portal">
    <h1>This is the Portal</h1>

    <!-- Feel free to hardcode an app ID here until dashboard logic is complete -->
    <v-btn depressed color="primary" small v-on:click="launch('70579CB0-3D33-47C3-8CAD-650FC4FFBBAC')">Launch App (hardcoded ID)</v-btn>

    <v-alert
      :value="error"
      type="error"
      transition="scale-transition"
    >
      {{error}}
    </v-alert>
  </div>
</template>

<script>
import { signLaunch, submitLaunch } from '@/services/request';

export default {
  data () {
    return {
      error: ""
    }
  },
  methods: {
    launch(appId) {
      this.error = "";

      signLaunch(appId).then(launchData => {
        submitLaunch(launchData).then(launchResponse => {
          window.location.href = launchResponse.redirectURL;
        }).catch(err => {
          let code = err.response.status;

          switch(code) {
            case 500:
              this.error = "An unexpected server error occurred. Please try again momentarily.";
              break;
            default:
              this.error = "An unexpected server error occurred. Please try again momentarily.";
              break;
          }
        })
      }).catch(err => {
        let code = err.response.status;

        switch(code) {
          case 500:
            this.error = "An unexpected server error occurred. Please try again momentarily.";
            break;
          default:
            this.error = "An unexpected server error occurred. Please try again momentarily.";
            break;
        }
      })
    }
  }
}
</script>

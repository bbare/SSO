<template>
  <div id="portal">
    <h1>This is the Dashboard</h1>

    <!-- Feel free to hardcode an app ID here until dashboard logic is complete -->
    <!-- PointMap Registered APP ID -->
    <v-btn depressed color="primary" small v-on:click="launch('DD479768-E765-4403-8A34-4B65AF653E34')">Launch App (PointMap ID)</v-btn>

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

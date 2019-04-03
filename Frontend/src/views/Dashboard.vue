<template>
  <div id="portal">
    <!-- Feel free to hardcode an app ID here until dashboard logic is complete -->
    <!-- <button v-on:click="launch('D146A971-593A-47E4-ABCB-8B3AAF686ECD')">Fake App (hardcoded ID)</button> -->

    <v-card>
      <h1 id="applicationPortal">Application Portal</h1>
      <v-container fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 sm6 md4 lg3 v-for="(app, index) in applications" :key="index">
            <v-card hover>
              <v-icon
                v-if="app.showInfo"
                large
                color="orange"
                style="float: right"
                @click="showInfo = true"
              >info</v-icon>
              <!-- @click="launchLoading = true; launch(app.Id)" -->
              <v-card-title primary-title>
                <img src="https://www.freeiconspng.com/uploads/no-image-icon-15.png">
                <div id="content">
                  <h3 class="headline mb-0">
                    <strong>{{ app.Title }}</strong>
                  </h3>
                </div>
                <read-more
                  more-str="read more"
                  :text="msg"
                  link="#"
                  less-str="read less"
                  :max-chars="100"
                ></read-more>
              </v-card-title>
            </v-card>
            <div v-if="launchLoading">
              <Loading :dialog="launchLoading"/>
            </div>
            <div v-if="showInfo">
              <AppInfo :dialog="showInfo" :appDetails="app" @exitAppInfo="exitAppInfo"/>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card>

    <v-alert :value="error" type="error" transition="scale-transition">{{error}}</v-alert>
  </div>
</template>

<script>
import Vue from "vue";
import Loading from "@/components/Dialogs/Loading.vue";
import AppInfo from "@/components/Dialogs/AppInfo.vue";
import { signLaunch, submitLaunch } from "@/services/request";
import { apiURL } from "@/const.js";
import axios from "axios";
import ReadMore from "vue-read-more";

Vue.use(ReadMore);

export default {
  components: { Loading, AppInfo },
  data() {
    return {
      applications: [],
      showInfo: false,
      launchLoading: false,
      error: "",
      msg:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent pharetra, ipsum sit amet aliquam rhoncus, felis tellus tempus mauris, eget interdum turpis enim vel velit. Nulla facilisi. Nulla hendrerit interdum est vel lacinia. Vivamus accumsan odio ultricies, porttitor magna non, ultrices odio. Cras consequat ipsum consequat, pharetra felis non, imperdiet sem. Vivamus vehicula pulvinar velit, et lobortis felis. In id turpis urna. Mauris dictum laoreet enim, nec sollicitudin magna. Maecenas magna quam, elementum sed volutpat at, sollicitudin in ipsum. Etiam pellentesque sem ligula, a faucibus nisl venenatis eu. Fusce rutrum, diam quis sagittis faucibus, diam orci porta diam, a fringilla odio sapien quis elit. Suspendisse semper vulputate mollis."
    };
  },
  watch: {
    launchLoading(val) {
      if (!val) return;
      setTimeout(() => (this.launchLoading = false), 3000);
    }
  },
  methods: {
    exitAppInfo(hideInfo) {
      this.showInfo = hideInfo;
    },
    launch(appId) {
      this.error = "";

      signLaunch(appId)
        .then(launchData => {
          submitLaunch(launchData)
            .then(launchResponse => {
              window.location.href = launchResponse.redirectURL;
            })
            .catch(err => {
              let code = err.response.status;

              switch (code) {
                case 500:
                  this.error =
                    "An unexpected server error occurred. Please try again momentarily.";
                  break;
                default:
                  this.error =
                    "An unexpected server error occurred. Please try again momentarily.";
                  break;
              }
            });
        })
        .catch(err => {
          let code = err.response.status;

          switch (code) {
            case 500:
              this.error =
                "An unexpected server error occurred. Please try again momentarily.";
              break;
            default:
              this.error =
                "An unexpected server error occurred. Please try again momentarily.";
              break;
          }
        });
    }
  },
  async mounted() {
    await axios
      .get(`${apiURL}/applications`)
      .then(response => (this.applications = response.data));

    // Add attribute for displaying info icon
    // Add attribute for editing app description
    for (var i = 0; i < this.applications.length; i++) {
      this.$set(this.applications[i], "editDescription", false);
    }
  }
};
</script>

<style scoped>
.v-card {
  margin: 1em;
}

#applicationPortal {
  padding: 1em;
  font-size: 38px;
  text-decoration: underline;
}

#content {
  margin-left: 1em;
}

img {
  width: 55px;
  height: 55px;
}
</style>

<template>
  <v-layout row justify-center>
    <v-dialog v-model="dialog" persistent max-width="290">
      <v-card>
        <v-card-title class="headline">{{appDetails.Title}}</v-card-title>
        <v-textarea
          outline
          name="input-7-4"
          label="Description"
          v-model="appDetails.Description"
          v-if="appDetails.editDescription"
        ></v-textarea>
        <v-card-text
          v-if="appDetails.Description === null && appDetails.editDescription === false"
        >No Description...</v-card-text>
        <v-card-text v-else-if="appDetails.editDescription === false">{{appDetails.Description}}</v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="green darken-1"
            flat
            v-if="appDetails.editDescription === false"
            @click="appDetails.editDescription = true"
          >Edit</v-btn>
          <v-btn
            color="green darken-1"
            flat
            v-if="appDetails.editDescription === true"
            @click="appDetails.editDescription = false"
          >Finish Editing</v-btn>
          <v-btn color="green darken-1" flat @click="exitAppInfo">Exit</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script>
export default {
  name: "AppInfo",
  props: ["dialog", "appDetails"],
  methods: {
    exitAppInfo() {
      this.appDetails.editDescription = false;
      this.$emit("exitAppInfo", false);
    }
  }
};
</script>

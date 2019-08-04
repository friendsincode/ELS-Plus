<template>
      <v-container fluid>
    <v-layout>
      <v-flex xs12>
        <v-layout align-end justify-end row fill-height>Whelen</v-layout>
      </v-flex>
    </v-layout>
  </v-container>
</template>


<script lang="ts">
import Vue from "vue";
import Store from "../store";
import Component from "vue-class-component";

// The @Component decorator indicates the class is a Vue component
@Component({
  name: "whelen",
  computed:{}
})
export default class Whelen extends Vue {
    constructor() {
    super();
  }

  

  
}
</script>
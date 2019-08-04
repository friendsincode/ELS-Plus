<template>
  <v-container v-bind:style="{ opacity: $store.state.panel_alpha}" fill-height fluid>
    <v-layout v-bind="$store.state.location" fill-height>
      <v-flex shrink>
        <v-card :color="$store.state.header_color">
          <v-container grid-list-xs>
            <v-layout row justify-space-between>
                <v-flex shrink align-self-center>
                  <v-chip
                    small
                    :color="$store.state.data.stage_1 ? 'blue' : ''"
                    text-color="white"
                  >1</v-chip>
                  <v-chip
                    small
                    :color="$store.state.data.stage_2 ? 'blue' : ''"
                    text-color="white"
                  >2</v-chip>
                  <v-chip
                    small
                    :color="$store.state.data.stage_3 ? 'blue' : ''"
                    text-color="white"
                  >3</v-chip>
                </v-flex>
                <v-flex shrink align-self-center>
                  <v-icon
                    :color="$store.state.extras.ex_1.status ? $store.state.extras.ex_2.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_2.status ? $store.state.extras.ex_2.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_3.status ? $store.state.extras.ex_3.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_4.status ? $store.state.extras.ex_4.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_7.status ? $store.state.extras.ex_7.color : 'grey'"
                  >stop</v-icon>                  
                  <v-icon
                    :color="$store.state.extras.ex_8.status ? $store.state.extras.ex_8.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_9.status ? $store.state.extras.ex_9.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_5.status ? $store.state.extras.ex_5.color : 'grey'"
                  >stop</v-icon>
                  <v-icon
                    :color="$store.state.extras.ex_6.status ? $store.state.extras.ex_6.color : 'grey'"
                  >stop</v-icon>
                </v-flex>
            </v-layout>

            <v-divider dark />
            <v-layout mt-1 row wrap>
              <v-flex xs12>
                <v-layout row>
                  <v-layout>
                    <ElsUiCard
                      title="PRM"
                      :button_color="$store.state.data.prm_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.prm_status"
                      :ui_data="$store.state.data.patterns.prm"
                    />
                    <ElsUiCard
                      title="SEC"
                      :button_color="$store.state.data.sec_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.sec_status"
                      :ui_data="$store.state.data.patterns.sec"
                    />
                    <ElsUiCard
                      title="WRN"
                      :button_color="$store.state.data.wrn_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.wrn_status"
                      :ui_data="$store.state.data.patterns.wrn"
                    />
                    <ElsUiCard
                      class="ml-3"
                      title="TDKN"
                      :button_color="$store.state.extras.ex_11.status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.extras.ex_11.status "
                    />
                    <ElsUiCard
                      title="SCN"
                      :button_color="$store.state.extras.ex_12.status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.extras.ex_12.status"
                    />
                  </v-layout>
                </v-layout>
                <v-layout row>
                  <v-layout justify-content-center align-content-center>
                    <ElsUiCard
                      title="HRN"
                      :button_color="$store.state.data.hrn_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.hrn_status"
                      :ui_data="$store.state.data.horn"
                    />
                    <ElsUiCard
                      title="SRN"
                      :button_color="$store.state.data.sec_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.sec_status"
                      :ui_data="$store.state.data.siren"
                    />
                    <ElsUiCard
                      title="SRM"
                      :button_color="$store.state.data.wrn_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.wrn_status"
                      ui_data="--"
                    />
                    <ElsUiCard
                      class="ml-3"
                      title="CRS"
                      :button_color="$store.state.data.crs_status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.data.crs_status"
                    />
                    <ElsUiCard
                      title="STBN"
                      :button_color="$store.state.extras.ex_10.status ? $store.state.btn_light_color : $store.state.header_color "
                      :button_status="$store.state.extras.ex_10.status"
                    />
                  </v-layout>
                </v-layout>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import Store from "../store";
import Component from "vue-class-component";
import ElsUiCard from "../components/elsuicard.vue";

// The @Component decorator indicates the class is a Vue component
@Component({
  name: "elsplus",
  computed: {},
  components: {
    ElsUiCard
  }
})
export default class ElsPlus extends Vue {
  constructor() {
    super();
  }
}
</script>
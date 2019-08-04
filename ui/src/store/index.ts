import Vue from "vue";
import Vuex from "vuex";
import router from "../router"
import extras from "./extras"
import data from "./data"

Vue.use(Vuex);

const Store = new Vuex.Store({
  state: {
    currentPage: "/elsplus",
    isVisible: true,
    location: {       
      'align-end': true,
      'justify-end': true,
    },
    btn_light_color: "blue",
    header_color: "grey darken-1",
    panel_alpha: .75
  },
  mutations: {
    CHANGE_PAGE: (state: any, page: string) => {
      state.currentPage = page;
      router.push(state.currentPage);
      localStorage.setItem("elsplus_ui_page",page)
    },
    SET_VISIBILITY: (state:any, vis: boolean) => {
      state.isVisible = vis
    },
    SET_LOCATION(state: any, loc: any) {
      state.location = loc
      localStorage.setItem("elsplus_ui_loc", loc)
    },
    SET_HEADER_COLOR(state: any, col: any) {
      state.header_color = col
    },
    SET_BTN_COLOR(state: any, btn: any) {
      state.btn_light_color = btn
    },
    SET_PANEL_ALPHA(state: any, alp: any) {
      state.panel_alpha = alp
      localStorage.setItem("elsplus_ui_alpha", alp)
    }
  },
  actions: {},
  modules: {
   extras,
   data
  }
});

window.addEventListener('message', (event: any) => {

  const item = event.data //'detail' is for debuging via browsers
  //let state:any = Store.state
  console.log(`Mutation: ${item.type} to ${item.data}`)
  Store.commit(`${item.type}`, item.data)
})


export default Store
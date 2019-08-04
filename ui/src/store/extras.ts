export default {
    namespaced: true,
    state: {
        ex_1: { 
            status: false,
            color: "blue"
        },
        ex_2: { 
            status: false,
            color: "blue"
        },
        ex_3: { 
            status: false,
            color: "blue"
        },
        ex_4: { 
            status: false,
            color: "blue"
        },
        ex_5: { 
            status: false,
            color: "blue"
        },
        ex_6: { 
            status: false,
            color: "blue"
        },
        ex_7: { 
            status: false,
            color: "blue"
        },
        ex_8: { 
            status: false,
            color: "blue"
        },
        ex_9: { 
            status: false,
            color: "blue"
        },
        ex_10: { 
            status: false,
            color: "red"
        },
        ex_11: { 
            status: false,
            color: "white"
        },
        ex_12: { 
            status: false,
            color: "white"
        },

    },
    mutations: {
        TOGGLE_1(state: any) {
            state.ex_1.status = !state.ex_1
        },
        TOGGLE_2(state: any) {
            state.ex_2.status = !state.ex_2
        },
        TOGGLE_3(state: any) {
            state.ex_3.status = !state.ex_3
        },
        TOGGLE_4(state: any) {
            state.ex_4.status = !state.ex_4
        },
        TOGGLE_5(state: any) {
            state.ex_5.status = !state.ex_5
        },
        TOGGLE_6(state: any) {
            state.ex_6.status = !state.ex_6
        },
        TOGGLE_7(state: any) {
            state.ex_7.status = !state.ex_7
        },
        TOGGLE_8(state: any) {
            state.ex_8.status = !state.ex_8
        },
        TOGGLE_9(state: any) {
            state.ex_9.status = !state.ex_9
        },
        TOGGLE_10(state: any) {
            state.ex_10.status = !state.ex_10
        },
        TOGGLE_11(state: any) {
            state.ex_11.status = !state.ex_11
        },
        TOGGLE_12(state: any) {
            state.ex_12.status = !state.ex_12
        },
        TOGGLE_CRUISE(state: any) {
            state.cruise = !state.cruise
        }
        
    },
    actions: {}
}
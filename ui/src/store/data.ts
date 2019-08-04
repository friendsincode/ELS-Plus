export default {
    namespaced: true,
    state: {
        patterns: {
            prm: "--",
            sec: "--",
            wrn: "--"
        },
        siren: "WL",
        horn: "--",
        prm_status: false,
        sec_status: false,
        wrn_status: false,
        hrn_status: false,
        srn_status: false,
        srm_status: false,
        stage_1: false,
        stage_2: false,
        stage_3: false,
        crs_status: false,
    },
    mutations: {
        CHANGE_PRM_PATT(state: any, prmPatt: string) {
            state.patterns.prm = prmPatt
        },
        CHANGE_SEC_PATT(state: any, secPatt: string) {
            state.patterns.sec = secPatt
        },
        CHANGE_WRN_PATT(state: any, wrnPatt: string) {
            state.patterns.wrn = wrnPatt
        },
        CHANGE_SIREN_TONE(state:any, srnTone: string) {
            state.siren = srnTone
        },
        CHANGE_HORN(state:any, hrn: string) {
            state.horn = hrn
        },
        CHANGE_PRM_STATUS(state: any, prmStat: boolean) {
            state.prm_status = prmStat
        },
        CHANGE_SEC_STATUS(state: any, secStat: boolean) {
            state.sec_status = secStat
        },
        CHANGE_WRN_STATUS(state: any, wrnStat: boolean) {
            state.wrn_status = wrnStat
        },
        CHANGE_HRN_STATUS(state: any, hrnStat: boolean) {
            state.hrn_status = hrnStat
        },
        CHANGE_SRN_STATUS(state: any, srnStat: boolean) {
            state.srn_status = srnStat
        },
        CHANGE_SRM_STATUS(state: any, srmStat: boolean) {
            state.srm_status = srmStat
        },
        CHANGE_CRS_STATUS(state: any, crsStat: boolean) {
            state.crs_status = crsStat
        },
        CHANGE_STAGE_1(state: any, st1: boolean) {
            state.stage_1 = st1
        },
        CHANGE_STAGE_2(state: any, st2: boolean) {
            state.stage_2 = st2
        },
        CHANGE_STAGE_3(state: any, st3: boolean) {
            state.stage_3 = st3
        }
    }
}
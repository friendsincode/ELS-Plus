

export default [
    {
      path: '/elsplus',
      name: 'elsplus',
      component: () => import(/* webpackChunkName: "elsplus" */ '../views/ElsPlus.vue')
    },
    {
      path: '/whelen',
      name: 'whelen',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "whelen" */ '../views/Whelen.vue')
    },
    {
      path: '/classic',
      name: 'classic',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "classic" */ '../views/Classic.vue')
    }
  ]

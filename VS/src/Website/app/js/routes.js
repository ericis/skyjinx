/*
  Dependencies:
    - angular-route: Downloads the AngularJS ngRoute module file.
*/
define('myapp-routes', ['angular-route'], function () {
    'use strict';

    console.log('define: routes'); // debug define() calls

    // initialization routine for routes
    // $routeProvider is an AngularJS argument
    var initialize = function ($routeProvider) {
        $routeProvider.
            when('/index', {
                templateUrl: 'app/html/index.html',
                //controller: 'MainCtrl'
            }).
            otherwise({
                redirectTo: '/index'
            });
    };

    // AngularJS module dependencies
    var dependencies = ['ngRoute'];

    // AngularJS config array
    var appConfig = ['$routeProvider', initialize];

    var routes = {
        dependencies: dependencies,
        appConfig: appConfig
    };

    return routes;
});
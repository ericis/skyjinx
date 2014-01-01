/*
  Dependencies:
    - angular: Downloads the AngularJS file.
    - routes: This application's routes.
*/
define('myapp', ['angular', 'myapp-routes'], function (angular, routes) {
    'use strict';

    console.log('define: myapp'); // debug define() calls

    // The name of your AngularJS application
    var APP_NAME = 'myapp';

    // Your app's dependencies
    var dependencies = [/*... AngularJS modules, or your custom modules (e.g. services) ...*/];

    // add route dependencies
    angular.forEach(routes.dependencies, function (dep) { dependencies.push(dep); });

    // create the app
    var app = angular.module('myapp', dependencies);

    // config routes
    app.config(routes.appConfig);

    return app;
});
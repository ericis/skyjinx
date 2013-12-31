///#source 1 1 /app/js/controllers.js

///#source 1 1 /app/js/app.js
define('app', ['angular', 'angular-route'], function (angular) {

    var app = angular.module('app', ['ngRoute']);

    return app;
});
///#source 1 1 /app/js/init.js
define('init', ['angular', 'app'], function (angular, app) {
    'use strict';
    
    angular.bootstrap(document, ['app']);

    return app;
});

require(['init']);
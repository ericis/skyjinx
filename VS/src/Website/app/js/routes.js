// <copyright file="routes.js" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

/*
  Dependencies:
    - module: This RequireJS module.
    - angular: Initializes AngularJS.
    - angular-route: Initializes the AngularJS ngRoute module.
*/
define('myapp-routes', ['module', 'angular', 'angular-route'], function (module, angular) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // AngularJS module dependencies
    var dependencies = ['ngRoute'];

    // create the routes module
    var routes = angular.module(module.id, dependencies);

    // configure the routes
    routes.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
            when('/index', {
                templateUrl: 'app/html/index.html'
            }).
            when('/about', {
                templateUrl: 'app/html/about.html'
            }).
            when('/contact', {
                templateUrl: 'app/html/contact.html'
            }).
            otherwise({
                redirectTo: '/index'
            });
    }]);

    return {
        id: module.id,
        module: routes
    };
});
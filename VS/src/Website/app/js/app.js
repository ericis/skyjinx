// <copyright file="app.js" company="Eric Swanson">
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
    - angular: Downloads the AngularJS file.
    - myapp-routes: This application's routes.
    - skyjinx-ng-bs: Downloads the SkyJinx Angular Bootstrap extensions.
*/
define('myapp', ['angular', 'myapp-routes', 'myapp-controllers', 'skyjinx'], function (angular, routes, controllers, skyJinx) {
    'use strict';

    //console.log('define: myapp'); // debug define() calls

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

    // add AngularJS directives for bootstrap
    skyJinx.angular.addBootstrapDirectives(app);

    // add app controllers
    controllers.addControllers(app);

    return app;
});
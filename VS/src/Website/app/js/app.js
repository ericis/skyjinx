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
    - module: This RequireJS module.
    - angular: Initializes AngularJS.
    - skyjinx-bootstrap: Initializes the SkyJinx Angular Bootstrap extensions.
    - myapp-routes: Initializes this application's routes.
    - myapp-controllers: Initializes this application's controllers.
*/
define('myapp', ['module', 'angular', 'skyjinx-bootstrap', 'myapp-routes', 'myapp-controllers', 'myapp-directives'], function (module, angular, bootstrap, routes, controllers, directives) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // Your app's dependencies
    var dependencies = [bootstrap.id, routes.id, controllers.id, directives.id];

    // create the app module
    var app = angular.module(module.id, dependencies);

    return {
        id: module.id,
        module: app
    };
});
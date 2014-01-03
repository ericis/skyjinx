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
    - angular: Initializes AngularJS.
    - skyjinx-bootstrap: Initializes the SkyJinx Angular Bootstrap extensions.
    - myapp-routes: Initializes this application's routes.
    - myapp-controllers: Initializes this application's controllers.
*/
define('myapp', ['angular', 'skyjinx-bootstrap', 'myapp-routes', 'myapp-controllers'], function (angular) {
    'use strict';

    //console.log('define: myapp'); // debug define() calls

    // The name of your AngularJS application
    var APP_NAME = 'myapp';

    // Your app's dependencies
    var dependencies = ['skyjinx-bootstrap', 'myapp-routes', 'myapp-controllers'];

    // create the app module
    var app = angular.module('myapp', dependencies);

    return app;
});
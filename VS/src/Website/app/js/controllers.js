// <copyright file="controllers.js" company="Eric Swanson">
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
*/
define('myapp-controllers', ['module', 'angular'], function (module, angular) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // AngularJS module dependencies
    var dependencies = [];

    // create the controllers module
    var controllers = angular.module(module.id, dependencies);

    // add each controller
    controllers.
        controller('NavCtrl', ['$scope', '$log', function ($scope, $log) {
            $log.info('NavCtrl()');

            $scope.items = [
                { title: 'Home', url: '#', selected: true },
                { title: 'About', url: '#/about', selected: false },
                { title: 'Contact', url: '#/contact', selected: false }
            ];
        }]);

    return {
        id: module.id,
        module: controllers
    };
});
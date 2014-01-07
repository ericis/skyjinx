///#source 1 1 /skyjinx/js/core.js
// <copyright file="core.js" company="Eric Swanson">
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
*/
define('skyjinx', ['module'], function (module) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    return {
        id: module.id,
        module: {}
    };
});
///#source 1 1 /skyjinx/bootstrap/js/bootstrap.js
// <copyright file="bootstrap.js" company="Eric Swanson">
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
define('skyjinx-bootstrap', ['module', 'angular'], function (module, angular) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // AngularJS module dependencies
    var dependencies = [];

    // define the bootstrap module
    var bootstrap = angular.module(module.id, dependencies);

    // define the directives
    bootstrap.
        directive('bsContainer', function () {
            return {
                restrict: 'E',
                transclude: true,
                templateUrl: 'skyjinx/bootstrap/html/container.html'
            };
        }).
        directive('bsNavbar', function () {
            return {
                restrict: 'EA',
                transclude: true,
                scope: {
                    toggleTitle: '=',
                    brandTitle: '=',
                    items: '=',
                    inverted: '@',
                    fixed: '@'
                },
                controller: ['$scope', '$log', function ($scope, $log) {
                    var toggleTitle = angular.isDefined($scope.toggleTitle) ? $scope.toggleTitle : 'Toggle navigation';
                    var brandTitle = angular.isDefined($scope.brandTitle) ? $scope.brandTitle : '[Brand Title]';
                    
                    if (!angular.isDefined($scope.items)) {
                        $scope.items = [];
                    }

                    $scope.flags = {
                        isInverted: angular.isDefined($scope.inverted) ? $scope.$eval($scope.inverted) : false,
                        isFixed: angular.isDefined($scope.fixed) ? $scope.$eval($scope.fixed) : false
                    };

                    $scope.brand = {
                        title: brandTitle
                    };

                    $scope.toggle = {
                        title: toggleTitle
                    };
                }],
                templateUrl: 'skyjinx/bootstrap/html/navbar.html'
            };
        });

    return {
        id: module.id,
        module: bootstrap
    };
});

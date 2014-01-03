///#source 1 1 /app/js/controllers.js
// <copyright file="controllers.js" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

define('myapp-controllers', ['angular'], function (angular) {
    'use strict';

    var addControllers = function (app) {
        app.
            controller('NavCtrl', ['$scope', '$log', function ($scope, $log) {
                $log.info('NavCtrl()');

                $scope.items = [
                    { title: 'Home', url: '#', selected: true },
                    { title: 'About', url: '#/about', selected: false },
                    { title: 'Contact', url: '#/contact', selected: false }
                ];
            }]);
    };

    return {
        addControllers: addControllers
    };
});
///#source 1 1 /app/js/routes.js
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
    - angular-route: Downloads the AngularJS ngRoute module file.
*/
define('myapp-routes', ['angular-route'], function () {
    'use strict';

    //console.log('define: routes'); // debug define() calls

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
///#source 1 1 /app/js/app.js
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
///#source 1 1 /app/js/init.js
// <copyright file="init.js" company="Eric Swanson">
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
    - skyjinx: Downloads the SkyJinx file.
    - app: This JS application
*/
define('init', ['angular', 'myapp'], function (angular, app) {
    'use strict';

    //console.log('define: init'); // debug define() calls
    
    // bootstrap the app to the document
    // *This could be scoped to a specific element.
    angular.bootstrap(document, ['myapp']);

    // return the modified app
    return app;
});

require(['init']);
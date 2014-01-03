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

/*
  Dependencies:
    - angular: Initializes AngularJS.
*/
define('myapp-controllers', ['angular'], function (angular) {
    'use strict';

    var dependencies = [];

    // create the controllers module
    var controllers = angular.module('myapp-controllers', dependencies);

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

    return controllers;
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
    - angular: Initializes AngularJS.
    - angular-route: Initializes the AngularJS ngRoute module.
*/
define('myapp-routes', ['angular','angular-route'], function (angular) {
    'use strict';

    //console.log('define: routes'); // debug define() calls
    
    // AngularJS module dependencies
    var dependencies = ['ngRoute'];

    // create the routes module
    var routes = angular.module('myapp-routes', dependencies);

    // configure the routes
    routes.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
            when('/index', {
                templateUrl: 'app/html/index.html',
                //controller: 'MainCtrl'
            }).
            otherwise({
                redirectTo: '/index'
            });
    }]);

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
    - angular: Initializes AngularJS.
    - myapp: Initializes this application.
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
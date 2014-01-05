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
        controller('NavCtrl', ['$scope', '$log', '$location', function ($scope, $log, $location) {
            $log.info('NavCtrl()');
            $log.info('Current path: ' + $location.path());

            $scope.title = "SkyJinx";

            $scope.items = [
                { title: 'Home', url: '#', selected: $location.path() === '/index' },
                { title: 'About', url: '#/about', selected: $location.path() === '/about' },
                { title: 'Contact', url: '#/contact', selected: $location.path() === '/contact' }
            ];
        }]);

    return {
        id: module.id,
        module: controllers
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
///#source 1 1 /app/js/directives.js
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
*/
define('myapp-directives', ['module', 'angular'], function (module, angular) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // Your app's dependencies
    var dependencies = [];

    // create the app module
    var directives = angular.module(module.id, dependencies);

    // define the directives
    directives.
        directive('mynav', function () {
            return {
                restrict: 'E',
                transclude: false,
                templateUrl: 'app/html/nav.html'
            };
        });

    return {
        id: module.id,
        module: directives
    };
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
    - module: This RequireJS module.
    - angular: Initializes AngularJS.
    - myapp: Initializes this application.
*/
define('init', ['module', 'angular', 'myapp'], function (module, angular, app) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    // AngularJS bootstrap dependencies
    var dependencies = [app.id];

    // bootstrap the app to the document
    // *This could be scoped to a specific element in the page.
    angular.bootstrap(document, dependencies);

    // return the modified app
    return {
        id: module.id,
        module: app
    };
});

// require the initialization module (run it and all of its dependencies)
require(['init']);

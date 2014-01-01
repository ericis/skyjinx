///#source 1 1 /app/js/controllers.js
// place-holder
///#source 1 1 /app/js/routes.js
/*
  Dependencies:
    - angular-route: Downloads the AngularJS ngRoute module file.
*/
define('myapp-routes', ['angular-route'], function () {
    'use strict';

    console.log('define: routes'); // debug define() calls

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
/*
  Dependencies:
    - angular: Downloads the AngularJS file.
    - routes: This application's routes.
*/
define('myapp', ['angular', 'myapp-routes'], function (angular, routes) {
    'use strict';

    console.log('define: myapp'); // debug define() calls

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

    return app;
});
///#source 1 1 /app/js/init.js
/*
  Dependencies:
    - angular: Downloads the AngularJS file.
    - app: This JS application
*/
define('init', ['angular', 'myapp'], function (angular, app) {
    'use strict';

    console.log('define: init'); // debug define() calls
    
    // bootstrap the app to the document
    // *This could be scoped to a specific element.
    angular.bootstrap(document, ['myapp']);

    // return the modified app
    return app;
});

require(['init']);
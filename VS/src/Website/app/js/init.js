define('init', ['angular', 'app'], function (angular, app) {
    'use strict';
    
    angular.bootstrap(document, ['app']);

    return app;
});

require(['init']);
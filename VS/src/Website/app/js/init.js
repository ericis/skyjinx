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
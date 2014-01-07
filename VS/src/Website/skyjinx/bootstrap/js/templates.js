// <copyright file="templates.js" company="Eric Swanson">
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
define('skyjinx-bootstrap-templates', ['module'], function (module) {
    'use strict';

    //console.log('define: ' + module.id); // debug define() calls

    var templates = {
        container: "<div class=\"container\" ng-transclude=\"\"></div>",
        nav: "<ul class=\"nav navbar-nav\" ng-class=\"{'navbar-left':flags.isAlignedLeft, 'navbar-right':flags.isAlignedRight}\" ng-transclude=\"\"></ul>",
        navbar: "<div class=\"navbar\" ng-class=\"{'navbar-default':!flags.isInverted, 'navbar-inverse':flags.isInverted, 'navbar-fixed-top':flags.isFixed}\" role=\"navigation\"><div class=\"container\"><div class=\"navbar-header\"><button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\".navbar-collapse\"> <span class=\"sr-only\">{{toggle.title}}</span> <span class=\"icon-bar\"></span> <span class=\"icon-bar\"></span> <span class=\"icon-bar\"></span> </button> <a class=\"navbar-brand\" href=\"#\">{{brand.title}}</a></div><div class=\"collapse navbar-collapse\" ng-transclude=\"\"></div></div></div>",
    };

    return {
        id: module.id,
        module: templates
    };
});
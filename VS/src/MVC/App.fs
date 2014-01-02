// <copyright file="App.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace SkyJinx.Web.Mvc

open System
open System.Web
open System.Web.Mvc
open System.Web.Routing

/// Contract for application session events
type IAppSessionEvents = 
    /// Called when a session starts
    abstract OnSessionStart : unit -> IAppSessionEvents
    /// Called when a session ends
    abstract OnSessionEnd : unit -> IAppSessionEvents
    
/// Contract for application request events
type IAppRequestEvents = 
    /// Called when a request starts
    abstract OnBeginRequest : unit -> IAppRequestEvents
    /// Called when authentication is requested
    abstract OnAuthenticateRequest : unit -> IAppRequestEvents
    
/// Contract for application events
type IAppEvents = 
    /// Called when the application starts
    abstract OnStart : unit -> IAppEvents
    /// Called when the application encounters an error
    abstract OnError : unit -> IAppEvents
    /// Called when the application ends
    abstract OnEnd : unit -> IAppEvents
    
/// Contract for an MVC application
type IApp = 
    inherit IAppSessionEvents
    inherit IAppRequestEvents
    inherit IAppEvents

/// Contract for initializing MVC Areas
type IAreaInitialization =
    /// Register MVC Areas
    abstract RegisterAreas : unit -> IAreaInitialization

/// Default implementation for initializing MVC Areas
type DefaultAreaInitialization() =
    /// Contract for initializing MVC Areas
    abstract RegisterAreas : unit -> IAreaInitialization
    default this.RegisterAreas() =
        AreaRegistration.RegisterAllAreas()
        this :> IAreaInitialization
        
    // contract implementation
    interface IAreaInitialization with
        member this.RegisterAreas() = this.RegisterAreas()
        
/// Contract for initializing MVC Filters
type IFilterInitialization =
    /// Register MVC Filters
    abstract RegisterFilters : unit -> IFilterInitialization
    
/// Default implementation for initializing MVC Filters
type DefaultFilterInitialization() =
    /// Register MVC Filters
    abstract RegisterFilters : unit -> IFilterInitialization
    default this.RegisterFilters() =
        GlobalFilters.Filters.Add(HandleErrorAttribute())
        this :> IFilterInitialization
        
    // contract implementation
    interface IFilterInitialization with
        member this.RegisterFilters() = this.RegisterFilters()
        
/// Contract for initializing MVC Routes
type IRouteInitialization =
    /// Register MVC Routes
    abstract RegisterRoutes : unit -> IRouteInitialization
    
/// Default record type for a Controller Route
type ControllerRouteDefaults =
    {
        /// The name of the default controller (without the "Controller" name suffix)
        controller: string
        /// The name of the default controller action
        action: string
    }

/// A single-page application controller
type SpaController() =
    inherit Controller()
    
    /// The index action for the SPA
    member this.Index() =
        this.View("~/SPA.cshtml")
        
/// Default implementation for initializing MVC Routes
type DefaultRouteInitialization() =
    /// Register MVC Routes
    abstract RegisterRoutes : unit -> IRouteInitialization
    default this.RegisterRoutes() =
        let ns = this.GetType().Namespace

        // ignore .axd requests
        RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        // map the default SPA route
        ignore <| RouteTable.Routes.MapRoute("Default", "{controller}/{action}", box { controller = "Spa"; action = "Index" }, [|ns|])

        this :> IRouteInitialization
        
    // contract implementation
    interface IRouteInitialization with
        member this.RegisterRoutes() = this.RegisterRoutes()

/// Contract for application startup
type IAppStartup =
    /// Starts the application
    abstract Start : IApp -> IApp
    
/// Default implementation for application startup
type DefaultAppStartup(logInit:Log.ILogInitializer, areasInit:IAreaInitialization, filtersInit:IFilterInitialization, routesInit:IRouteInitialization) =
    /// Starts the application
    abstract Start : IApp -> IApp
    default this.Start(app) =
        // init logging
        let log = logInit.Initialize()

        // log the app startup event
        log.Startup()

        // init areas
        ignore <| areasInit.RegisterAreas()
        
        // init filters
        ignore <| filtersInit.RegisterFilters()
        
        // init routes
        ignore <| routesInit.RegisterRoutes()

        app
        
    // contract implementation
    interface IAppStartup with
        member this.Start(app) = this.Start(app)
        
/// Contract for application shutdown
type IAppShutdown =
    /// Shuts down the application
    abstract Shutdown : IApp -> IApp
    
/// Default implementation for application startup
type DefaultAppShutdown(logDispose:Log.ILogDisposer, log:Log.ILog) =
    /// Shuts down the application
    abstract Shutdown : IApp -> IApp
    default this.Shutdown(app) =
        // log the app shutdown event
        log.Shutdown()

        // clean up logging
        logDispose.Dispose(log)
        
        app
        
    // contract implementation
    interface IAppShutdown with
        member this.Shutdown(app) = this.Shutdown(app)

/// SkyJinx Web Application
type App(startup:IAppStartup, shutdown:IAppShutdown) = 
    inherit HttpApplication()
    
    /// Default constructor builds default start-up and shut-down implementation
    new() = 
        let log = Log.GetLog()
        let logInit = Log.DefaultLogInitializer(log)
        let logDispose = Log.DefaultLogDisposer()
        let areaInit = DefaultAreaInitialization()
        let filterInit = DefaultFilterInitialization()
        let routeInit = DefaultRouteInitialization()
        // TODO: BundleTable.Bundles
        let startup = DefaultAppStartup(logInit, areaInit, filterInit, routeInit)
        let shutdown = DefaultAppShutdown(logDispose, log)
        new App(startup, shutdown)
        
    /// Called when the application starts
    abstract OnStart : unit -> IApp
    default this.OnStart() = 
        startup.Start(this)
    /// ASP.NET default application start method for an HttpApplication
    member this.Application_Start(sender:obj, e:EventArgs) =
        ignore <| this.OnStart()
    
    /// Called when a session starts
    abstract OnSessionStart : unit -> IApp
    default this.OnSessionStart() = this :> IApp
    /// ASP.NET default session start method for an HttpApplication
    member this.Session_Start(sender:obj, e:EventArgs) = 
        ignore <| this.OnSessionStart()
    
    /// Called when a request begins
    abstract OnBeginRequest : unit -> IApp
    default this.OnBeginRequest() = this :> IApp
    /// ASP.NET default begin request method for an HttpApplication
    member this.Application_BeginRequest(sender:obj, e:EventArgs) =
        ignore <| this.OnBeginRequest()
    
    /// Called when authentication is requested
    abstract OnAuthenticateRequest : unit -> IApp
    default this.OnAuthenticateRequest() = this :> IApp
    /// ASP.NET default authenticate request method for an HttpApplication
    member this.Application_AuthenticateRequest(sender:obj, e:EventArgs) =
        ignore <| this.OnAuthenticateRequest()
    
    /// Called when an application error occurs
    abstract OnError : unit -> IApp
    default this.OnError() = this :> IApp
    /// ASP.NET default application error method for an HttpApplication
    member this.Application_Error(sender:obj, e:EventArgs) =
        ignore <| this.OnError()
    
    /// Called when a session ends
    abstract OnSessionEnd : unit -> IApp
    default this.OnSessionEnd() = this :> IApp
    /// ASP.NET default session end method for an HttpApplication
    member this.Session_End(sender:obj, e:EventArgs) = 
        ignore <| this.OnSessionEnd()
    
    /// Called when the application ends
    abstract OnEnd : unit -> IApp
    default this.OnEnd() = 
        shutdown.Shutdown(this)
    /// ASP.NET default application end method for an HttpApplication
    member this.Application_End(sender:obj, e:EventArgs) = 
        ignore <| this.OnEnd()
    
    // contract implementation
    interface IApp with
        member this.OnStart() = this.OnStart() :> IAppEvents
        member this.OnSessionStart() = this.OnSessionStart() :> IAppSessionEvents
        member this.OnBeginRequest() = this.OnBeginRequest() :> IAppRequestEvents
        member this.OnAuthenticateRequest() = this.OnAuthenticateRequest() :> IAppRequestEvents
        member this.OnError() = this.OnError() :> IAppEvents
        member this.OnSessionEnd() = this.OnSessionEnd() :> IAppSessionEvents
        member this.OnEnd() = this.OnEnd() :> IAppEvents

open System.Runtime.CompilerServices

[<Extension>]
module UrlExtensions =
    /// Returns a URL string specific to a path used by RequireJS
    [<Extension>]
    let Require(url:UrlHelper, vpath:string) =
        match url with
        | null -> MvcHtmlString.Empty
        | _ ->
            // get the regular URL
            let path = url.Content(vpath)
            // remove .js
            let requirejsPath = path.Replace(".js", "")
            // return
            MvcHtmlString.Create(requirejsPath)
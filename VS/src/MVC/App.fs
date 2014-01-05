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

/// Contract for application startup
type IAppStartup =
    /// Starts the application
    abstract Start : app:IApp -> IApp
    
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
    
    /// Default constructor builds default start-up and shut-down implementation with specified configuration root path
    new(configurationRootPath:string) = 
        let log = Log.GetLog()
        let logInit = Log.DefaultLogInitializer(log)
        let logDispose = Log.DefaultLogDisposer()
        let areaInit = DefaultAreaInitialization()
        let filterInit = DefaultFilterInitialization()
        let routeInit = DefaultRouteInitialization(log, configurationRootPath)
        // TODO: BundleTable.Bundles
        let startup = DefaultAppStartup(logInit, areaInit, filterInit, routeInit)
        let shutdown = DefaultAppShutdown(logDispose, log)
        new App(startup, shutdown)
    
    /// Default constructor builds default start-up and shut-down implementation
    new() = new App("skyjinx")

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
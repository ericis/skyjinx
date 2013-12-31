namespace SkyJinx.Web.Mvc

open System
open System.Web
open System.Web.Mvc
open System.Web.Routing

type IAppSessionEvents = 
    abstract OnSessionStart : unit -> IAppSessionEvents
    abstract OnSessionEnd : unit -> IAppSessionEvents

type IAppRequestEvents = 
    abstract OnBeginRequest : unit -> IAppRequestEvents
    abstract OnAuthenticateRequest : unit -> IAppRequestEvents

type IAppEvents = 
    abstract OnStart : unit -> IAppEvents
    abstract OnError : unit -> IAppEvents
    abstract OnEnd : unit -> IAppEvents

type IApp = 
    inherit IAppSessionEvents
    inherit IAppRequestEvents
    inherit IAppEvents

type IAreaInitialization =
    abstract RegisterAreas : unit -> IAreaInitialization

type DefaultAreaInitialization() =
    abstract RegisterAreas : unit -> IAreaInitialization
    default this.RegisterAreas() =
        AreaRegistration.RegisterAllAreas()
        this :> IAreaInitialization

    interface IAreaInitialization with
        member this.RegisterAreas() = this.RegisterAreas()

type IFilterInitialization =
    abstract RegisterFilters : unit -> IFilterInitialization

type DefaultFilterInitialization() =
    abstract RegisterFilters : unit -> IFilterInitialization
    default this.RegisterFilters() =
        GlobalFilters.Filters.Add(HandleErrorAttribute())
        this :> IFilterInitialization

    interface IFilterInitialization with
        member this.RegisterFilters() = this.RegisterFilters()

type IRouteInitialization =
    abstract RegisterRoutes : unit -> IRouteInitialization
    
type ControllerRouteDefaults =
    {
        controller: string
        action: string
        id: UrlParameter
    }

type SpaController() =
    inherit Controller()
    
    member this.Index() =
        this.View("~/SPA.cshtml")

type DefaultRouteInitialization() =
    abstract RegisterRoutes : unit -> IRouteInitialization
    default this.RegisterRoutes() =
        let ns = this.GetType().Namespace
        RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        ignore <| RouteTable.Routes.MapRoute("Default", "{controller}/{action}", box { controller = "Spa"; action = "Index"; id = UrlParameter.Optional }, [|ns|])
        this :> IRouteInitialization

    interface IRouteInitialization with
        member this.RegisterRoutes() = this.RegisterRoutes()

type IAppStartup =
    abstract Start : IApp -> IApp

type DefaultAppStartup(logInit:Log.ILogInitializer, areasInit:IAreaInitialization, filtersInit:IFilterInitialization, routesInit:IRouteInitialization) =
    abstract Start : IApp -> IApp
    default this.Start(app) =
        let log = logInit.Initialize()
        log.Startup()
        ignore <| areasInit.RegisterAreas()
        ignore <| filtersInit.RegisterFilters()
        ignore <| routesInit.RegisterRoutes()
        app

    interface IAppStartup with
        member this.Start(app) = this.Start(app)

type IAppShutdown =
    abstract Shutdown : IApp -> IApp

type DefaultAppShutdown(logDispose:Log.ILogDisposer, log:Log.ILog) =
    abstract Shutdown : IApp -> IApp
    default this.Shutdown(app) =
        logDispose.Dispose(log)
        log.Shutdown()
        app

    interface IAppShutdown with
        member this.Shutdown(app) = this.Shutdown(app)

type App(startup:IAppStartup, shutdown:IAppShutdown) = 
    inherit HttpApplication()
    
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

    abstract OnStart : unit -> IApp
    default this.OnStart() = 
        startup.Start(this)
    member this.Application_Start(sender:obj, e:EventArgs) =
        ignore <| this.OnStart()
    
    abstract OnSessionStart : unit -> IApp
    default this.OnSessionStart() = this :> IApp
    member this.Session_Start(sender:obj, e:EventArgs) = 
        ignore <| this.OnSessionStart()
    
    abstract OnBeginRequest : unit -> IApp
    default this.OnBeginRequest() = this :> IApp
    member this.Application_BeginRequest(sender:obj, e:EventArgs) =
        ignore <| this.OnBeginRequest()
    
    abstract OnAuthenticateRequest : unit -> IApp
    default this.OnAuthenticateRequest() = this :> IApp
    member this.Application_AuthenticateRequest(sender:obj, e:EventArgs) =
        ignore <| this.OnAuthenticateRequest()
    
    abstract OnError : unit -> IApp
    default this.OnError() = this :> IApp
    member this.Application_Error(sender:obj, e:EventArgs) =
        ignore <| this.OnError()
    
    abstract OnSessionEnd : unit -> IApp
    default this.OnSessionEnd() = this :> IApp
    member this.Session_End(sender:obj, e:EventArgs) = 
        ignore <| this.OnSessionEnd()
    
    abstract OnEnd : unit -> IApp
    default this.OnEnd() = 
        shutdown.Shutdown(this)
    member this.Application_End(sender:obj, e:EventArgs) = 
        ignore <| this.OnEnd()
    
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
    [<Extension>]
    let Require(url:UrlHelper, vpath:string) =
        match url with
        | null -> MvcHtmlString.Empty
        | _ ->
            let path = url.Content(vpath)
            let requirejsPath = path.Replace(".js", "")
            MvcHtmlString.Create(requirejsPath)
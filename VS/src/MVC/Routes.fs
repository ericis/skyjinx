// <copyright file="Routes.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace SkyJinx.Web.Mvc

open System.Linq
open System.Web.Mvc
open System.Web.Routing

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

/// Default implementation for initializing MVC Routes
type DefaultRouteInitialization(log:Log.ILog, xPathConfigurationRoot:string) =
    /// Register MVC Routes
    abstract RegisterRoutes : unit -> IRouteInitialization
    default this.RegisterRoutes() =

        let registerDefaultRoutes () =
            let ns = this.GetType().Namespace
            
            // ignore .axd requests
            let ignorePath = "{resource}.axd/{*pathInfo}"
            log.RegisterRoute("ignore", ignorePath)
            RouteTable.Routes.IgnoreRoute(ignorePath)

            // map the default route
            let defaultRouteName = "Default"
            let defaultRoutePath = "Default"
            log.RegisterRoute(defaultRouteName, defaultRoutePath)
            ignore <| RouteTable.Routes.MapRoute(defaultRouteName, defaultRoutePath, box { controller = "Home"; action = "Index" }, [|ns|])
        
        let config = SkyJinx.Web.Mvc.Configuration.NavigationSection.GetSection(xPathConfigurationRoot)

        match config with
        | null -> registerDefaultRoutes()
        | _ -> 
            config.Routes.Items.Cast<SkyJinx.Web.Mvc.Configuration.NavigationRouteElementBase>()
            |> Seq.iter (fun routeConfig ->
                match routeConfig with
                | :? SkyJinx.Web.Mvc.Configuration.NavigationIgnoreRouteElement as ignoreRoute ->
                    log.RegisterRoute("ignore", ignoreRoute.Url)
                    RouteTable.Routes.IgnoreRoute(ignoreRoute.Url)
                | :? SkyJinx.Web.Mvc.Configuration.NavigationControllerRouteElement as controllerRoute ->
                    
                    if not (controllerRoute.Defaults.AllKeys.Contains("controller")) then
                        raise (System.Configuration.ConfigurationErrorsException("MVC controller route is missing a default value for 'controller' (default controller type name without the 'Controller' suffix)."))

                    if not (controllerRoute.Defaults.AllKeys.Contains("action")) then
                        raise (System.Configuration.ConfigurationErrorsException("MVC controller route is missing a default value for 'action' (default controller action)."))

                    let controllerDefault = controllerRoute.Defaults.["controller"].Value
                    let actionDefault = controllerRoute.Defaults.["action"].Value
                    
                    let defaults = 
                        box
                            {
                                controller = controllerDefault
                                action = actionDefault
                            }

                    let namespaces = 
                        controllerRoute.Namespaces.Cast<System.Web.Configuration.NamespaceInfo>()
                        |> Seq.map (fun namespaceInfo -> namespaceInfo.Namespace)
                        |> Seq.toArray

                    log.RegisterRoute(controllerRoute.Name, controllerRoute.Url)
                    ignore <| RouteTable.Routes.MapRoute(controllerRoute.Name, controllerRoute.Url, defaults, namespaces)
                | _ -> 
                    raise (System.Configuration.ConfigurationErrorsException(sprintf "Unrecognized or unsupported route type %s" (routeConfig.GetType().FullName))))
        
        this :> IRouteInitialization
        
    // contract implementation
    interface IRouteInitialization with
        member this.RegisterRoutes() = this.RegisterRoutes()
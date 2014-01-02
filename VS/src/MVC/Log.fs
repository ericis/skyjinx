// <copyright file="Log.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

module Log

open System
open System.Diagnostics
open System.Diagnostics.Tracing
open Microsoft.Practices.EnterpriseLibrary.SemanticLogging

/// Contract for MVC Logging
type ILog =
    /// Logs the application startup event
    abstract Startup : unit -> unit
    /// Logs when areas are registered
    abstract RegisterAreas : unit -> unit
    /// Logs when a filter is registered
    abstract RegisterFilter : string -> unit
    /// Logs when a route is registered
    abstract RegisterRoute : string * string -> unit
    /// Logs the application shutdown event
    abstract Shutdown : unit -> unit

/// Contract for initializing logging
type ILogInitializer =
    /// Initializes the logging implementation
    abstract Initialize : unit -> ILog

/// Contract for disposing of logging
type ILogDisposer =
    /// Disposes of the logging implementation
    abstract Dispose : ILog -> unit

/// The default MVC logging event source.
[<EventSource(Name = "SkyJinx-Web-MVC")>]
type MvcEventSource private() =
    inherit EventSource()

    static let log = new Lazy<_>(fun _ -> new MvcEventSource() :> ILog)

    /// Gets the static instance of the logging implementation
    static member Instance with get() = log.Value

    /// Logs the application startup event
    [<Event(1, Message = "Starting up.", Level = EventLevel.Informational)>]
    member this.Startup() =
        Debug.WriteLine("Starting up.")
        if (this.IsEnabled()) then
            this.WriteEvent(1)
            
    /// Logs when areas are registered
    [<Event(50, Message = "Registering MVC Areas.", Level = EventLevel.Informational)>]
    member this.RegisterAreas() =
        Debug.WriteLine("Registering MVC Areas.")
        if (this.IsEnabled()) then
            this.WriteEvent(50)
            
    /// Logs when a filter is registered
    [<Event(60, Message = "Registered MVC Filter: {0}", Level = EventLevel.Informational)>]
    member this.RegisterFilter(name:string) =
        Debug.WriteLine(sprintf "Registered MVC Filter: %s" name)
        if (this.IsEnabled()) then
            this.WriteEvent(60, name)
            
    /// Logs when a route is registered
    [<Event(70, Message = "Registered MVC Route \"{0}\": {1}", Level = EventLevel.Informational)>]
    member this.RegisterRoute(name:string, path:string) =
        Debug.WriteLine(sprintf "Registered MVC Route \"%s\": %s" name path)
        if (this.IsEnabled()) then
            this.WriteEvent(70, name, path)
            
    /// Logs the application shutdown event
    [<Event(9999, Message = "Shutting down.", Level = EventLevel.Informational)>]
    member this.Shutdown() =
        Debug.WriteLine("Shutting down.")
        if (this.IsEnabled()) then
            this.WriteEvent(9999)

    // contract implementation
    interface ILog with
        member this.Startup() = this.Startup()
        member this.RegisterAreas() = this.RegisterAreas()
        member this.RegisterFilter(name) = this.RegisterFilter(name)
        member this.RegisterRoute(name, path) = this.RegisterRoute(name, path)
        member this.Shutdown() = this.Shutdown()

/// Provides the default implementation of MVC logging
let GetLog() = MvcEventSource.Instance

/// The default log initializer
type DefaultLogInitializer(log:ILog) =
    let listener = new ObservableEventListener()
    
    /// Initializes the logging implementation
    member this.Initialize() =
        
        // if the logging implementation is an 'EventSource', setup
        match log with
        | :? EventSource as src -> 
            listener.EnableEvents(src, EventLevel.LogAlways, Keywords.All)
            ignore <| listener.LogToConsole()
        | _ -> ()

        log
        
    // contract implementation
    interface ILogInitializer with
        member this.Initialize() = this.Initialize()
        
/// The default log disposer
type DefaultLogDisposer() =
    let listener = new ObservableEventListener()
    
    /// Disposes of the logging implementation
    member this.Dispose(log:ILog) =
        
        // if the logging implementation is an 'EventSource', cleanup
        match log with
        | :? EventSource as src -> 
            listener.DisableEvents(src)
            listener.Dispose()
        | _ -> ()

        ()
        
    // contract implementation
    interface ILogDisposer with
        member this.Dispose(log) = this.Dispose(log)
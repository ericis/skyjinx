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

type ILog =
    abstract Startup : unit -> unit
    abstract RegisterAreas : unit -> unit
    abstract RegisterFilter : string -> unit
    abstract RegisterRoute : string * string -> unit
    abstract Shutdown : unit -> unit

type ILogInitializer =
    abstract Initialize : unit -> ILog

type ILogDisposer =
    abstract Dispose : ILog -> unit

[<EventSource(Name = "SkyJinx-Web-MVC")>]
type MvcEventSource private() =
    inherit EventSource()

    static let log = new Lazy<_>(fun _ -> new MvcEventSource() :> ILog)

    static member Instance with get() = log.Value

    [<Event(1, Message = "Starting up.", Level = EventLevel.Informational)>]
    member this.Startup() =
        Debug.WriteLine("Starting up.")
        if (this.IsEnabled()) then
            this.WriteEvent(1)

    [<Event(50, Message = "Registering MVC Areas.", Level = EventLevel.Informational)>]
    member this.RegisterAreas() =
        Debug.WriteLine("Registering MVC Areas.")
        if (this.IsEnabled()) then
            this.WriteEvent(50)

    [<Event(60, Message = "Registered MVC Filter: {0}", Level = EventLevel.Informational)>]
    member this.RegisterFilter(name:string) =
        Debug.WriteLine(sprintf "Registered MVC Filter: %s" name)
        if (this.IsEnabled()) then
            this.WriteEvent(60, name)

    [<Event(70, Message = "Registered MVC Route \"{0}\": {1}", Level = EventLevel.Informational)>]
    member this.RegisterRoute(name:string, path:string) =
        Debug.WriteLine(sprintf "Registered MVC Route \"%s\": %s" name path)
        if (this.IsEnabled()) then
            this.WriteEvent(70, name, path)

    [<Event(9999, Message = "Shutting down.", Level = EventLevel.Informational)>]
    member this.Shutdown() =
        Debug.WriteLine("Shutting down.")
        if (this.IsEnabled()) then
            this.WriteEvent(9999)

    interface ILog with
        member this.Startup() = this.Startup()
        member this.RegisterAreas() = this.RegisterAreas()
        member this.RegisterFilter(name) = this.RegisterFilter(name)
        member this.RegisterRoute(name, path) = this.RegisterRoute(name, path)
        member this.Shutdown() = this.Shutdown()

let GetLog() = MvcEventSource.Instance

type DefaultLogInitializer(log:ILog) =
    let listener = new ObservableEventListener()
    
    member this.Initialize() =
        
        match log with
        | :? EventSource as src -> 
            listener.EnableEvents(src, EventLevel.LogAlways, Keywords.All)
            ignore <| listener.LogToConsole()
        | _ -> ()

        log

    interface ILogInitializer with
        member this.Initialize() = this.Initialize()

type DefaultLogDisposer() =
    let listener = new ObservableEventListener()
    
    member this.Dispose(log:ILog) =
        
        match log with
        | :? EventSource as src -> 
            listener.DisableEvents(src)
            listener.Dispose()
        | _ -> ()

        ()

    interface ILogDisposer with
        member this.Dispose(log) = this.Dispose(log)
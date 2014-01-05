// <copyright file="AppEvents.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace SkyJinx.Web.Mvc

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
// <copyright file="Filters.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace SkyJinx.Web.Mvc

open System.Web.Mvc
        
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
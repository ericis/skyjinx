// <copyright file="HtmlExtensions.fs" company="Eric Swanson">
// Copyright (C) 2014 Eric Swanson
//      
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace SkyJinx.Web.Mvc

open System.Runtime.CompilerServices

[<Extension>]
module HtmlExtensions =
    
    open System
    open System.Configuration
    open System.Linq
    open System.Text
    open System.Web.Mvc

    open SkyJinx.Web.Mvc.Configuration
    
    let private getRequireUrl (path:string) (url:UrlHelper) =
        UrlExtensions.Require(url, path)

    let private getUrl (path:string) (url:UrlHelper) =
        match path.StartsWith("~/") with
        | true -> url.Content(path)
        | false -> path
        
    let private getScript (html:HtmlHelper) (rootConfigElementName:string) =
    
        // TODO: clean-up this brute-force giant function

        let contentConfig = ContentSection.GetSection(rootConfigElementName)
        
        if contentConfig = null then
            raise (ConfigurationErrorsException("Missing <content> configuration section"))
        
        let requireConfig = RequireJsSection.GetSection(rootConfigElementName)

        if requireConfig = null then
            raise (ConfigurationErrorsException("Missing <requirejs> configuration section"))
        
        let scriptConfigs = contentConfig.Scripts.Scripts.Cast<ContentScriptElement>()

        let findScriptConfig name = 
            scriptConfigs |> Seq.tryFind (fun item -> item.Name = name)

        let requireJsConfig = findScriptConfig requireConfig.ScriptName

        let mainJsConfig = findScriptConfig requireConfig.MainScriptName

        if requireJsConfig.IsNone then
            raise (ConfigurationErrorsException("The scriptName from the RequireJS section (<requirejs scriptName=\"...\">) does not match a script name in the content scripts section (<content><scripts><script name=\"...\" ... </scripts></content>) in configuration"))

        if mainJsConfig.IsNone then
            raise (ConfigurationErrorsException("The mainScriptName from the RequireJS section (<requirejs mainScriptName=\"...\">) does not match a script name in the content scripts section (<content><scripts><script name=\"...\" ... </scripts></content>) in configuration"))
        
        let requireJsUrl = requireJsConfig.Value.Url
        let mainJsUrl = mainJsConfig.Value.Url
        
        if String.IsNullOrWhiteSpace(requireJsUrl) then
            raise (ConfigurationErrorsException(sprintf "The url for the script %s is required and must not be empty in configuration" requireConfig.ScriptName))
        
        if String.IsNullOrWhiteSpace(mainJsUrl) then
            raise (ConfigurationErrorsException(sprintf "The url for the script %s is required and must not be empty in configuration" requireConfig.MainScriptName))

        let url = new UrlHelper(html.ViewContext.RequestContext)

        sprintf "<script data-main=\"%s\" src=\"%s\"></script>" (getRequireUrl mainJsUrl url) (getUrl requireJsUrl url)

    let getConfig (html:HtmlHelper) (rootConfigElementName:string) =
        
        // TODO: clean-up this brute-force giant function

        let contentConfig = ContentSection.GetSection(rootConfigElementName)
        
        if contentConfig = null then
            raise (ConfigurationErrorsException("Missing <content> configuration section"))
        
        let requireConfig = RequireJsSection.GetSection(rootConfigElementName)

        if requireConfig = null then
            raise (ConfigurationErrorsException("Missing <requirejs> configuration section"))
        
        let scriptConfigs = contentConfig.Scripts.Scripts.Cast<ContentScriptElement>()

        let findScriptConfig name = 
            scriptConfigs |> Seq.tryFind (fun item -> item.Name = name)

        let url = new UrlHelper(html.ViewContext.RequestContext)

        let requireJsScriptConfigs = 
            requireConfig.Scripts.Scripts.Cast<RequireJsScriptElement>()

        let requireJsScripts = 
            requireJsScriptConfigs
            |> Seq.map (fun requireJsScriptConfig -> 
                
                let scriptConfig = findScriptConfig requireJsScriptConfig.Name
                
                if scriptConfig.IsNone then
                    raise (ConfigurationErrorsException(sprintf "The RequireJS script name %s from the RequireJS section (<requirejs>) does not match a script name in the content scripts section (<content><scripts><script name=\"...\" ... </scripts></content>) in configuration" requireJsScriptConfig.Name))

                match String.IsNullOrWhiteSpace(scriptConfig.Value.Url) with
                | true -> 
                    let pathConfigs = scriptConfig.Value.Paths.Cast<ContentScriptPathElement>()

                    let scriptPaths =
                        pathConfigs 
                        |> Seq.map (fun pathConfig -> 
                            sprintf "'%s'" (getRequireUrl pathConfig.Url url))
                        |> Seq.toArray

                    let paths = String.Join(",", scriptPaths)

                    sprintf "'%s': [%s]" requireJsScriptConfig.Name paths
                | false -> 
                    sprintf "'%s': '%s'" requireJsScriptConfig.Name (getRequireUrl scriptConfig.Value.Url url))
            |> Seq.toArray

        let paths = String.Join(",", requireJsScripts)
        
        let shims = 
            requireJsScriptConfigs
            |> Seq.map (fun requireJsScriptConfig -> 
                let shim = new StringBuilder(sprintf "'%s':{" requireJsScriptConfig.Name)
                let hasValues = ref false

                if not (String.IsNullOrWhiteSpace(requireJsScriptConfig.Exports)) then
                    ignore <| shim.Append(sprintf "exports:'%s'" requireJsScriptConfig.Exports)
                    hasValues := true

                if requireJsScriptConfig.Dependencies.Scripts.Count > 0 then
                    if hasValues.Value then
                        ignore <| shim.Append(",")

                    let deps = 
                        requireJsScriptConfig.Dependencies.Scripts.Cast<RequireJsScriptDependencyElement>()
                        |> Seq.map (fun dependency ->
                            sprintf "'%s'" dependency.Name)
                        |> Seq.toArray
                    
                    ignore <| shim.Append(sprintf "deps:[%s]" (String.Join(",", deps)))
                    hasValues := true

                shim.Append("}").ToString())
            |> Seq.toArray

        let shim = String.Join(",", shims)

        sprintf "<script>var require={paths:{%s},shim:{%s}};</script>" paths shim
        
    let getStyleSheet (html:HtmlHelper) (name:string) (contentConfig:ContentSection) =
    
        let styleConfig = 
            contentConfig.Styles.Styles.Cast<ContentStyleElement>()
            |> Seq.tryFind(fun style -> style.Name = name)
        
        match styleConfig with
        | Some(cfg) -> 
            
            let urlHelper = new UrlHelper(html.ViewContext.RequestContext)

            let url = getUrl cfg.Url urlHelper

            sprintf "<link rel=\"stylesheet\" href=\"%s\" />" url

        | None -> raise (ConfigurationErrorsException("The style named %s does not match a style name in the content styles section (<content><styles><style name=\"...\" ... </styles></content>) in configuration"))

    [<Extension>]
    let StyleSheet(html:HtmlHelper, rootConfigElementName:string, name:string) =
        
        let contentConfig = ContentSection.GetSection(rootConfigElementName)

        if contentConfig = null then
            raise (ConfigurationErrorsException("Missing <content> configuration section"))
        
        let style = getStyleSheet html name contentConfig

        MvcHtmlString.Create(style)
        
    [<Extension>]
    let StyleSheets(html:HtmlHelper, rootConfigElementName:string, [<ParamArrayAttribute>]names:string[]) =
        
        let contentConfig = ContentSection.GetSection(rootConfigElementName)

        if contentConfig = null then
            raise (ConfigurationErrorsException("Missing <content> configuration section"))
        
        let styles = 
            names
            |> Seq.map (fun name -> getStyleSheet html name contentConfig)

        MvcHtmlString.Create(String.Concat(styles))

    [<Extension>]
    /// Returns the RequireJS script configuration
    let RequireJsConfig(html:HtmlHelper, rootConfigElementName:string) =
        
        let requireConfigScript = getConfig html rootConfigElementName

        MvcHtmlString.Create(requireConfigScript)
    
    [<Extension>]
    /// Returns the RequireJS script
    let RequireJsScript(html:HtmlHelper, rootConfigElementName:string) =
        
        let script = getScript html rootConfigElementName
        
        MvcHtmlString.Create(script)
    
    [<Extension>]
    /// Returns the RequireJS script configuration and script
    let RequireJS(html:HtmlHelper, rootConfigElementName:string) =
        
        let requireConfigScript = getConfig html rootConfigElementName
        let script = getScript html rootConfigElementName

        let output = requireConfigScript + script

        MvcHtmlString.Create(output)
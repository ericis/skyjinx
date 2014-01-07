namespace SkyJinx.Web.Mvc.Configuration

open System
open System.Configuration
open System.Web.Configuration



[<AbstractClass>]
type NavigationRouteElementBase() =
    inherit ConfigurationElement()
    
    [<ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = false)>]
    member x.Url
        with get() = string x.["url"]
        and set(value:string) = x.["url"] <- value

type NavigationIgnoreRouteElement() =
    inherit NavigationRouteElementBase()

type NavigationApiRouteElement() =
    inherit NavigationRouteElementBase()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("namespaces", IsRequired = false, IsKey = false)>]
    [<ConfigurationCollection(typeof<NamespaceCollection>)>]
    member x.Namespaces
        with get() = x.["namespaces"] :?> NamespaceCollection
        and set(value:NamespaceCollection) = x.["defaults"] <- value
    
    [<ConfigurationProperty("defaults", IsRequired = false, IsKey = false)>]
    [<ConfigurationCollection(typeof<NameValueConfigurationCollection>)>]
    member x.Defaults
        with get() = x.["defaults"] :?> NameValueConfigurationCollection
        and set(value:NameValueConfigurationCollection) = x.["defaults"] <- value

type NavigationControllerRouteElement() =
    inherit NavigationRouteElementBase()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("namespaces", IsRequired = false, IsKey = false)>]
    [<ConfigurationCollection(typeof<NamespaceCollection>)>]
    member x.Namespaces
        with get() = x.["namespaces"] :?> NamespaceCollection
        and set(value:NamespaceCollection) = x.["defaults"] <- value
    
    [<ConfigurationProperty("defaults", IsRequired = false, IsKey = false)>]
    [<ConfigurationCollection(typeof<NameValueConfigurationCollection>)>]
    member x.Defaults
        with get() = x.["defaults"] :?> NameValueConfigurationCollection
        and set(value:NameValueConfigurationCollection) = x.["defaults"] <- value

type NavigationRouteCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> NavigationRouteElementBase

        and set index (value:NavigationRouteElementBase) =
            
            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:NavigationRouteElementBase) =
            x.Item(index) <- value

    override x.CollectionType
        with get() = ConfigurationElementCollectionType.BasicMapAlternate

    override x.ElementName
        with get() = ""

    override x.IsElementName(elementName:string) = 
        if String.IsNullOrWhiteSpace(elementName) then invalidArg "elementName" "elementName is required"
        match elementName.ToUpperInvariant() with
        | "IGNORE" -> true
        | "API" -> true
        | "CONTROLLER" -> true
        | _ -> false

    override x.GetElementKey(element:ConfigurationElement) = 
        box element

    override x.CreateNewElement() =
        (new NavigationControllerRouteElement()) :> ConfigurationElement

    override x.CreateNewElement(elementName:string) =
        if String.IsNullOrWhiteSpace(elementName) then invalidArg "elementName" "elementName is required"

        match elementName.ToUpperInvariant() with
        | "IGNORE" -> (new NavigationIgnoreRouteElement()) :> ConfigurationElement
        | "API" -> (new NavigationApiRouteElement()) :> ConfigurationElement
        | _ -> (new NavigationControllerRouteElement()) :> ConfigurationElement

    member x.Add(route:NavigationRouteElementBase) =
        x.BaseAdd(route)

    member x.Add(route:NavigationControllerRouteElement) =
        x.BaseAdd(route)

    member x.Add(route:NavigationApiRouteElement) =
        x.BaseAdd(route)

    member x.Add(route:NavigationIgnoreRouteElement) =
        x.BaseAdd(route)

    member x.Clear() =
        x.BaseClear()

type NavigationRoutesElement() =
    inherit ConfigurationElement()
    
    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<NavigationRouteCollection>, AddItemName = "ignore,controller,api", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)>]
    member x.Items
        with get() = x.[""] :?> NavigationRouteCollection

[<AllowNullLiteralAttribute>]
type NavigationSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/nav" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> NavigationSection

    [<ConfigurationProperty("routes", IsRequired = true, IsKey = false)>]
    member x.Routes
        with get() = x.["routes"] :?> NavigationRoutesElement
        and set(value:NavigationRoutesElement) = x.["routes"] <- value

[<AllowNullLiteralAttribute>]
type SpaSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/spa" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> SpaSection
        
    [<ConfigurationProperty("view", IsRequired = true, IsKey = false)>]
    member x.ViewPath
        with get() = x.["view"] :?> string
        and set(value:string) = x.["view"] <- value

type ContentStyleElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("url", DefaultValue = "", IsRequired = false, IsKey = false)>]
    member x.Url
        with get() = string x.["url"]
        and set(value:string) = x.["url"] <- value

type ContentStyleCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> ContentStyleElement
        and set index (value:ContentStyleElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:ContentStyleElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let script = element :?> ContentStyleElement
        
        box script.Name

    override x.CreateNewElement() =
        (new ContentStyleElement()) :> ConfigurationElement

    member x.Add(script:ContentStyleElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentStylesElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<ContentStyleCollection>, AddItemName = "style")>]
    member x.Styles
        with get() = x.[""] :?> ContentStyleCollection

type ContentScriptPathElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Url
        with get() = string x.["url"]
        and set(value:string) = x.["url"] <- value

type ContentScriptPathCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> ContentScriptPathElement
        and set index (value:ContentScriptPathElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:ContentScriptPathElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let path = element :?> ContentScriptPathElement
        
        box path.Url

    override x.CreateNewElement() =
        (new ContentScriptPathElement()) :> ConfigurationElement

    member x.Add(script:ContentScriptPathElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentScriptElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("url", DefaultValue = "", IsRequired = false, IsKey = false)>]
    member x.Url
        with get() = string x.["url"]
        and set(value:string) = x.["url"] <- value

    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<ContentScriptPathCollection>, AddItemName = "path")>]
    member x.Paths
        with get() = x.[""] :?> ContentScriptPathCollection

type ContentScriptCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> ContentScriptElement
        and set index (value:ContentScriptElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:ContentScriptElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let script = element :?> ContentScriptElement
        
        box script.Name

    override x.CreateNewElement() =
        (new ContentScriptElement()) :> ConfigurationElement

    member x.Add(script:ContentScriptElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentScriptsElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<ContentScriptCollection>, AddItemName = "script")>]
    member x.Scripts
        with get() = x.[""] :?> ContentScriptCollection

[<AllowNullLiteralAttribute>]
type ContentSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/content" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> ContentSection
        
    [<ConfigurationProperty("styles", IsRequired = true, IsKey = false)>]
    member x.Styles
        with get() = x.["styles"] :?> ContentStylesElement
        and set(value:ContentStylesElement) = x.["styles"] <- value
        
    [<ConfigurationProperty("scripts", IsRequired = true, IsKey = false)>]
    member x.Scripts
        with get() = x.["scripts"] :?> ContentScriptsElement
        and set(value:ContentScriptsElement) = x.["scripts"] <- value

type RequireJsScriptDependencyElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
type RequireJsScriptDependencyCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> RequireJsScriptDependencyElement
        and set index (value:RequireJsScriptDependencyElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:RequireJsScriptDependencyElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let script = element :?> RequireJsScriptDependencyElement
        
        box script.Name

    override x.CreateNewElement() =
        (new RequireJsScriptDependencyElement()) :> ConfigurationElement

    member x.Add(script:RequireJsScriptDependencyElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentScriptDependenciesElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<RequireJsScriptDependencyCollection>, AddItemName = "script")>]
    member x.Scripts
        with get() = x.[""] :?> RequireJsScriptDependencyCollection

type RequireJsScriptElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("exports", DefaultValue = "", IsRequired = false, IsKey = false)>]
    member x.Exports
        with get() = string x.["exports"]
        and set(value:string) = x.["exports"] <- value
        
    [<ConfigurationProperty("dependsOn", IsRequired = false, IsKey = false)>]
    member x.Dependencies
        with get() = x.["dependsOn"] :?> ContentScriptDependenciesElement
        and set(value:ContentScriptDependenciesElement) = x.["dependsOn"] <- value

type RequireJsScriptCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> RequireJsScriptElement
        and set index (value:RequireJsScriptElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:RequireJsScriptElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let script = element :?> RequireJsScriptElement
        
        box script.Name

    override x.CreateNewElement() =
        (new RequireJsScriptElement()) :> ConfigurationElement

    member x.Add(script:RequireJsScriptElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type RequireJsScriptsElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<RequireJsScriptCollection>, AddItemName = "script")>]
    member x.Scripts
        with get() = x.[""] :?> RequireJsScriptCollection

[<AllowNullLiteralAttribute>]
type RequireJsSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/requirejs" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> RequireJsSection
        
    [<ConfigurationProperty("scriptName", IsRequired = true, IsKey = false)>]
    member x.ScriptName
        with get() = x.["scriptName"] :?> string
        and set(value:string) = x.["scriptName"] <- value
        
    [<ConfigurationProperty("mainScriptName", IsRequired = true, IsKey = false)>]
    member x.MainScriptName
        with get() = x.["mainScriptName"] :?> string
        and set(value:string) = x.["mainScriptName"] <- value
        
    [<ConfigurationProperty("scripts", IsRequired = false, IsKey = false)>]
    member x.Scripts
        with get() = x.["scripts"] :?> RequireJsScriptsElement
        and set(value:RequireJsScriptsElement) = x.["scripts"] <- value
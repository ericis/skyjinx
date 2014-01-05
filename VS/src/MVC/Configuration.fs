namespace SkyJinx.Web.Mvc.Configuration

open System
open System.Configuration
open System.Web.Configuration

type ContentElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("path", DefaultValue = "", IsRequired = true, IsKey = false)>]
    member x.Path
        with get() = string x.["path"]
        and set(value:string) = x.["path"] <- value

type ContentElementCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> ContentElement
        and set index value =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:ContentElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let script = element :?> ContentElement
        
        box script.Name

    override x.CreateNewElement() =
        (new ContentElement()) :> ConfigurationElement

    member x.Add(script:ContentElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentGroupElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)>]
    member x.Name
        with get() = string x.["name"]
        and set(value:string) = x.["name"] <- value
    
    [<ConfigurationProperty("path", DefaultValue = "", IsRequired = true, IsKey = false)>]
    member x.Path
        with get() = string x.["path"]
        and set(value:string) = x.["path"] <- value
    
    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<ContentElementCollection>, AddItemName = "item")>]
    member x.Items
        with get() = x.[""] :?> ContentElementCollection

type ContentGroupCollection() =
    inherit ConfigurationElementCollection()

    member x.Item
        with get(index) = 
            x.BaseGet(index) :?> ContentGroupElement
        and set index (value:ContentGroupElement) =

            if Object.ReferenceEquals(null, x.BaseGet(index)) then
                x.BaseRemoveAt(index)
            
            x.BaseAdd(index, value)

    member x.this
        with get(index:int) = 
            x.Item(index)
        and set index (value:ContentGroupElement) =
            x.Item(index) <- value

    override x.GetElementKey(element:ConfigurationElement) = 
        let group = element :?> ContentGroupElement
        
        box group.Name

    override x.CreateNewElement() =
        (new ContentGroupElement()) :> ConfigurationElement

    member x.Add(script:ContentGroupElement) =
        x.BaseAdd(script)

    member x.Clear() =
        x.BaseClear()

type ContentGroupsElement() =
    inherit ConfigurationElement()

    [<ConfigurationProperty("publicRootPath", IsRequired = true, IsKey = false)>]
    member x.PublicRootPath
        with get() = string x.["publicRootPath"]
        and set(value:string) = x.["publicRootPath"] <- box value
    
    [<ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)>]
    [<ConfigurationCollection(typeof<ContentGroupCollection>, AddItemName = "group")>]
    member x.Groups
        with get() = x.[""] :?> ContentGroupCollection
        
[<AllowNullLiteralAttribute>]
type ContentSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/content" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> ContentSection
        
    [<ConfigurationProperty("scripts", IsRequired = true, IsKey = false)>]
    member x.Scripts
        with get() = x.["scripts"] :?> ContentGroupsElement
        and set(value:ContentGroupsElement) = x.["scripts"] <- value
        
    [<ConfigurationProperty("styles", IsRequired = true, IsKey = false)>]
    member x.Styles
        with get() = x.["styles"] :?> ContentGroupsElement
        and set(value:ContentGroupsElement) = x.["styles"] <- value

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
type SiteSection() =
    inherit ConfigurationSection()
    
    static member GetSection(xPathRoot:string) =
        let section = ConfigurationManager.GetSection(sprintf "%s/site" xPathRoot)
        match section with
        | null -> null
        | _ -> section :?> SiteSection

    [<ConfigurationProperty("title", IsRequired = true, IsKey = true)>]
    member x.Title
        with get() = x.["title"] :?> string
        and set(value:string) = x.["title"] <- value

    [<ConfigurationProperty("description", IsRequired = true, IsKey = false)>]
    member x.Description
        with get() = x.["description"] :?> string
        and set(value:string) = x.["description"] <- value

    [<ConfigurationProperty("copyright", IsRequired = true, IsKey = false)>]
    member x.Copyright
        with get() = x.["copyright"] :?> string
        and set(value:string) = x.["copyright"] <- value
if not exist ..\.publish\ mkdir ..\.publish\

..\.nuget\nuget.exe pack ..\src\MVC\MVC.fsproj -OutputDirectory ..\.publish\

..\.nuget\nuget.exe pack ..\src\Web.Angular.Bootstrap\Web.Angular.Bootstrap.csproj -Exclude **\*.dll -OutputDirectory ..\.publish\
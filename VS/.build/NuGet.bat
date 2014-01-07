if not exist ..\.publish\ mkdir ..\.publish\

..\.nuget\nuget.exe pack ..\src\MVC\MVC.fsproj -OutputDirectory ..\.publish\

..\.nuget\nuget.exe pack ..\src\Web.Angular.Bootstrap2\Web.Angular.Bootstrap2.csproj -Exclude **\*.dll -OutputDirectory ..\.publish\
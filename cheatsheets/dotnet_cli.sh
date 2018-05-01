# create new bootstrap solution, -n for solution name
dotnet new sln -n bootstrap

# create nuget config
dotnet new nugetconfig

# create new console application, -n for project name, -o for output folder
dotnet new console -n config_demo -o demos/config_demo

# add project to solution
dotnet sln bootstrap.sln add demos/config_demo/config_demo.csproj

# add package to project
dotnet add demos/config_demo/config_demo.csproj package Microsoft.Extensions.Configuration.Json

# build project
dotnet build demos/config_demo/config_demo.csproj

# run console application project
dotnet run -p demos/config_demo/config_demo.csproj

# run test project with console logger
dotnet test demos/test_demo/test_demo.csproj --logger:"console;verbosity=normal"

# add project reference
dotnet add demos/MSTestDemo_test_temp/*.csproj reference demos/MSTestDemo_to_be_tested_temp/*.csproj
# Creating New Module

1.) Create/modify tables in the database(SSMS)
2.) Scaffold the tables to the project refer to readme.md on EF project
3.) Create DTOs under Service Project Service/Dto/[Parent Module]/[ModuleName]Dto.
4.) Create the Service Interface under Service Project Service/[Parent Module]/Interfaces/I[ModuleName]Service.cs
5.) Create the Service under Service Project Service/[Parent Module]/[ModuleName]Service.cs
6.) Create the Controller under API Project Controllers/[Parent Module]/[ModuleName]Controller.cs
7.) Add the service to the Program.cs ConfigureServices method x: builder.Services.AddScoped<IRoleService, RoleService>();
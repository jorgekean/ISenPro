# First thing First

1.) Always GET/PULL the latest code from the repository before starting any work.
2.) Always PUSH your code to the repository after finishing your work. Do not push broken code.
3.) When in doubt, ask for help.

# Creating New Module

1.) Create/modify tables in the database(SSMS)
2.) Scaffold the tables to the project refer to readme.md on EF project
3.) Create DTOs under Service Project Service/Dto/[Parent Module]/[ModuleName]Dto.
4.) Create the Service Interface under Service Project Service/[Parent Module]/Interfaces/I[ModuleName]Service.cs
5.) Create the Service under Service Project Service/[Parent Module]/[ModuleName]Service.cs
6.) Create the Controller under API Project Controllers/[Parent Module]/[ModuleName]Controller.cs
7.) Add the service to the Program.cs ConfigureServices method x: builder.Services.AddScoped<IRoleService, RoleService>();
8.) After unit testing and all is working fine, compare and sync database schema from SQL Server to the Database project.
9.) Commit and push your code to the repository.

# Database Schema Sync

1.) Right click on the Database project and select Schema Compare.
2.) Select the SOURCE(SQL DB) and TARGET(database project) database.
3.) Click on the compare button.
4.) Select the objects you want to sync.
5.) Click on the update button.



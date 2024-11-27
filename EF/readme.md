

EF Scaffolding Steps

1.) cd EF
2.) dotnet tool install --global dotnet-ef
3.) dotnet add package Microsoft.EntityFrameworkCore.Design
4.) dotnet add package Microsoft.EntityFrameworkCore.SqlServer

5.) Open Package Manager Console and make sure to select the EF project


6.)
# For Adding a new table in User Management - replace <UM_Table> with the table name

    dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models/UserManagement --table <UM_Table> -c TempContext --context-dir Models --force
    copy the newly created DBset and modelBuilder.Entity from TemContext to ISenProContext

# For Adding a new table in System Setup - replace <SS_Table> with the table name
    dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models/SystemSetup --table <SS_Table_> -c TempContext --context-dir Models --force
    copy the newly created DBset and modelBuilder.Entity from TemContext to ISenProContext

# For generating all tables
 
    dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ISenProContext
    dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ISenProContext --force

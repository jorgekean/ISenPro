

EF Scaffolding Steps

1.) dotnet tool install --global dotnet-ef
2.) dotnet add package Microsoft.EntityFrameworkCore.Design
3.) dotnet add package Microsoft.EntityFrameworkCore.SqlServer

4.) dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ISenProContext
    dotnet ef dbcontext scaffold "Server=(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ISenProContext --force

5.) dotnet ef dbcontext scaffold "(LocalDB)\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --table <tablename 1> --table <tablename 2>
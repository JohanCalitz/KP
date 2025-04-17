### How to update the database

1. Add your new class with the table structure under the 'Data.Models' project
2. Under the 'Data' project you need to update the 'KPContext.cs' file with the new DbSet
3. If the class has relationships then you need to add these to the 'KPContext' OnModelCreating method, have a look at the files for examples. 
4. Open Tools->Package Manager Console
5. Set the default startup project to 'Data', you can do this by right clicking the project file
6. In the package manager console set the default project to 'Data'
7. Run the command 'add-migration -Context KPContext'.


First command to run add-migration Initial -Context KingPriceDbContext
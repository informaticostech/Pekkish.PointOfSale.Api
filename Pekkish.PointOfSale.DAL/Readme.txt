Scaffolding new tables:
Package Manager Console

Scaffold-DbContext "Data Source=pekkish.database.windows.net;Initial Catalog=PointOfSale;User ID=pekkishadmin;Password=N@ut1l1u5;Persist Security Info=True;Pooling=false;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Tables AppDeliveryGroups -force

Scaffold-DbContext "Data Source=localhost\SQLEXPRESS;Initial Catalog=PointOfSaleBackup;Trusted_Connection=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Tables AppWatiOrderDetailOptions

You can specify which tables you want to generate entities for by adding the -Tables argument to the command above. For example, -Tables CorpsEmployee, ImageStore

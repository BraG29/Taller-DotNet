# Quality Management Service

## Configuración de persistencia
* Descargar e instalar [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (version Express)
* Una vez terminada la instalación crear una base de datos con nombre **Quality_Management_DB**
* Dentro del proyecto, ejecutar el comando `dotnet ef database update` esto deberia de crea la tabla de **Procedures** en la DB 

### Pasos para agregar una nueva Entidad
  1. Agregar al [DbContext](./DataAccess/QualityManagementDbContext.cs) un nuevo **DbSet** de la entidad en cuestion
  2. Revisar entidad [Procedure](./Model/Procedure.cs) para tomar ejemplos de como definir campos obligatorios, la key de la entidad entre otros
  3. Ejecutar el comando que crea la migration con la orden de `INSERT`: `dotnet ef migrations add "Create <nombre_de_la_entidad> table"`
  4. Ejecutar la el comando para actualizar la DB: `dotnet ef database update` 
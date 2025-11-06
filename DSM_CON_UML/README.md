# Clean Architecture scaffold (generated)

Estructura generada a partir de `domain.model.json` y `diagrama.puml`.

Proyectos principales:

- `ApplicationCore`: Entidades de dominio (EN), Repositorios (interfaces), CEN y CP.
- `Infrastructure`: Adaptadores / NHibernate placeholders y mappings.
- `InitializeDb`: Inicializador mínimo para crear la base de datos y ejecutar SchemaExport (scaffold).

Cómo probar (scaffold):

1. Abrir PowerShell en la raíz del repo.
2. Ejecutar el inicializador (scaffold sólo imprime mensajes):

```powershell
Push-Location .\InitializeDb
dotnet run --project .\InitializeDb.csproj
Pop-Location
```

Siguientes pasos recomendados:
- Implementar mapeos `.hbm.xml` completos en `Infrastructure/NHibernate/Mappings`.
- Implementar las clases concreatas de repositorio en `Infrastructure` usando NHibernate.
- Implementar `NHibernateHelper` para cargar configuración y crear `ISessionFactory`.
- Implementar el `InitializeDb` real: crear/adjuntar LocalDB MDF, ejecutar SchemaExport, y dejar hook para seed usando CEN/CP.

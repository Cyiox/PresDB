# CEDAC PresDB 

This repository contains the code for CEDAC's web interface for the preservation database. It will include functionality of searching, generating reports, importing and exporting information from the preservation database.

## TODO

- Migrate local Access data into SQL Server Express database.
- Prototype initial UI
  - Currently working on: Project Search, Project Details.
- Authentication for CRUD users.

## Database connection

The app queries SQL Server at runtime. The MSSQL extension connection in VS Code is not automatically used by the app.

Set `Database:ConnectionStringName` to select a connection string:

```json
{
  "Database": {
    "ConnectionStringName": "Default"
  }
}
```

Use `Default` for the shared/remote database and `Local` for a local SQL Server or SQL Express database. Keep credentials in user secrets or environment variables rather than committing them to `appsettings*.json`.

`PreservationTest_Export.sql` is a SQL Server data export, not a database file that the app can open directly. To use it offline, first create a local `PreservationTest` database with the required schema, run the export against that database, set `Database:ConnectionStringName` to `Local`, and make sure the local SQL Server instance is running.

Before using comment editing, run `Database/001_CreatePropertyCommentHistory.sql` once against the selected `PreservationTest` database. The application does not create tables automatically.

For the current error, `localhost,1433` is not reachable. Either replace the `Default` connection string with the exact server/database credentials from the MSSQL extension, or start/configure a local SQL Server instance and use the `Local` connection string.



## Directory Layout

- Components
  - Visual UI elements, Including accompanying CSS files
- Models
  - Defines the data structure of an object. For example, the PropertyModel includes the Name,City and Address for a Property. Allows us to make changes to the backend without changing the frontend.  
- Services
  - Contains logic. The decision tree and authentication / CRUD logic should be kept here. 
- wwwroot
  - Contains static assets, like the CEDAC logo.



## Known bugs

- Query issues. For example, when searching for Elm Place in Cambridge, the details page is unable to locate the total number of units, among other information. Access version shows the intended 19 units. 
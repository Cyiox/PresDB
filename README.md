# CEDAC PresDB

This repository contains the code for CEDAC's web interface for the preservation database. It includes functionality for searching, generating reports, and importing and exporting information from the preservation database.

## Known Bugs

- Report drop down menu on the nav bar does not go away when clicking anywhere else on the screen.
  - 40T notice page can not search a keyword more than 2 chracters
- No UI to create a new project from scratch
- No implamentation to pull HUD data directly from the database
- The property details viewer pulls information based on import.
- Property details page source viewer does not pull all of the feilds that the orghinal Access DB does. All those fields need to be pulled as a minimum

## TODO (Updated 10-8-26)

- Can add coloriong for when a project has a data discrepency between external soruces that should not happen. (For example, the project having a differnet amount of total units between two or more soruces.)

- Add admin page. Admin page will include features such as:
  - Creating new project from scratch
  - Pulling the HUD data like the Access DB does automaically.
  - Honestly the whole import tab can be moved over to the admin tab. No normal user should be importing anyway.
  - View logs to seew which user has generated which report(s)

- Add lodading screen when switching between tabs. Some queries take longer than expected.

- UI Development

- [ ] Experimental matching feature
- [ ] Interface to add/remove projects.
      .

- Migrating decision tree
  - Original 10 rules have been completed. Need to add additional rules

- Expansion to report making

- Add DHCD information export

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

Database changes are stored as numbered, idempotent scripts under `Database/`. Apply each script to a database after creating or restoring it. For example, for the Docker SQL Server instance:

```bash
docker cp Database/001_CreatePropertyCommentHistory.sql mssql-dev:/tmp/001_CreatePropertyCommentHistory.sql
docker exec mssql-dev /opt/mssql-tools18/bin/sqlcmd \
  -S localhost,1433 -U sa -P '<password>' -C \
  -d PreservationTest -b \
  -i /tmp/001_CreatePropertyCommentHistory.sql
```

The comment history migration is safe to rerun. It creates `dbo.PropertyCommentHistory`, its index, and its foreign key only when they are missing. The application does not create or update database tables automatically. Apply the same migration to the shared database through the normal database-change process before deploying code that depends on it.

For the current error, `localhost,1433` is not reachable. Either replace the `Default` connection string with the exact server/database credentials from the MSSQL extension, or start/configure a local SQL Server instance and use the `Local` connection string.

## Directory Layout

- Components
  - Visual UI elements, including accompanying CSS files
- Models
  - Defines the data structure of an object. For example, the PropertyModel includes the Name,City and Address for a Property. Allows us to make changes to the backend without changing the frontend.
- Services
  - Contains logic. The decision tree and authentication / CRUD logic should be kept here.
- wwwroot
  - Contains static assets, like the CEDAC logo.

## Known bugs

- Query issues. For example, when searching for Elm Place in Cambridge, the details page is unable to locate the total number of units, among other information. Access version shows the intended 19 units.

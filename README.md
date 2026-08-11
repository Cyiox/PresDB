# CEDAC PresDB 

This repository contains the code for CEDAC's web interface for the preservation database. It will include functionality of searching, generating reports, importing and exporting information from the preservation database.

## TODO

- Migrate local Access data into SQL Server Express database.
- Prototype initial UI
  - Currently working on: Project Search, Project Details.
- Authentication for CRUD users.



## Directory Layout

- Components
  - Visual UI elements, Including accompanying CSS files
- Models
  - Defines the data structure of an object. For example, the PropertyModel includes the Name,City and Address for a Property. Allows us to make changes to the backend without changing the frontend.  
- Services
  - Contains logic. The decision tree and authentication / CRUD logic should be kept here. 
- wwwroot
  - Contains static assets, like the CEDAC logo.
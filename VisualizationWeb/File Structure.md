# Projects and their responsibilities

This Solution utilizes (Semi-)Onion architecture.

## UI

The ´UI´ Project is responsible for the visual presentation of data and processes.

### What belongs here?

- ViewModels
- Controllers
- Views
- JavaScript Files
- Static Files
- Web Configuration
- Authorization & Authentication
- Bootstrap & CSS (+ Other Visual Elements)
- Clientsided WebSocket functionality (if necessary)

### References

This Project is allowed to reference following Projects:
- Application
- DataAccess (avoid as much as possible. Preferably accessed through Application)
- Core


## Application

The ´Application´ Project is responsible for Business Logic.
(such as Error Handling, Validation and Calculations)

### What belongs here?

- Services
- Calculation + Functions
- Serversided WebSocket functionality
- Mediator (for now, refactor and resolve later)
- Other Business-relevant configuration and files

### References

This Project is allowed to reference following Projects:
- DataAccess
- Core


## DataAccess

The ´DataAccess´ Project is responsible for accessing data 
through EntityFramework (and other file access providers).

### What belongs here?

- Database Context (EF)
- Migrations (EF)
- Repositories (EF)
- Other file access functionality

### References

This Project is allowed to reference following Projects:
- Core


## Core

The ´Core´ Project is responsible for Objects, Models, and Entities used throughout the web application.
Functionality in any Form does NOT belong here.

### What belongs here?

- Entities (for now. Try to move to DataAccess asap)
- Models
- Exceptions

### References

This Project shouldn't have any references.


# Folders and their contents

## Solution Files

Any documentation or other non-specific files/documents belong into the ´Solution Files´ folder.


## Tests

All Testing Projects belong into the ´Tests´ folder.
These Projects are to be labeled as such: {Name}Tests.csproj

# Known Issues & Pending Improvements

## Pending Improvements

Due to time constraints and limited initial documentation, 
the previous code base has not been completely refactored.
However, the code itself is completely functional.

Recommended pending improvements and refactorings:
- The *Mediator* class has to many unclear responsibilites. It is advised to completely move its
functionality to other classes and then remove it.
- This application runs on .NET Framework. Migrating to .NET Core will overall improve performance
and code quality.
- In a best practice environment, Database entities are supposed to live in the DataAccess project.
- Implementing an Automapper and seperating all Models into project-specific ones will improve 
understanding and readability (Seperation Of Concern).
- Create Services for Repositories (Unit-Of-Work).
- Implement Dependency Injection to greatly improve code quality.
- Implement proper default values for configurations (configurable).
- Move sensible data (con strings etc.) to an environment file (.env) for security reasons.

## Known Issues

-X-

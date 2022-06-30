# Database

The currently used Database is a **LocalDB** instance of **MSSQL**.

Connection String: <br>
*Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=FloatingCity;Integrated Security=True*

## Setup

To setup a localDB, the corresponding Visual Studio package needs to be installed.<br>
(Access Visual Studio Installer to install said package)

Then the database itself has to be created. The database name should be **FloatingCity**.<br>
(Same as *Initial Catalog* in the connection string)

Provided the database exists, you can load the EF migrations onto the database. <br>
To do so, type *update-database* into the Package Manager Console while having *DataAccess*
selected as your default Project.
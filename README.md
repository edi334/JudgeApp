# JudgeApp

## Backend

### Setup

* Install ``.NET SDK 6.0``
* Install the ``Entity Framework tools``: ``dotnet tool install --global dotnet-ef``
* Install/Configure a ``PostgreSQL`` database
* Set the Default Connection String for the database in the ``appsettings.Development.json`` file
  * Example: ``"Default": "Server=127.0.0.1;Port=5432;Database=judgeapp;User Id=postgres;Password=parola01;"``
* Run ``dotnet ef database update`` to apply all migrations and create the necessary tables in your database
* Create the ``Admin``, ``Participant`` and ``Judge`` roles manually in the ``AspNetRoles`` table
* Creat an account manually in the ``AspNetUsers`` table and give it the admin role in the ``AspNetUserRoles`` table
* Run ``dotnet watch run`` to run the backend and navigate to:
  * ``/admin`` to go to the admin panel
  * ``/swagger`` to go to the API Documentation

### Usage

* To add a new migration run the ``migrate.bat`` (Windows) or ``migrate.sh`` (Linux) script and specify the name of the migration as a command line parameter

## Frontend

* Install ``node.js``
* Open the frontend project and install the necessary dependencies running ``npm install`` 
* Run the project using ``npm run start``. Running this command will make a proxy to the backend server (localhost:7070) so make sure you have the backend up in the first place.

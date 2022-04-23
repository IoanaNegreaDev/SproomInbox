# Sproom Inbox job interview case

1. In order to run the application, please update as necessary the connection string {LibraryDBConnectionString} located in appsettings.json. 
 The Initial migration is provided. At the first run of the API, the database will be created and seeded with 3 users (Hobbit, Wizard, MuadDib) 
and 300 documents having the state Received. 


2. This application consists of an API that connects a MSSQL Db to a simple Web appplication.
I kept the structure of the projects as provided and also the model and the workflow. I tried to adhere to your suggestions while implementing the requirements.


3. This is an overview of the application:

### The REST API 
 - lists documents with query (filter documents on username, state, type) and search. It also provides pagination.
 - updates the state of a single document or a list of documents 
 - it is documented with Swagger, althowgh I didn't manage to make Swagger display in UI info about some action parameters that have complex types
 - it is backed up by a MS Sql Database, that follows the suggested skema
 - it is structured in layers: controller -> service/s -> unit of work -> repositories and uses dependency injection
 - it contains unit tests for a controller, a service, a repository and one utility class. The tests are incipient and some of them must be splitted 
   to be more granular. I used XUnit and Moq. In order to test the repository I used an In Memory database.
 - all the actions, except GET users, require a username as parameter 
 - I took the liberty of considering that after a docunent state is updated to Approved or Rejected, it cannot change state anymore, as these are
   final states.
 - when querying the database for documents I choose to eager load the document states (history)
 - implements only the required end points: 
		GET documents?query ( should contain at least {username}),
		GET documents/{id} {username},
		PUT documents/{id} {newstate} {username},
		PUT documents/{id list} {newstate} {username},
		GET users
 - I verified and solved the warnings only in API

### The web app
 - Lists documents and updates document states (approve or reject).
 - When a document is approved, it must call some "forward to e-mail method" - it is done by calling a mock EmailService.SendApprovedDocumentsEmail in WebApp.Server
 - it is also structured as controller-> service/s and uses dependency injection. The Http calls are done by dedicated services.
 - the code for the Web Client is somewhat clumsy and cluttered and i'm sure it can be done and structured much better.
 - Graphical design was not the focus, but it was time consuming :)


 Thank you for the challenge!






# Lab 1 - Elastic Database Pools

## Introduction

> Note that this lab build on the deployed results of the labs in [exercise 1](https://github.com/software-architects/msft-intelligent-app-workshop/tree/master/Exercises/exercise1-devops).

WWI has started to experiment with *Azure SQL Databases*. Their development database already runs in Azure (this is the database you deployed and used in exercise 1). Now they want to create a test environment to which they can deploy release candidates for the testing team. Over time, they expect to have multiple test and development environments. Additionally, they will have multiple production environments for different countries. Different countries will sometimes run different versions of WWI's ERP system.

> Discuss how *Azure SQL Elastic Database Pools* can help WWI to save money.

## Elastic Database Pools

> Note to presenters: If you have limited time, skip this live demo and prepare the database pool before the workshop.

* Open the *Azure Portal* and navigate to the *Resource Group* you created in exercise 1.

* Start creating a new Elastic Database Pool:

![Create a new Elastic Database Pool](images/new-elastic-db-pool.png)

* Create the Pool with the following options (note that your server name will be different):

![Create a new Elastic Database Pool](images/create-elastic-db-pool.png)

* Once the Pool has been created, open the Pool's configuration:

![Pool configuration](images/configure-db-pool.png)

* Add the existing development database to the pool:

![Add DB to pool](images/add-db-to-pool.png)

* Create a new test database in the pool:

![Create new DB in pool](images/create-db-in-pool.png)

* The new test database must be assigned to the pool, too:

![New DB options](images/new-db-options.png)

Now we have both the development and the test database in the Elastic Database Pool.


# Lab 2 - Database DevOps

## Introduction

The development team at WWI has developed guidelines for doing structural database changes (e.g. new table, new column, etc.). They have a special folder in source control where developers store manually created DB maintenance SQL scripts. When releasing a new version, an administrator executes the scripts before putting the new application in production.

The current practice has frequently led to bugs. Here are some examples:

1. Sometimes developers forget to create maintenance scripts. They change the development DB manually via *SQL Server Management Studio*.
1. Sometimes scripts get lost (e.g. not added to TFS source control).
1. Human mistakes happen (e.g. forget one script during deployment) rather often.
1. Scripts are of poor quality (e.g. not idempotent).

> Discuss what WWI should change in terms of database development and DevOps.

## Conclusions

* WWI wants to automate the creation of DB maintenance scripts.
* WWI wants to automate the deployment of DB maintenance scripts.

## Database DevOps

> Note to presenters: If you have limited time, open the ready-made [*ReadyRoll* solution](ReadyRoll/ErpDb/) instead of creating it from scratch.

* Start *Visual Studio 2017* and create a new *ReadyRoll* project:

![Create ReadyRoll project in VS2017](images/create-ready-roll-project.png)

* Connect ReadyRoll with your Azure database. Note that you should explicitely switch to the network protocol *Tcp/Ip* in the *Advanced Settings* for databases in Azure.

![Connect ReadyRoll with Azure DB](images/connect-ready-roll.png)

* Import the existing development database in the new project. As a result, ReadyRoll will create an initial migration script with the existing `Order` table.

![Import existing DB](images/import-db.png)

> Note to presenters: Describe the concept of migration script. Consider showing the changes that *ReadyRoll* made to the development database (in particular `__MigrationLog` table).

* Open the development DB in *SQL Server Management Studio* (SSMS) or in the Azure Portal's *Query Editor*:

![Query Editor in Azure Portal](images/azure-portal-query-editor.png)

* Create a new table by executing the following SQL script. If you prefer, you can also create the table using interactive table designer in SSMS.

```
create table dbo.Product (
  ID int not null IDENTITY(1, 1),
  ProductCode nvarchar(20) not null,
  ProductDescription nvarchar(max),
  UnitPrice decimal(18,2)
)
```

* Switch back to ReadyRoll in Visual Studio. Hit *Refresh* so that ReadyRoll can detect the changes you made to the DB:

![Refresh ReadyRoll](images/ready-roll-refresh.png)

* Note that ReadyRoll has recognized the new table. *Import and generate* a migration script for the new table. Note that you can manually change or extend the file if necessary.

![Import change and generate script](images/ready-roll-differences.png)

* Build the project in Visual Studio. Take a look at the generated files in `bin/Debug/`.

> Note to presenters: Describe the generated *.sql* and *.ps1* files. Point out that the files are created for the *SQLCMD* tool.

* Open the generated *.sql* file in SSMS.

* Connect SSMS with the target database, i.e. the new test database.

* Switch SSMS into *SQLCMD Mode* and uncomment the *SQLCMD Variables* section:

![SQLCMD Mode in SSMS](images/run-readyroll-in-ssms.png)

Now you can run the generated migrations. Your test database will be updated accordingly.

Note that ReadyRoll build and deploy can be integrated in your VSTS build and release pipeline. The necessary step is available for free in the *VSTS Marketplace*:

![ReadyRoll in VSTS Marketplace](images/ready-roll-marketplace.png)

Once you have installed this step, you can add it to your build and/or release definitions:

![ReadyRoll in VSTS Marketplace](images/ready-roll-from-marketplace.png)


# Lab 3 - NoSQL with CosmosDB

## Introduction

WWI wants to create a new application which allows WWI's stores in various countries to issue trouble tickets (e.g. problems with products, complaints about WWI's internal shared services, etc.). Originally, WWI planned to add the trouble ticket management into its ERP system. As mentioned before, it is based on C# and the .NET Framework. It stores data in SQL DBs.

Some developers argue against adding the new functionality. Their arguments are:

1. The ERP system's code is huge and the documentation is poor. Extending the existing code base is difficult.
1. The data model of the trouble ticket system will be complex. A normalized, relational model would consist of many dozens of tables.
1. All developers with experience in .NET are busy with other projects. Only people in the web development team would have time. Their primary knowledge is JavaScript and Node.js.

> Discuss how WWI should implement the new trouble ticketing system. Consider a Microservice approach.

## Conclusions

* The trouble ticket system will be implemented as an autonomous Microservice.
* It will consist of a platform-independent Web API to support multiple clients (e.g. mobile, web).
* The team building the trouble ticket Microservice is autonomous in their technology decision.
  * In this lab, we assume that the service is written with TypeScript and Node.js.
  * The service will run on Windows and Linux.
  * The team decided to use MongoDB (=NoSQL) for storing data.

## CosmosDB

In this lab, we are going to setup a *CosmosDB* instance (=highly scalable and reliable NoSQL PaaS service) with MongoDB API. Next, we will connect a Node.js Web API with CosmosDB.

> Note to presenters: If you have limited time, skip building the Node.js code step-by-step. Open the [ready-made program](CosmosDB) and just do a code walkthrough.


* Open the *Azure Portal* and navigate to the *Resource Group* you created in exercise 1.

* Start creating a new CosmosDB:

![Create a new CosmosDB](images/cosmosdb-create.png)

* Create the CosmosDB with the following options (note that you have to choose a unique ID):

![Create a new CosmosDB](images/cosmosdb-create-options.png)

* Once the CosmosDB has been created, copy the connection string. We will need it shortly:

![CosmosDB connection string](images/cosmosdb-connection-string.png)

* Create an empty folder for your Node.js application and open the folder in Visual Studio Code.

* Open Visual Studio Code's terminal window.

* Initialize the Node.js application with `npm init`. Hit enter multiple times to accept the defaults.

* Install the Node.js packages necessary at runtime with `npm install restify restify-errors http-status-codes mongodb --save`

* Install the Node.js packages necessary at build time with `npm install typescript ts-node @types/restify-errors @types/http-status-codes @types/mongodb @types/restify --save-dev`

> Note to presenters: Emphasize that our code does not contain any library from CosmosDB. It is just using a MongoDB client library.

* Create the config file for TypeScript `tsconfig.json` with the following content:

```
{
  "compilerOptions": {
    "target": "es2017",
    "module": "commonjs",
    "moduleResolution": "node"
  }
}
```

* Implement the Web API. Here is a sample implementation. Note that you **have to change the MongoDB connection string** to your connection string copied from the Azure portal previously. ** Important Note**: Due to [changes in the MongoDB API version 3](https://github.com/mongodb/node-mongodb-native/blob/HEAD/CHANGES_3.0.0.md), information in the connection has to be URL-encoded. Therefore, you have to replace `=` characters in CosmosDB's password with `%3D`.

```
import {CREATED} from 'http-status-codes';
import * as mongodb from 'mongodb';
import {createServer, plugins} from 'restify';
import {BadRequestError, NotFoundError} from 'restify-errors';

/**
 * Note that this is NOT PRODUCTION-READY code. It just demonstrates how to
 * access CosmosDB via the Mongo API.
 */

// Setup RESTify server
const server = createServer();
server.use(plugins.bodyParser());

// Variables for mongo connection
const mongoUrl =
    'mongodb://user:password%3D%3D@something.documents.azure.com:10255/?ssl=true&replicaSet=globaldb';
let ticketCollection: mongodb.Collection = null;

server.post('/api/ticket', async (req, res, next) => {
  function isValidTicket(ticket: any): boolean {
    // For the sake of simplicity, no error checking is done in this sample
    return !!ticket;
  }

  // Check if body is valid
  if (!isValidTicket(req.body)) {
    next(new BadRequestError('Invalid ticket data'));
  } else {
    // Insert one row into DB
    const insertRepose = await ticketCollection.insertOne(req.body);

    // Build REST API response
    res.send(
        CREATED, req.body,
        {Location: `${req.path()}/${insertRepose.insertedId.toHexString()}`});
  }
});

server.get('/api/ticket/:id', async (req, res, next) => {
  // Query DB
  const result =
      await ticketCollection.findOne({_id: new mongodb.ObjectID(req.params.id)});
  if (result) {
    res.send(result);
  } else {
    next(new NotFoundError());
  }
});

mongodb.MongoClient.connect(mongoUrl, (err, database) => {
  ticketCollection = database.collection('tickets');

  server.listen(8080, function() {
    console.log('API is listening');
  });
})
```

* Start the Web API in the terminal using `./node_modules/.bin/ts-node app.ts`. Alternatively, you can add a script `start` to `package.json` containing `ts-node app.ts`.

* Use any interactive REST client (e.g. [REST Client plugin in Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)) to add a new trouble ticket (see also [*request.http*](CosmosDB/request.http)) as follows:

```
POST http://localhost:8080/api/ticket HTTP/1.1
Accept: application/json
Content-Type: application/json

{
    "customer": "Foo Bar",
    "severity": "high",
    "description": "Lorem ipsum..."
}
```

* Use the REST client to query the created ticket:

```
GET http://localhost:8080/api/ticket/59a6a1d9dcde05027a9fa874 HTTP/1.1
Accept: application/json
```


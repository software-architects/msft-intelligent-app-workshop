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
  console.log(err);
  ticketCollection = database.db('demo').collection('tickets');

  server.listen(8080, function() {
    console.log('API is listening');
  });
})

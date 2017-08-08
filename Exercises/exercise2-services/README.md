# Exercise 2: Services

## Content

Microservices, API-first approach, Serverless Computing and related technologies are the focus of this part:
* Short recap: What are Microservices?
* Short recap: What is PaaS and Serverless Computing?
* Technological consequences of an API-first culture
* Microsoft’s related offerings in Azure (e.g. App Service, Azure Functions, Azure Service Bus)

## Material

This block contains presentations and demos/hands-on exercises. Slides about
* Microservices,
* Serverless computing,
* application gateways, and
* related Azure services (e.g. App Service, Azure Functions, Logic Apps, API Management)
will be provided. A step-by-step description of the following demos for will be created:
* Azure Functions: Serverless function triggered by webhook (e.g. Office 365 incoming mail)
* Azure Functions: Connect microservices using Service Bus
* Azure Functions Proxies: Setup application gateway
* Azure Logic Apps: Setup simple workflow related to functions previously created

## Sample
### Scenario
* We want to refactor our existing order process logic because
  * it's full of if statements to implement the business rules
  * it contains supplier specific code mixed with core logic
  * it is hard to test

### Steps
* Create a service bus queue for new order messages (use topics)
* Create an Azure function for ordering from supplier
* Create a Logic App for handling a price change workflow (supplier calls our Logic App to notify about a price change, manual approval process)
* Create an API proxy to get a common interface for Function and Logic App services

### Responsibility
* Roman
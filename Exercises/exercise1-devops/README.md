# Exercise 1: DevOps

## Content

The second part focuses on DevOps concepts:
* Short recap: What is DevOps?
* What are the consequences of DevOps on software companies (technology, organization, business model)?
* Infrastructure is code
* Microsoft’s DevOps offerings in VSTS


## Material

This block contains presentations and demos/hands-on labs. Slides about 
* DevOps,
* Infrastructure automation (Azure Resource Manager), and
* VSTS
will be provided. A step-by-step description of the following demos will be created:
* Automate resource creation with ARM (ARM template, Azure CLI)
* VSTS: Continuous integration with hosted build controller for a .NET Core web API (including unit tests tests)
* VSTS: Continuous deployment of web API to Azure App Service (eventually including performance tests)

## Sample
### Scenario
Create a new tenant for subsidiary, testing, etc.

### Steps
* ARM-Template 
  * create a new SQL Server 
  * import a small bakpac file
  * create an App Service Plan
  * create an API App
  * create an Application Insights resource

### Responsibility
* Rainer


# Lab

In this lab, we are going to automate resource management with [*Azure Resource Manager* templates](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-overview#template-deployment). In order to do this lab, you need the following prerequisites:

* Azure subscription in which you can create and manage resource groups and their content.
* [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/overview)
  * We recommend writing and running the PowerShell code in this lab using [PowerShell ISE](https://docs.microsoft.com/en-us/powershell/scripting/introducing-the-windows-powershell-ise?view=powershell-6)
* [Visual Studio Code](https://code.visualstudio.com); install the following extensions:
  * [Azure Resource Manager Tools](https://marketplace.visualstudio.com/items?itemName=msazurermtools.azurerm-vscode-tools)

> Note to presenters: If you have limited time, skip building the ARM Template step-by-step. Open the [ready-made template](erp.json) and just do a code walkthrough.


## Create Basic Structure of ARM Template

* Create an empty folder in which you can store all the files of this lab.
* Open the folder in *Visual Studio Code*
* Create a new *ARM Template* file called `erp.json` (we are deploying an ERP system, hence the name).
* Add the following basic structure for our template:

```
{
    "$schema": "http://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {  },
    "variables": {  },
    "resources": [  ],
    "outputs": {  }
}
```

* Read more about the [format of *ARM Templates*...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-authoring-templates#template-format)


## Create Parameters

* Add the following parameters to the `parameters` section. These parameter help to keep your template reusable:

```
"parameters": {
    "dbAdminUser": {
        "type": "string",
        "defaultValue": "demo",
        "metadata": {
            "description": "The administrator login for the created SQL Server."
        }
    },
    "dbAdminPassword": {
        "type": "securestring",
        "metadata": {
            "description": "The SQL Server's administrator password."
        }
    },
    "environment": {
        "type": "string",
        "allowedValues": [
            "dev",
            "stg",
            "prod"
        ],
        "defaultValue": "dev",
        "metadata": {
            "description": "The environment you want to deploy."
        }
    },
    "databaseEdition": {
        "type": "string",
        "allowedValues": [
            "Basic",
            "Standard",
            "Premium"
        ],
        "defaultValue": "Standard",
        "metadata": {
            "description": "The database edition"
        }
    }
},
```

* Read more about [*ARM Template Parameters*...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-authoring-templates#parameters)
* Read more about [best practices regarding *ARM Template Parameters*...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-template-best-practices#parameters)

> Note to presenters: Speak about the importance of parameters for interactive deployments. Use e.g. the *Deploy to Azure* button in e.g. an [Azure Quickstart template](https://github.com/Azure/azure-quickstart-templates/tree/master/201-web-app-blob-connection) to demonstrate the point.


## Create Variables

* As always in programming: Don't repeat yourself. Store values that are needed throughout your template in variables. Therefore, add the following code to the `variables` section:

```
"variables": {
    "namePrefix": "wwi",
    "sqlServerName": "[concat(variables('namePrefix'), 'sql', parameters('environment'), uniqueString(resourceGroup().id))]",
    "sqlDbName": "[concat('sql', parameters('environment'), 'erp01')]",
    "webFarmName": "[concat(variables('namePrefix'), 'webfarm', parameters('environment'), uniqueString(resourceGroup().id))]",
    "webAppName": "[concat(variables('namePrefix'), 'web', parameters('environment'), uniqueString(resourceGroup().id))]",
    "webAppSlotName": "staging2",
    "appInsightsName": "[concat(variables('namePrefix'), 'ai', parameters('environment'), uniqueString(resourceGroup().id))]",
    "location": "[resourceGroup().location]",
    "webDeployUser": "publisher"
},
```

* Read more about [*ARM Template Variables*...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-authoring-templates#variables)
* Read more about [best practices regarding *ARM Template Variables*...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-template-best-practices#variables)
* Read more about [*ARM Template Functions*](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-template-functions) like `concat` and `uniqueString`...
* Read more about [best practices for resource names...](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-template-best-practices#resource-names)


## Creating *Azure SQL* Resources

* We are ready to create our first resources. Let's start with an *Azure SQL* server and database. Add the following code into the `resources` array of your template:

```
{
    "name": "[variables('sqlServerName')]",
    "type": "Microsoft.Sql/servers",
    "apiVersion": "2014-04-01",
    "location": "[variables('location')]",
    "tags": {
        "Project": "Intelligent Apps Workshop",
        "Tier": "Database",
        "Environment": "[parameters('environment')]"
    },
    "properties": {
        "administratorLogin": "[parameters('dbAdminUser')]",
        "administratorLoginPassword": "[parameters('dbAdminPassword')]"
    },
    "resources": [
        {
            "name": "AllowAllIps",
            "type": "firewallRules",
            "apiVersion": "2014-04-01",
            "dependsOn": [
                "[concat('Microsoft.Sql/servers/', variables('sqlServerName'))]"
            ],
            "properties": {
                "startIpAddress": "0.0.0.0",
                "endIpAddress": "255.255.255.255"
            }
        },
        {
            "name": "[variables('sqlDbName')]",
            "type": "databases",
            "apiVersion": "2014-04-01",
            "location": "[variables('location')]",
            "dependsOn": [
                "[variables('sqlServerName')]"
            ],
            "tags": {
                "Project": "Intelligent Apps Workshop",
                "Tier": "Database",
                "Environment": "[parameters('environment')]"
            },
            "properties": {
                "edition": "[parameters('databaseEdition')]"
            }
        }
    ]
}
```

* Note IntelliSense when using *Visual Studio Code* to edit your template file.
* Note the nesting of resource (e.g. *Firewall Rules* inside of `resources` of the *Azure SQL Server*)
* Read more about [*SQL resources* in template...](https://docs.microsoft.com/en-us/azure/templates/microsoft.sql/servers)
* Take a look at the [*ARM Schemas* in GitHub...](https://github.com/Azure/azure-resource-manager-schemas/tree/master/schemas/2014-04-01)

> Note to presenters: Emphasize that ARM is not specifically for *IaaS* or *PaaS*. It can create *any* kind of resource available in Azure, independent of its IaaS- or PaaS-nature.


## Creating *Application Insights* Resource

* Next, we want to add an *Application Insights* resource. Add the following code into the `resources` array of your template:

```
{
    "name": "[variables('appInsightsName')]",
    "type": "microsoft.insights/components",
    "apiVersion": "2015-05-01",
    "location": "[variables('location')]",
    "tags": {
        "Project": "Intelligent Apps Workshop",
        "Tier": "Web",
        "Environment": "[parameters('environment')]"
    },
    "properties": {
        "Application_Type": "web"
    }
}
```

* Read more about [*Application Insights resources* in template...](https://docs.microsoft.com/en-us/azure/templates/microsoft.insights/components)


## Creating *App Service* Resources

* We want to deploy a web app. For that, we need an *App Service Plan*. Add the following code into the `resources` array of your template:

```
{
    "name": "[variables('webFarmName')]",
    "type": "Microsoft.Web/serverfarms",
    "apiVersion": "2016-09-01",
    "location": "[variables('location')]",
    "tags": {
        "Project": "Intelligent Apps Workshop",
        "Tier": "Web",
        "Environment": "[parameters('environment')]"
    },
    "properties": {
        "name": "[variables('webFarmName')]"
    },
    "sku": {
        "name": "S1",
        "tier": "Standard",
        "size": "S1",
        "family": "S",
        "capacity": 1
    }
}
```

* Read more about [*Serverfarm* resources in template...](https://docs.microsoft.com/en-us/azure/templates/microsoft.web/serverfarms)


## Creating *Web App* Resources

* Now we have all we need to finally deploy the web app. Add the following code into the `resources` array of your template:

```
{
    "name": "[variables('webAppName')]",
    "type": "Microsoft.Web/sites",
    "apiVersion": "2016-08-01",
    "location": "[variables('location')]",
    "tags": {
        "Project": "Intelligent Apps Workshop",
        "Tier": "Web",
        "Environment": "[parameters('environment')]"
    },
    "dependsOn": [
        "[variables('appInsightsName')]"
    ],
    "properties": {
        "serverFarmId": "[resourceId('Microsoft.Web/serverfarms/', variables('webFarmName'))]",
        "siteConfig": {
            "defaultDocuments": [
                "default.html"
            ],
            "netFrameworkVersion": "v4.6",
            "publishingUsername": "[variables('webDeployUser')]",
            "connectionStrings": [
                {
                    "name": "DB",
                    "connectionString": "[concat('Server=tcp:', variables('sqlServerName'),'.database.windows.net,1433;Initial Catalog=', variables('sqlDbName'), ';Persist Security Info=False;User ID=', parameters('dbAdminUser'), ';Password=', parameters('dbAdminPassword'), ';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;')]",
                    "type": "SQLAzure"
                }
            ],
            "appSettings": [
                {
                    "name": "appInsightsInstrumentationKey",
                    "value": "[reference(resourceId('Microsoft.Insights/components', variables('appInsightsName')), '2015-05-01').InstrumentationKey]"
                }
            ],
            "use32BitWorkerProcess": false,
            "webSocketsEnabled": true,
            "alwaysOn": true,
            "cors": {
                "allowedOrigins": [
                    "*"
                ]
            }
        }
    },
    "resources": [
        {
            "name": "[variables('webAppSlotName')]",
            "type": "slots",
            "apiVersion": "2016-08-01",
            "location": "[variables('location')]",
            "tags": {
                "Project": "Intelligent Apps Workshop",
                "Tier": "Web",
                "Environment": "[parameters('environment')]"
            },
            "dependsOn": [
                "[variables('appInsightsName')]",
                "[variables('webAppName')]"
            ],
            "properties": {
                "siteConfig": {
                    "defaultDocuments": [
                        "default.html"
                    ],
                    "netFrameworkVersion": "v4.6",
                    "publishingUsername": "[variables('webDeployUser')]",
                    "connectionStrings": [
                        {
                            "name": "DB",
                            "connectionString": "[concat('Server=tcp:', variables('sqlServerName'),'.database.windows.net,1433;Initial Catalog=', variables('sqlDbName'), ';Persist Security Info=False;User ID=', parameters('dbAdminUser'), ';Password=', parameters('dbAdminPassword'), ';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;')]",
                            "type": "SQLAzure"
                        }
                    ],
                    "appSettings": [
                        {
                            "name": "appInsightsInstrumentationKey",
                            "value": "[reference(resourceId('Microsoft.Insights/components', variables('appInsightsName')), '2015-05-01').InstrumentationKey]"
                        }
                    ],
                    "use32BitWorkerProcess": false,
                    "webSocketsEnabled": true,
                    "alwaysOn": true,
                    "cors": {
                        "allowedOrigins": [
                            "*"
                        ]
                    }
                }
            },
            "resources": []
        }
    ]
}
```

* Note how we add application configuration for dependent resources (DB, Application Insights)
* Read more about [*Web Site* resources in template...](https://docs.microsoft.com/en-us/azure/templates/microsoft.web/sites)

> Note to presenters: Describe the concept of *Slots* in *App Service* and why it is important in a DevOps world.


## Deploy Template with PowerShell

* Open *PowerShell ISE*
* Add the following code to a script called `deploy.ps1`:

```
# Check if user is already signed in
Try {
  Get-AzureRmContext | Out-Null
} Catch {
  if ($_ -like "*Login-AzureRmAccount to login*") {
    Login-AzureRmAccount
  }
}

# Select subscription where name contains `MSDN`
Get-AzureRmSubscription | where { $_.SubscriptionName -like "*MSDN*" } | Select-AzureRmSubscription

# Set some string constants
$rg = "intel-app-workshop3"
$location = "westeurope"
$dep = "Deployment-" + [guid]::NewGuid()
$path = "C:\Code\GitHub\msft-intelligent-app-workshop\Exercises\exercise1-devops\"
$vaultName = "wwikvdev01"
$dbAdminUser = "demo"

# Check if resource group already exists
$group = Get-AzureRmResourceGroup -Name $rg -ErrorAction SilentlyContinue
if (!$group) {
    New-AzureRmResourceGroup -Name $rg -Location $location

    # Ask for username and password and store it in KeyVault
    New-AzureRmKeyVault -VaultName $vaultName -ResourceGroupName $rg -Location "westeurope" | Out-Null
    Set-AzureKeyVaultSecret -VaultName $vaultName -Name "SqlAdminPassword" -SecretValue (Get-Credential).Password | Out-Null
}

# Get username and password from KeyVault
$password = Get-AzureKeyVaultSecret -VaultName $vaultName -Name "SqlAdminPassword"
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($dbAdminUser, $password.SecretValue)  

# Deploy ARM template
New-AzureRmResourceGroupDeployment -ResourceGroupName $rg -TemplateFile "$path\erp.json" `
    -Name $dep -dbAdminUser $dbAdminUser -dbAdminPassword $cred.Password
```

* Execute the script and watch how ARM creates your resources.
* Read more about [Azure PowerShell...](https://docs.microsoft.com/en-us/powershell/azure/overview)

> Note to presenters: Describe the importance of *KeyVault* as a secure location to store passwords.

# Monitoring

## Introduction

Currently, WWI is running their ERP system on a hand full of VMs in their own computer room. Logging and monitoring currently works like this:

* The server-side components of the ERP system write to *Windows Event Log*.
* In case of problems, the admin team connect to the corresponding server using RDP and looks into the *Windows Event Log*.
* Problems in the ERP client full client app are written in a log file on the client's local disk. In case of problems, the user can send the log file to the admin team.
* Newer parts of the ERP system write to a log table in the ERP database. This table has been growing rapidly and the DB team is starting to get worried about its size negatively influencing the DB performance.

The development team of WWI has decided to go for Microservices in the future. The admin team is worried about how they should monitor all these services in the future given that they cannot control the servers anymore.

> Discuss what WWI should consider concerning logging and telemetry in a Microservices environment.


## Using Application Insights

In this lab we will add Application Insights (AI) to a Web application and see how it helps during development, monitoring and root cause analysis.

### Create a new Web application and configure AI
* Open *Visual Studio 2017*
* Create a new *ASP.NET Core Web Application*

![Create a new project](images/newproject.png)

* Choose the *Web Application (Model-View-Controller)* template and create the project

![Choose a template](images/newproject-template.png)

* Let's add Application Insights to this new project by using the context menu.

![Add Application Insights](images/newproject-addai.png)

* Click on *Start Free* to configure your AI resource
* Choose an *Azure subscription*
* Click on *Configure settings* to choose the *Resource group*, a name for the *Application Insights Resource* and a *Location*
* Decide, if you want to stop collecting data after 1 GB/Month to remain in the free plan.

![Configure Application Insights](images/configure-ai.png)

* Click on *Register*, your Application Insights Resource is being created now.

![Application Insights is created](images/ai-loading.png)

### See telemetry data
* Start your web application and click through a few pages
* While you are debugging your application, Visual Studio can show telemetry data from the current debug session. Click on the *Application Insights* button in your menu bar to open the *Application Insights Search* window.

![Open telemetry data](images/vs-openai.png)

* You see all requests, errors, dependency failures, etc. on one place - currently filtered to your debug session. 
* You can filter the telemetry data by time, HTTP method, request, response code, etc.

![Application Insights Search window](images/ai-search.png)

* Let's change some lines of code to produce an exception. We will make a HTTP request to an unreachable URL. Open the *Controllers/HomeController.cs* file and add the below using block. Be also aware of the changes in the method signature (*async* keyword and return type of *Task<IActionResult>*).

```cs
public async Task<IActionResult> About()
{
    ViewData["Message"] = "Your application description page.";

    using (var client = new HttpClient())
    {
        var result = await client.GetStringAsync("http://request-to-nothing.com");
    }

    return View();
}
```

* Start a new debugging session, click on the About page and see the *Application Insights* telemetry data again.
* You'll see the exception and the dependency on the external service.

![See exception data](images/ai-exception.png)

* In the exception details you can jump directly to the code

![Jump to the code](images/ai-jumptocode.png)

* Application Insights integrates into CodeLens, you see the amount of requests and failures and all the exceptions that occured in your method. Currently you are filtered to your debugging session, but you can get the same experience for production data as well.

![CodeLens](images/ai-codelens.png)

![CodeLens](images/ai-codelens2.png)

## Application Insights Trends
* To see some useful test data, we should slow down one of your pages a little bit.
* Open the *Controllers/HomeController.cs* file and add an increasing delay to the *Index* action.

```cs
private static int delay = 200;

public async Task<IActionResult> Index()
{
    await Task.Delay(delay);

    delay = delay * 2;

    return View();
}
```

* Start a new debugging session.
* From the Application Insights menu you can open additional views. Try out *Explore Telemetry Trends*.

![Explore Telemetry Trends](images/ai-trends.png)

* Use the *Application Insights Trends* view to visualize your data over time and group by Performance, Country, Request, etc.

![Telemetry Trends](images/ai-trends-view.png)

## Application Insights Portal

* Additionally you can view the telemetry data in the Azure portal.

![Open in Azure Portal](images/ai-portal-menu.png)

* Explore the different views, click through some requests and exceptions. Show the *performance view* when clicking on the server response time.

![Application Insights in Azure Portal](images/ai-portal.png)

## Availability tests
* You can create availability tests to ensure that your services are up and running.
* Click on *Availability* in your Application Insights Azure Portal

![Open Availability](images/portal-availability.png)

* Click on *Add test* and show the options to create a test. As you didn't publish your website, you can cancel the creation of the test for now.

![Availability test](images/availabilitytest.png)

## Analytics
* Open *Analytics* from your Azure Portal.

![Open Analytics](images/portal-analytics.png)

* Explore the *Analytics* window and maybe switch to the [*Demo workspace*](https://analytics.applicationinsights.io/demo) to see more relevant data.

![Analytics](images/analytics.png)

## Application map
* The [*Application map*](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-app-map) shows all the components of your application and how they work together.

![Open Application Map](images/portal-applicationmap.png)

![Application Map](images/applicationmap.png)


## Integrate EF Core 2

> **Important**: With the following sample you can integrate a database access with Entity Framework Core 2 to your web app. Unfortunately there was no Application Insights support for EF Core 2 at the time this training material was created. Check out the [Roadmap](https://github.com/aspnet/EntityFrameworkCore/wiki/Roadmap), maybe EF Core 2.1 is already released and AI support is included.

* Let's add some database code to our web app.
* Add the NuGet package *Microsoft.EntityFrameworkCore.SqlServer* to your project.

![NuGet package manager](images/nuget-ef.png)

* Create a *Data* folder in your project (for demo reasons we pass on creating a multi-tier application)
* Add a class named *Supplier*

```cs
[Table("Suppliers", Schema = "Purchasing")]
public class Supplier
{
    public int SupplierID { get; set; }

    public string SupplierName { get; set; }
}
```

* Add a class named *WWIContext*

```cs
public class WWIContext : DbContext
{
    public WWIContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Supplier> Suppliers { get; set; }
}
```

* In the *Startup.cs* file modify the existing *ConfigureServices* method and configure your database context for dependency injection

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    services.AddDbContext<WWIContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("WWIDatabase")));
}
```

* Open the *appsettings.json* file and add your connection string

```json
{
  "ConnectionStrings": {
    "WWIDatabase": "Server=tcp:intel-app-workshop.database.windows.net,1433;Initial Catalog=WWI-Standard;Persist Security Info=False;User ID=demo;Password=xxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

* Add a constructor to your *HomeController*, the *WWIContext* instance is injected

```cs
private WWIContext db;

public HomeController(WWIContext db)
{
    this.db = db;
}
```

* Modify your *Contact* action and add some database code

```cs
public IActionResult Contact()
{
    ViewData["Message"] = "Your contact page.";
            
    var suppliers = this.db.Suppliers.ToList();
            
    return View();
}
```

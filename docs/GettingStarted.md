# Walkthrough: Getting started.

## Create your first application with Ozzy
During this walkthough we are going to use Sql Server via Enity Framwork as persistence mechanism
Entities will be saved in tables as in classic approach
To start you will need to follow these steps:

## What will we do

## Prepare Infrastructure

### Install nuggets
We will need following nuggets:
* Ozzy.Server - Core functionality of Ozzy App Node to use in your application
* Ozzy.Server.EntityFramework - Implementation of Ozzy Persistance to Entity Framework Core compatible Databases
* Microsoft.EntityFrameworkCore.SqlServer - As far as we will use Sql Server as our Entity Framework Core compatible Database for Ozzy Persistance

You can obtain latest nugets from [this myget feed](https://www.myget.org/F/sashaozz/api/v3/index.json). Please take in mind that nugets on this feed are prerelease versions so don't' forget to mark "Include Prerelease" in Visual Studio.

### Setup appSettings.config
Setup your connection string to database
```
  "ConnectionStrings": {
    "SampleDbContext": "Data Source=.;Initial Catalog=ozzy;Integrated Security=True;"
  }
```
It is not needed to have this database created, we will create it automatically later in this walkthrough using EF Core migrations.
### Setup database context
Create 
```
public class SampleDbContext : AggregateDbContext
{
    public SampleDbContext(IExtensibleOptions options) : base(options)
    {
    }
}
```
### Configure Ozzy Domain
```
services
    .AddOzzyDomain<SampleDbContext>(options =>
    {
        options.UseInMemoryFastChannel();
    })
    .UseEntityFramework((options =>
    {
        options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"));
    }));
```
### Configure Ozzy node
```
services.ConfigureOzzyNode<SampleDbContext>();
```

```
app.UseOzzy().Start();
```

### Create DataBase

### Overview
At this point we have fully functional Ozzy Node and we can start using its features like
* Background Processes
* Queues
* etc

## Create our Domain Model

```
public class ContactFormMessage : AggregateBase
{
    public string From { get; protected set; }
    public string Message { get; protected set; }
    public bool MessageSent { get; protected set; }

    public ContactFormMessage(string from, string message)
    {
        From = from;
        Message = message;
        this.RaiseEvent(new ContactFormMessageRecieved { From = from, Message = message, MessageId = Id });
    }
}
```


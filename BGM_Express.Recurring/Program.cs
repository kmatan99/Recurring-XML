using BGM_Express.Core.Repository;
using BGM_Express.Core.Service;
using BGM_Express.Domain;
using BGM_Express.Recurring;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<BGM_DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BGM_Express")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddHostedService<FetchWorker>();
builder.Services.AddHostedService<DispatchWorker>();
builder.Services.AddTransient<IXMLService, XMLService>();
builder.Services.AddTransient<IBaseRepository, BaseRepository>();

var host = builder.Build();
host.Run();

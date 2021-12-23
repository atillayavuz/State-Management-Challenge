using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using StateManagement.Business.Flow.Command;
using StateManagement.Business.State.Command;
using StateManagement.Business.State.Queries;
using StateManagement.Business.Task.Command;
using StateManagement.Data.Repositories.EventStore;
using StateManagement.Data.Repositories.Mongo;
using StateManagement.Domain;

namespace StateManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var eventStoreConnection = EventStoreClientSettings.Create(
                connectionString: Configuration.GetValue<string>("EventStore:ConnectionString"));

            var eventStoreClient = new EventStoreClient(eventStoreConnection);
            services.AddSingleton(eventStoreClient);

            services.AddSingleton<EventStoreRepository>();

            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CreateFlowCommandHandler));
            services.AddMediatR(typeof(MoveBackwardCommandHandler));
            services.AddMediatR(typeof(TaskForwardCommandHandler));
            services.AddMediatR(typeof(CreateFlowStateCommandHandler));
            services.AddMediatR(typeof(CreateTaskCommandHandler));
            services.AddMediatR(typeof(CreateStateCommandHandler));
            services.AddMediatR(typeof(StartProcessCommandHandler)); 
            services.AddMediatR(typeof(GetProcessLastStateHandler));

            var mongoUrlBuilder = new MongoUrlBuilder(Configuration.GetValue<string>("Mongo:ConnectionString"));
            var settings = MongoClientSettings.FromUrl(mongoUrlBuilder.ToMongoUrl());
             
            services.AddSingleton<IMongoClient>(new MongoClient(settings));
            services.AddSingleton<IMongoRepository<Flow>, MongoRepository<Flow>>();
            services.AddSingleton<IMongoRepository<State>, MongoRepository<State>>();
            services.AddSingleton<IMongoRepository<FlowState>, MongoRepository<FlowState>>();
            services.AddSingleton<IMongoRepository<Task>, MongoRepository<Task>>();
             
            services.AddSingleton<ProcessAggregate>();
              
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StateManagement Challenge",
                    Description = "StateManagement API Swagger",
                    Contact = new OpenApiContact { Name = "Atilla Yavuz", Email = "atillayavuz@gmail.com" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StateManagement API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

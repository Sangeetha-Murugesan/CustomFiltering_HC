using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Types;
using HotChocolate.Types.NodaTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomFiltering
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {

                services.AddGraphQLServer("test")
                .AddFiltering<CustomConvention>()
                .AddConvention<IFilterConvention>(new FilterConventionExtension(
                    x => x.AddProviderExtension(new QueryableFilterProviderExtension(
                    y => y.AddFieldHandler<QueryableStringInvariantEqualsHandler>().AddFieldHandler<QueryableStringInvariantContainsHandler>())
                )))

                .AddSorting()
                .SetPagingOptions(new HotChocolate.Types.Pagination.PagingOptions { DefaultPageSize = 50, MaxPageSize = 50})
              
                .AddDocumentFromString(@"type Query{ getpartSaleOrderInquiry (tcdReq : String) : getPartSaleOrderInquiry}")
               
                .AddType<getPartSaleOrderInquiry>()

                .BindRuntimeType<BasPSOHub_getpartSaleOrderInquiry>()
                .AddResolver("Query", "getpartSaleOrderInquiry", t => t.Resolver<BasPSOHub_getpartSaleOrderInquiry>().getpartSaleOrderInquiry(t).Result)

                .SetRequestOptions(_ => new HotChocolate.Execution.Options.RequestExecutorOptions { ExecutionTimeout = TimeSpan.FromMinutes(10) })

                .UseField(next =>
                {

                    HotChocolate.Resolvers.FieldDelegate fieldDelegate = async context =>
                    {
                        //Handled some logic...

                        await next.Invoke(context);
                    };
                    return fieldDelegate;
                });

                services.AddControllers();

                services.AddErrorFilter<GraphQLErrorFilter>();
                services.AddHttpContextAccessor();

            }
            catch (Exception Ex)
            {                
                throw Ex;
            }


        }
    
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                //app.UseHttpsRedirection();

                app.UseRouting();
                app.UseWebSockets();
                //app.UseCors();

                app.UseEndpoints(endpoints =>
                {


                    endpoints.MapGraphQL("/test", schemaName: "test");
                    endpoints.MapControllers();
                    endpoints.MapGraphQL();
                    endpoints.MapGet("/", context =>
                    {
                        context.Response.Redirect("/playground");
                        return Task.CompletedTask;
                    });
                });
                app.UsePlayground("/graphql", "/playground");
            }
            catch (Exception Ex)
            {
               
                throw Ex;
            }
        }

    }
    public class GraphQLErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error.WithMessage(error.Exception.Message);
        }
    }
}

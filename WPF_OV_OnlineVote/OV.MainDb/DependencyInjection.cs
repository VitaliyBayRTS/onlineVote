using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb
{
    public static class DepenedencyInjection
    {

        private static IServiceCollection TryAddOvMainDbQueryExecutor(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddScoped<OvMainDatabase>();
            //serviceCollection.TryAddScoped<IQuertExecutor<OvMainDatabase>>(provider =>
            //    new QueryExecutor<OvMainDatabase>(
            //        new KeyViolationInterceptingQueryExecutor(
            //            new QueryExecutor(
            //                provider.GetRequiredService<OvMainDatabase>()
            //            )
            //        )
            //    )
            //);

            return serviceCollection;
        }

        private static object TryAddMainDbDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ConnectionStringConfig>(configuration.GetSection("ConnectionStrings"));

            serviceCollection.TryAddOvMainDbQueryExecutor();

            serviceCollection.AddDbContext<IOvMainDbContext, OvMainDbContext>(
                builder =>
                    builder.UseSqlServer(
                        configuration.GetSection("ConnectionsStrings").Get<ConnectionStringConfig>().OvMainDb)
                //.Get<ConnectionStringConfig>()
                //.OvMainDb;
                ,
                ServiceLifetime.Transient
             );

            serviceCollection.TryAddScoped<IOvMainDbContextFactory, OvMainDbContextFactory>();

            return serviceCollection;
        }
    }
}

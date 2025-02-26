using AutoMapper;
using InviceConsoleApp.Service.Interfaces;
using InviceConsoleApp.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceConsoleApp.Infra.CrossCutting.IoC
{
    public static class SimpleInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Application
            services.AddAutoMapper(GetAutoMapperProfilesFromAllAssemblies().ToArray());
            services.AddScoped<IInvoiceConsoleAppService, InvoiceConsoleAppService>();


            // Infra - Data
        }

        /// <summary>
        /// See article below
        /// https://stackoverflow.com/questions/40275195/how-to-set-up-automapper-in-asp-net-core
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetAutoMapperProfilesFromAllAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var aType in assembly.GetTypes())
                {
                    if (aType.IsClass && !aType.IsAbstract && aType.IsSubclassOf(typeof(Profile)))
                    {
                        yield return aType;
                    }
                }
            }
        }
    }
}

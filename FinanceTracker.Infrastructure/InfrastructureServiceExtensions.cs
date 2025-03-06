using FinanceTracker.Infrastructure.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Supabase;

namespace FinanceTracker.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("FinanceTracker.Infrastructure")));


            services.AddSingleton(provider =>
            {
                var supabaseUrl = configuration["Supabase:Url"];
                var supabaseKey = configuration["Supabase:Key"];

                var supabaseClient = new Client(supabaseUrl, supabaseKey);
                supabaseClient.InitializeAsync().Wait(); // Ensure initialization

                return supabaseClient;
            });

            // Register repositories, AutoMapper, etc.
            // Example: services.AddScoped<IExpenseRepository, ExpenseRepository>();

            return services;
        }
    }
}

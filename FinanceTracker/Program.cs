using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Mappers; // Add this

namespace FinanceTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Add services to the container.
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration); var app = builder.Build();
            
            builder.Services.AddAutoMapper(cfg => { /* optional config */ },
                typeof(MappingProfileExpenses).Assembly,
                typeof(MappingProfilePaymentMethod).Assembly
                /* etc. */
            );
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

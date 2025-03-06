using FinanceTracker.Infrastructure; // Add this

namespace FinanceTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Add services to the container.
            var builder = WebApplication.CreateBuilder(args);

            // Supabase Configuration
            var supabaseUrl = builder.Configuration["Supabase:Url"];
            var supabaseKey = builder.Configuration["Supabase:Key"];

            // Register Supabase Client
            builder.Services.AddSingleton(_ => new Supabase.Client(supabaseUrl, supabaseKey));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration); var app = builder.Build();

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

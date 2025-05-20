
using HelperContainer.JsonConvert;
using Microsoft.Extensions.Options;
using Service;
using Service.IService;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bank_Attemp_Final
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Enum as string (e.g., "Male", not 0)
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));

                // DateTime parsing in ISO 8601 (default)
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

                // Decimal handling with precise format
                options.JsonSerializerOptions.Converters.Add(new JsonConverterForDecimal());

                // Optional: Customize DateTime globally
                options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ssZ")); // adjust as needed
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
           
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<ITransferService, TransferService>();

            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
                app.UseSwagger();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

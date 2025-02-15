using Shortify.Api.Extensions;
namespace Shortify.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Host.ConfigureSerilog();

        builder.Services.AddServices(builder.Configuration);

        var app = builder.Build();

        app.UseAppDefaults();
    }
}

using Streambit.Catalog.Api.Extensions;

namespace Streambit.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices(typeof(Program));
            var app = builder.Build();

            app.RegisterPipelineComponents(typeof(Program));

            app.Run();

            // Add services to the container.

            //builder.Services.AddControllers();

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            //app.Run();
        }
    }
}

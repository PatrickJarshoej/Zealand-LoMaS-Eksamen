using Zealand_LoMaS_Lib.Repo;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;

namespace Zealand_LoMaS_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Singleton for TransportRepo
            builder.Services.AddSingleton<ITransportRepo, TransportRepo>();
            builder.Services.AddSingleton<TransportService>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}

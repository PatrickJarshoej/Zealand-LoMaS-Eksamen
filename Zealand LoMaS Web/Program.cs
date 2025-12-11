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

            //Singleton for TransportRepo
            builder.Services.AddSingleton<IInstitutionRelationRepo, InstitutionRelationRepo>();
            builder.Services.AddSingleton<InstitutionRelationService>();

            //Singleton for AdminRepo
            builder.Services.AddSingleton<IAdminRepo, AdminRepo>();
            builder.Services.AddSingleton<AdminService>();

            //Singleton for TeacherRepo
            builder.Services.AddSingleton<ITeacherRepo, TeacherRepo>();
            builder.Services.AddSingleton<TeacherService>();

            //Singleton for InstitutionRepo
            builder.Services.AddSingleton<IInstitutionRepo, InstitutionRepo>();
            builder.Services.AddSingleton<InstitutionService>();

            //Singleton for AClassRepo
            builder.Services.AddSingleton<IAClassRepo, AClassRepo>();
            builder.Services.AddSingleton<AClassService>();

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

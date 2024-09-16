using ClinicalManagementSystem_Team5.Repository;
using ClinicalManagementSystem_Team5.Service;

namespace ClinicalManagementSystem_Team5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the connection string as a singleton.
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMvcwin");
            builder.Services.AddSingleton(connectionString);

            // Register repositories and services
            builder.Services.AddScoped<IPatientRepository, PatientRepositoryImpl>();
            builder.Services.AddScoped<IPatientService, PatientServiceImpl>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepositoryImpl>(); // Register IDoctorRepository
            builder.Services.AddScoped<IDoctorService, DoctorServiceImpl>(); // Register IDoctorService

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Patient}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

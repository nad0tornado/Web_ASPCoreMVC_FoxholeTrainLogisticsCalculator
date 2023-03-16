using FoxholeTrainLogistics.Contexts;
using FoxholeTrainLogistics.Controllers;
using FoxholeTrainLogistics.Interfaces;
using FoxholeTrainLogistics.Models;
using FoxholeTrainLogistics.Trains;

namespace FoxholeTrainLogistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // .. Setup DB Context
            builder.Services.AddSingleton<ITrainsDbContext, TrainsInMemoryContext>();

            var app = builder.Build();

            SetupDBDummyData(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void SetupDBDummyData(WebApplication app)
        {
            var dbContext = app.Services.GetRequiredService<ITrainsDbContext>();

            var rand = new Random();
            for (int i = 0; i < 10; ++i)
            {
                var trainCars = new List<TrainCar>()
                {
                    TrainCar.Engine,
                    TrainCar.CoalCar,
                    TrainCar.InfantryCar,
                };

                for (int j = 0; j < 10; ++j)
                    trainCars.Add(TrainCar.FlatbedCar);

                trainCars.Add(TrainCar.Caboose);

                dbContext.Trains.Add(new Train()
                {
                    TrainId = new Guid(),
                    Cars = trainCars,
                    Status = (TrainStatus)rand.Next(0, 3)
                });
            }

            dbContext.SaveChanges();
        }
    }
}
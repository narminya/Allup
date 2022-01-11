using Allup.Models.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models.DataAccessLayer
{
    static public class AppDataSeed
    {
        static  public IApplicationBuilder Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

               // var role = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

              //  db.Database.Migrate();
                 InitCategories(db);


            }
            return app;
        }

        private static void InitCategories(AppDbContext db)
        {
            if (!db.categories.Any())
            {
                db.categories.AddRange(
                    new Category
                    {
                        Name = "Winter",
                        Image = "category-1.jpg",
                        IsMain = true
                    },
                     new Category
                     {
                         Name = "Winter",
                         Image = "category-1.jpg",
                         IsMain = true
                     },
                      new Category
                      {
                          Name = "Winter",
                          Image = "category-1.jpg",
                          IsMain = true
                      }
                    );
            }
            db.SaveChanges();
        }
    }
}

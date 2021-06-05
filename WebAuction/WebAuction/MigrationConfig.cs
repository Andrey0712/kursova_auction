using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuction.Entities;

namespace WebAuction
{
    public static class MigrationConfig
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                //var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var context = serviceScope.ServiceProvider.GetRequiredService<MyContext>();

                

                if (!context.Lot.Any())
                {
                    var lots = new List<Lot>()
                    {
                        new Lot
                        {
                            Name = "Монета",
                            Prise=2000,
                            Description = "1 рубль 1870 г выпуска, в идеальном состоянии",
                            Begin=DateTime.Now,
                            End=DateTime.Now.AddDays(2),
                            Image="1.jpg"
                        },
                        new Lot
                        {
                            Name = "Картина. 9-й вал.",
                            Prise=1000,
                            Description = "Копия картины Айвазовского 9-й вал",
                            Begin=DateTime.Now,
                            End=DateTime.Now.AddDays(3),
                            Image="Ayvazobsky.jpg"
                        },
                        new Lot
                        {
                            Name = "Кубок",
                            Prise=3000,
                            Description = "Серебряный кубок, 18 век",
                            Begin=DateTime.Now,
                            End=DateTime.Now.AddDays(5),
                            Image="Cup.jpg"
                        }
                    };
                    context.Lot.AddRange(lots);
                    context.SaveChanges();
                }
                if (context.Users.Count() == 0)
                {
                    context.Users
                        .Add(new User
                        {
                            Name = "Василиса",
                            Surname = "Зуева",
                            Login = "Шопоголик",
                            Password="шоп1"
                        });
                    context.Users
                        .Add(new User
                        {
                            Name = "Кличко",
                            Surname = "Виталик",
                            Login = "Бокс",
                            Password = "шоп2"
                        });

                    context.SaveChanges();
                }
                if (context.UserLot.Count() == 0)
                {
                    context.UserLot
                        .Add(new UserLot
                        {
                            UserId = 1,
                            LotId = 1,
                        });
                    context.UserLot
                        .Add(new UserLot
                        {
                            UserId = 1,
                            LotId = 3,
                        });
                    context.UserLot
                        .Add(new UserLot
                        {
                            UserId = 2,
                            LotId = 2,
                        });
                    context.UserLot
                        .Add(new UserLot
                        {
                            UserId = 2,
                            LotId = 3,
                        });

                    context.SaveChanges();
                }

            }
        }
    }
}


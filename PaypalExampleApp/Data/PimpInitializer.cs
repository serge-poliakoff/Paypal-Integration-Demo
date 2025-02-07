using PaypalExampleApp.DbContexts;
using PaypalExampleApp.Models;

namespace PaypalExampleApp.Data;

public static class PimpDataInitializer
{
    public static WebApplication SeedData(this WebApplication app)
    {
        List<PuteModel> content = new List<PuteModel>()
        {
            new PuteModel()
            {
                Name = "Caroline",
                PriceHour = 169,
                Description = "Orgies, Shoenberg music, violon concertos. Keen striptiseur." +
                    " Some milght erotic dreams you had or anything stronger with a lofty thin brunette"
            },
            new PuteModel()
            {
                Name = "Angelica",
                PriceHour = 129,
                Description = "Lovely long-haired blonde up to make any of your wishes come true" +
                    "A combination of experience and sensetivity won't leave anyone unsurprised"
            },
            new PuteModel()
            {
                Name = "Medusa",
                PriceHour = 219,
                Description = "A deadly regarde, making everything turn to sculpture, unhuman power" +
                    " and soft pale skin. Spend one of your most beautiful nights with a former Poseidon lover"
            }
        };
        using (var scope = app.Services.CreateScope())
        {
            using (var db = scope.ServiceProvider.GetRequiredService<PimpDbContext>())
            {
                db.Database.EnsureCreated();
                foreach (var model in content)
                {
                    var modelInDb = db.putes.Where(m => m.Name == model.Name).FirstOrDefault();
                    if(modelInDb == null)
                    {
                        db.Add(model);
                        db.SaveChanges();
                    }else
                    {
                        model.Id = modelInDb.Id;
                        if (modelInDb != model)
                        {
                            modelInDb.Name = model.Name;
                            modelInDb.PriceHour = model.PriceHour;
                            modelInDb.Description = model.Description;
                            db.Update(modelInDb);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
        return app;
    }
}


using TestApplication.Domain.Entities;

namespace TestApplication.Infrastructure
{

    public class DbInitializer
    {
        public static void Initialize(DataItemDbContext context)
        {
            if (!context.DataItems.Any())
            {

                context.DataItems.Add(new DataItem()
                {
                    Data = "{\"name\":\"Alice\",\"age\":25}"
                });


                context.DataItems.Add(new DataItem()
                {
                    Data = "{\"name\":\"Bob\",\"age\":30}"
                });

                context.DataItems.Add(new DataItem()
                {
                    Data = "{\"name\":\"Charlie\",\"age\":35}"
                });

                context.SaveChanges();

            }
        }
    }
}

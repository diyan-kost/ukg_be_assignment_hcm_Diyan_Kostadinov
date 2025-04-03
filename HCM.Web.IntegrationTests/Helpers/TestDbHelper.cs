using HCM.Core.Helpers;
using HCM.Infrastructure;
using HCM.Infrastructure.Entities;

namespace HCM.Web.IntegrationTests.Helpers
{
    public class TestDbHelper
    {
        public static string GetValidUsername(HCMContext db)
        {
            return db.Users.First().Username;
        }
    }
}

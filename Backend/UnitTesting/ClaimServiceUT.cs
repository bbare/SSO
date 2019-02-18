using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTesting
{
    [TestClass]
    public class ClaimServiceUT
    {
        ClaimService claimService;
        User user1;
        User user2;
        Service service1;
        Service service2;
        Claim claim1;

        public ClaimServiceUT()
        {
            var testUtils = new TestingUtils();

            claimService = new ClaimService();

            user1 = testUtils.CreateUserInDb();
            user2 = testUtils.CreateUserInDb();

            service1 = testUtils.CreateServiceInDb(true);
            service2 = testUtils.CreateServiceInDb(true);

            

            claim1 = testUtils.CreateClaim(user1, service1, user2);
        }

        [TestMethod]
        public void CreateClaim()
        {
            
            // ACT
            int response = claimService.CreateClaim(user1.Id, service1.Id);

            using (var _db = new DatabaseContext()) {
                

                Claim recentclaim = _db.Claims
                    .Where(c => c.UserId == user1.Id && c.ServiceId == service1.Id)
                    .FirstOrDefault();
                Assert.IsTrue(recentclaim!=null);
            }

               
                
                Assert.IsTrue(response > 0);
        }

        [TestMethod]
        public void getService()
        {
            Service received = claimService.GetService(service1.ServiceName);

            StringAssert.Contains(received.ServiceName, service1.ServiceName);
        }

        [TestMethod]
        public void addServiceToUser()
        {
            claimService.AddServiceToUser(user2, service2);

            using (var _db = new DatabaseContext())
            {
                int count = _db.Claims
                    .Where(c => c.UserId == user2.Id && c.ServiceId == service2.Id)
                    .Count();
                
                Assert.IsTrue(count > 0);
            }
        }

        [TestMethod]
        public void userHasServiceAccess()
        {
            bool hasAccess = claimService.UserHasServiceAccess(user1, service1);

            Assert.IsTrue(hasAccess);
        }
    }
}

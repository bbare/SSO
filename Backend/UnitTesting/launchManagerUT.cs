using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer.Models;
using ManagerLayer.LaunchManagement;
using DataAccessLayer.Database;

namespace UnitTesting
{
    [TestClass]
    public class LaunchManagerUT
    {
        TestingUtils _tu;
        LaunchManager _lm;
        User newUser;
        Application newApp;
        Session newSession;
        LaunchPayload testPayload;

        public LaunchManagerUT()
        {
            _tu = new TestingUtils();
            _lm = new LaunchManager();

            newUser = _tu.CreateUserInDb();
            newApp = _tu.CreateApplicationInDb();
            newSession = _tu.CreateSessionObject(newUser);
            _tu.CreateSessionInDb(newSession);

            testPayload = new LaunchPayload()
            {
                ssoUserId = newSession.UserId,
                email = newUser.Email,
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };
        }

        [TestMethod]
        public void validateSignature()
        {
            //Verifies given identical data will produce same hash
            Assert.AreEqual(_lm.generateSignature(testPayload, newApp), _lm.generateSignature(testPayload, newApp));
        }

        [TestMethod]
        public void validSignLaunch()
        {
            using (var _db = new DatabaseContext())
            {
                LaunchResponse lr = _lm.SignLaunch(_db, newSession, newApp.Id);

                Assert.AreEqual(lr.url, newApp.LaunchUrl);
                Assert.IsNotNull(lr.launchPayload.signature);
            }
        }

        [TestMethod]
        public void invalidSessionLaunch()
        {
            Session nonExistingSession = _tu.CreateSessionObject(newUser);

            using (var _db = new DatabaseContext())
            {
                LaunchResponse lr = _lm.SignLaunch(_db, nonExistingSession, newApp.Id);

                //TODO Compare response for invalid session
            }
        }
    }
}

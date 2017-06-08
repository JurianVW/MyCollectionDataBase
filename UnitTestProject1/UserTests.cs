using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository.Data;
using Repository.Logic;

namespace UnitTests
{
    [TestClass]
    public class UserTests
    {
        private UserRepository userRepository = new UserRepository(new UserMemoryContext());

        [TestMethod]
        public void TestLogin()
        {
            User user1 = new User { Username = "Tester1", Password = "Test1" };
            User user2 = new User { Username = "Tester2", Password = "Test1" };

            Assert.AreEqual(true, userRepository.CheckUserLogin(user1));
            Assert.AreEqual(false, userRepository.CheckUserLogin(user2));
        }
    }
}
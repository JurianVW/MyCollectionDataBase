using System;
using System.Collections.Generic;
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
        public void TestCheckUserLogin()
        {
            User user1 = new User { Username = "Tester1", Password = "Test1", EmailAddress = "Tester1@home.com" };
            User user2 = new User { Username = "Tester2", Password = "Test1", EmailAddress = "Tester2@home.com" };

            Assert.AreEqual(true, userRepository.CheckUserLogin(user1));
            Assert.AreEqual(false, userRepository.CheckUserLogin(user2));
        }

        [TestMethod]
        public void TestCheckUsernameAvailability()
        {
            User user1 = new User { Username = "Tester1", Password = "Test1", EmailAddress = "Tester1@home.com" };
            User user2 = new User { Username = "Tester2", Password = "Test2", EmailAddress = "Tester2@home.com" };

            Assert.AreEqual(true, userRepository.CheckUsernameAvailability(user1));
            Assert.AreEqual(false, userRepository.CheckUsernameAvailability(user2));
        }

        [TestMethod]
        public void TestEmailAvailability()
        {
            User user1 = new User { Username = "Tester1", Password = "Test1", EmailAddress = "Tester1@home.com" };
            User user2 = new User { Username = "Tester2", Password = "Test2", EmailAddress = "Tester2@home.com" };

            Assert.AreEqual(true, userRepository.CheckUsernameAvailability(user1));
            Assert.AreEqual(false, userRepository.CheckUsernameAvailability(user2));
        }

        [TestMethod]
        public void TestGetUsernames()
        {
            List<string> usernames = new List<string>();
            Assert.AreEqual(0, usernames.Count);
            usernames = userRepository.GetUsernames();
            Assert.AreEqual(4, usernames.Count);
        }
    }
}
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository.Data;
using Repository.Logic;

namespace UnitTests
{
    [TestClass]
    public class CollectionTests
    {
        private CollectionRepository collectionRepository = new CollectionRepository(new CollectionMemoryContext());

        [TestMethod]
        public void TestCheckMyList()
        {
            Assert.AreEqual(true, collectionRepository.CheckMyList(1, 1));
            Assert.AreEqual(false, collectionRepository.CheckMyList(1, 2));
        }
    }
}
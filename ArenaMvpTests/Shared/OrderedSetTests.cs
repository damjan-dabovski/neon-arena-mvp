namespace ArenaMvpTests.Shared
{
    using NeonArenaMvp.Shared;

    [TestClass]
    public class OrderedSetTests
    {
        private readonly OrderedSet<int> orderedSet;

        public OrderedSetTests()
        {
            orderedSet = new();
        }

        [TestMethod]
        public void AddsMultipleItems()
        {
            // Act
            orderedSet.Add(1);
            orderedSet.Add(2);

            // Assert
            Assert.AreEqual(2, orderedSet.Count);
        }

        [TestMethod]
        public void DoesntAddAnExistingItem()
        {
            // Arrange
            orderedSet.Add(1);

            // Act
            var isAddSuccessful = orderedSet.Add(1);

            // Assert
            Assert.IsFalse(isAddSuccessful);
            Assert.AreEqual(1, orderedSet.Count);
        }

        [TestMethod]
        public void PreservesOrderWhenAdding()
        {
            // Act
            orderedSet.Add(1);
            orderedSet.Add(2);
            orderedSet.Add(3);

            // Assert
            var expectedOrder = new int[] { 1, 2, 3 };
            var isOrderPreserved = true;
            var loopIndex = 0;

            foreach (var item in orderedSet)
            {
                if (item != expectedOrder[loopIndex])
                {
                    isOrderPreserved = false;
                }
                loopIndex++;
            }

            Assert.IsTrue(isOrderPreserved);
        }

        [TestMethod]
        public void RemovesAnItem()
        {
            // Arrange
            orderedSet.Add(1);
            orderedSet.Add(2);

            // Act
            var isRemoveSuccessful = orderedSet.Remove(2);

            // Assert
            Assert.IsTrue(isRemoveSuccessful);
            Assert.AreEqual(1, orderedSet.Count);
            Assert.IsFalse(orderedSet.Any(x => x == 2));
        }

        [TestMethod]
        public void DoesntRemoveAnItemThatDoesntExist()
        {
            // Arrange
            orderedSet.Add(1);

            // Act
            var isRemoveSuccessful = orderedSet.Remove(2);

            // Assert
            Assert.IsFalse(isRemoveSuccessful);
            Assert.AreEqual(1, orderedSet.Count);
        }

        [TestMethod]
        public void PreservesOrderWhenRemoving()
        {
            // Arrange
            orderedSet.Add(1);
            orderedSet.Add(2);
            orderedSet.Add(3);

            // Act
            orderedSet.Remove(2);

            // Assert
            var expectedOrder = new int[] { 1, 3 };
            var isOrderPreserved = true;
            var loopIndex = 0;

            foreach (var item in orderedSet)
            {
                if (item != expectedOrder[loopIndex])
                {
                    isOrderPreserved = false;
                }
                loopIndex++;
            }

            Assert.IsTrue(isOrderPreserved);
        }
    }
}

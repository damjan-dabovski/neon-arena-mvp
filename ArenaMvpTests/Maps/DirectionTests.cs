namespace ArenaMvpTests.Maps
{
    using NeonArenaMvp.Game.Maps;
    using static NeonArenaMvp.Game.Maps.Enums;

    [TestClass]
    public class DirectionTests
    {
        [TestMethod]
        [DataRow(Direction.Up, Direction.Right)]
        [DataRow(Direction.Right, Direction.Down)]
        [DataRow(Direction.Down, Direction.Left)]
        [DataRow(Direction.Left, Direction.Up)]
        public void RelativeRightTest(Direction original, Direction expected)
        {
            // Act
            var newDirection = original.RelativeRight();

            // Assert
            Assert.AreEqual(newDirection, expected);
        }

        [TestMethod]
        [DataRow(Direction.Up, Direction.Left)]
        [DataRow(Direction.Right, Direction.Up)]
        [DataRow(Direction.Down, Direction.Right)]
        [DataRow(Direction.Left, Direction.Down)]
        public void RelativeLeftTest(Direction original, Direction expected)
        {
            // Act
            var newDirection = original.RelativeLeft();

            // Assert
            Assert.AreEqual(newDirection, expected);
        }

        [TestMethod]
        [DataRow(Direction.Up, Direction.Down)]
        [DataRow(Direction.Right, Direction.Left)]
        [DataRow(Direction.Down, Direction.Up)]
        [DataRow(Direction.Left, Direction.Right)]
        public void ReverseTest(Direction original, Direction expected)
        {
            // Act
            var newDirection = original.Reverse();

            // Assert
            Assert.AreEqual(newDirection, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsExceptionWhenRelativeRightDirectionInvalid()
        {
            // Act & Assert
            ((Direction)10).RelativeRight();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsExceptionWhenRelativeLeftDirectionInvalid()
        {
            // Act & Assert
            ((Direction)10).RelativeLeft();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowsExceptionWhenReverseDirectionInvalid()
        {
            // Act & Assert
            ((Direction)10).Reverse();
        }
    }
}
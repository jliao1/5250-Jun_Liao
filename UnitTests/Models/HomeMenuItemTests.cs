using Mine.Models;
using NUnit.Framework;

namespace UnitTests.Models
{
    [TestFixture]
    public class HomeMenuItemTests
    {
        [Test]
        public void HomeMenuItem_Constructor_Valid_Default_Should_Pass()
        {
            // Arrange

            // Act
            var result = new HomeMenuItem();

            // Reset

            // Assert 
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItemModel_Set_Get_Valid_Default_Should_Pass()
        {
            // Arrange

            // Act
            var result = new HomeMenuItem();
            result.Id = MenuItemType.About;
            result.Title = "Text";

            // Reset

            // Assert 

            Assert.AreEqual(MenuItemType.About, result.Id);
            Assert.AreEqual("Text", result.Title);

        }
    }

}

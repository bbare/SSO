using System;
using ManagerLayer.ApplicationManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class ApplicationManagerUT
    {
        ApplicationManager am;

        public ApplicationManagerUT()
        {
            am = new ApplicationManager();
        }

        [TestMethod]
        public void IsValidTitle_Pass_ReturnTrue()
        {
            // Arrange
            string title = "Good Title";

            // Act
            var actual = am.IsValidTitle(title);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidTitle_Fail_ReturnFalse()
        {
            // Arrange
            string title = "";
            for(int i = 0; i < 102; i++)
            {
                title += "a";
            }

            // Act
            var actual = am.IsValidTitle(title);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidTitle_Fail_NullValueReturnsFalse()
        {
            // Act
            var actual = am.IsValidTitle(null);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidEmail_Pass_ReturnTrue()
        {
            // Arrange
            string email = "sso@email.com";

            // Act
            var actual = am.IsValidEmail(email);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidEmail_Fail_ReturnFalse()
        {
            // Arrange
            string email = "email";

            // Act
            var actual = am.IsValidEmail(email);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidEmail_Fail_NullValuesReturnFalse()
        {
            // Act
            var actual = am.IsValidEmail(null);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidUrl_Pass_ReturnTrue()
        {
            // Arrange
            string url = "https://example.com";
            Uri urlResult = null;

            // Act
            var actual = am.IsValidUrl(url, ref urlResult);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsNotNull(urlResult);
        }

        [TestMethod]
        public void IsValidUrl_Fail_ReturnFalse()
        {
            // Arrange
            string url = "url";
            Uri urlResult = null;

            // Act
            var actual = am.IsValidUrl(url, ref urlResult);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsNull(urlResult);
        }

        [TestMethod]
        public void IsValidUrl_Fail_NullValuesReturnFalse()
        {
            Uri url = null;

            // Act
            var actual = am.IsValidUrl(null, ref url);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidDescription_Pass_ReturnTrue()
        {
            // Arrange
            string description = "description";

            // Act
            var actual = am.IsValidDescription(description);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidDescription_Fail_ReturnFalse()
        {
            // Arrange
            string description = "";
            for (int i = 0; i < 2002; i++)
            {
                description += "a";
            }

            // Act
            var actual = am.IsValidDescription(description);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidDescription_Fail_NullValuesReturnFalse()
        {
            // Act
            var actual = am.IsValidDescription(null);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidImageExtension_Pass_ReturnTrue()
        {
            // Arrange
            Uri image = new Uri("https://example.com/image.png");

            // Act
            var actual = am.IsValidImageExtension(image);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidImageExtenxion_Fail_ReturnFalse()
        {
            // Arrange
            Uri image = new Uri("https://example.com/image.jpg");

            // Act
            var actual = am.IsValidImageExtension(image);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidImageExtension_Fail_NullValuesReturnFalse()
        {
            // Act
            var actual = am.IsValidImageExtension(null);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidDimensions_Pass_ReturnTrue()
        {
            // Arrange
            Uri image = new Uri("http://icons.iconarchive.com/icons/treetog/i/48/Image-File-icon.png");

            // Act
            var actual = am.IsValidDimensions(image);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidDimensions_Fail_ReturnFalse()
        {
            // Arrange
            Uri image = new Uri("https://www.color-hex.com/palettes/16812.png");

            // Act
            var actual = am.IsValidDimensions(image);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidDimensions_Fail_NullValuesReturnFalse()
        {
            // Act
            var actual = am.IsValidDimensions(null);

            // Assert
            Assert.IsFalse(actual);
        }
    }
}

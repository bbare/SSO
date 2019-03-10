using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTesting
{
    [TestClass]
    class EmailServiceUT
    {
        EmailService es;
        public EmailServiceUT()
        {
            es = new EmailService();
        }

        [TestMethod]
        public void createMessage_Pass()
        {
            //Arrange
            string ExpectedReceiverName = "Bob";
            string ExpectedReceiverEmail = "Bob@website.com";
            string ExpectedEmailSubject = "Message Test";
            string ExpectedEmailBody = "This is a test body";

            string ActualReceiverName = "Bob";
            string ActualReceiverEmail = "Bob@website.com";
            string ActualEmailSubject = "Message Test";
            string ActualEmailBody = "This is a test body";
            //Act
            var expected = es.createEmailPlainBody(ExpectedReceiverName, ExpectedReceiverEmail, ExpectedEmailSubject, ExpectedEmailBody);
            var actual = es.createEmailPlainBody(ActualReceiverName, ActualReceiverEmail, ActualEmailSubject, ActualEmailBody);
            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void createMessage_Fail()
        {
            //Arrange
            string ExpectedReceiverName = "Bob";
            string ExpectedReceiverEmail = "Bob@website.com";
            string ExpectedEmailSubject = "Message Test";
            string ExpectedEmailBody = "This is a test body";

            string ActualReceiverName = "Alice";
            string ActualReceiverEmail = "Alice@website.com";
            string ActualEmailSubject = "Message Testz";
            string ActualEmailBody = "This is a test bodyz";
            //Act
            var expected = es.createEmailPlainBody(ExpectedReceiverName, ExpectedReceiverEmail, ExpectedEmailSubject, ExpectedEmailBody);
            var actual = es.createEmailPlainBody(ActualReceiverName, ActualReceiverEmail, ActualEmailSubject, ActualEmailBody);
            //Assert
            Assert.AreNotEqual(actual, expected);
        }
    }
}

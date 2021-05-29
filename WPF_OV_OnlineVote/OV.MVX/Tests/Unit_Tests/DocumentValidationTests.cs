using FluentAssertions;
using OV.MVX.Helpers;
using Xunit;

namespace OV.MVX.Tests.Unit_Tests
{
    public class DocumentValidationTests
    {
        [Theory]
        [InlineData("12345678Z", true)]
        [InlineData("12345678F", false)]
        [InlineData("87654321X", true)]
        [InlineData("87654321a", false)]
        [InlineData("876543211", false)]
        public void ShouldReturnTrueIfDNI_IsCorrect(string DNI, bool isCorrectDNI)
        {
            //Arrange
            
            //Act
            var result = DocumentValidation.isValidDocument(DNI);
            
            //Assert
            result.Should().Be(isCorrectDNI);
        }

        [Theory]
        [InlineData("X1234567-L", true)]
        [InlineData("X1234567L", false)]
        [InlineData("X1234567A", false)]
        [InlineData("1234567L", false)]
        [InlineData("G1234567L", false)]
        [InlineData("Y7654321G", false)]
        [InlineData("Y7654321-G", true)]
        [InlineData("7654321G", false)]
        [InlineData("Z7654321-H", true)]
        [InlineData("Z7654321-G", false)]
        [InlineData("Z7654321H", false)]
        [InlineData("Y7654321-H", false)]
        [InlineData("7654321H", false)]
        public void ShouldReturnTrueIfNIE_IsCorrect(string NIE, bool isCorrectNIE)
        {
            //Arrange
            
            //Act
            var result = DocumentValidation.isValidDocument(NIE);
            
            //Assert
            result.Should().Be(isCorrectNIE);
        }

        [Theory]
        [InlineData("X1234567-L", "NIE")]
        [InlineData("Y7654321-G", "NIE")]
        [InlineData("12345678Z", "DNI")]
        [InlineData("87654321X", "DNI")]
        [InlineData("X1234567L", "Unknow")]
        [InlineData("1234567L", "Unknow")]
        [InlineData("G1234567L", "Unknow")]
        public void ShouldReturnTypeOfDocument(string documentNumber, string expectedDocumentType)
        {
            //Arrange
            
            //Act
            var result = DocumentValidation.GetDocumentType(documentNumber);
            
            //Assert
            result.Should().Be(expectedDocumentType);
        }
    }
}

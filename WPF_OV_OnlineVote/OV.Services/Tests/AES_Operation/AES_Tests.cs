using FluentAssertions;
using OV.Services.AES_Operation;
using Xunit;

namespace OV.Services.Tests.AES_Operation
{
    public class AES_Tests
    {
        public class AES_Encrypt_Decrypt
        {
            [Fact]
            public void ShouldEncryptString()
            {
                //Arrange
                var key = "&F)J@NcRfUjXn2r5u8x/A?D(G-KaPdSg";
                var stringToEncrypt = "very_Important_Password123";

                //Act
                var encryptedString = AES.EncryptString(key, stringToEncrypt, AES_IV_MODES.InitializationVectorEnable);

                //Assert
                encryptedString.Should().NotBeNullOrEmpty();
                encryptedString.Should().NotBe(stringToEncrypt);
            }

            [Fact]
            public void ShouldDecryptString()
            {
                //Arrange
                var key = "&F)J@NcRfUjXn2r5u8x/A?D(G-KaPdSg";
                var stringToDecrypt = "+dSgz0aEuKnQJQBC844i/e9II0qS8kYbcvvUGF02YTo=";
                var expectedString = "very_Important_Password123";

                //Act
                var decryptedString = AES.DecryptString(key, stringToDecrypt, AES_IV_MODES.InitializationVectorEnable);

                //Assert
                decryptedString.Should().Be(expectedString);
            }
        }
    }
}

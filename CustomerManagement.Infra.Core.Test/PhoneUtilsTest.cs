using CustomerManagement.Infra.Core.Utils;

namespace CustomerManagement.Infra.Core.Test
{
    public class PhoneUtilsTest
    {
        [Theory]
        [InlineData("4134567890", "41", "34567890")]
        [InlineData("(41)3456-7890", "41", "34567890")]
        [InlineData("0(41)3456-7890", "41", "34567890")]
        [InlineData("(041)3456-7890", "41", "34567890")]
        [InlineData("04134567890", "41", "34567890")]
        [InlineData("001234567890", "01", "234567890")]
        [InlineData("123-456-7890", "12", "34567890")]
        [InlineData("00123-456-7890", "01", "234567890")]
        public void PhoneUtils_Should_Parse_Phone_And_Ddd(string phoneNumber, string expectedDdd, string expectedNumber)
        {
            PhoneUtils.GetDddAndNumber(phoneNumber, out var ddd, out var number);

            Assert.Equal(ddd, expectedDdd);
            Assert.Equal(number, expectedNumber);
        }
    }
}
using System.Text.RegularExpressions;

namespace CustomerManagement.Infra.Core.Utils
{
    public class PhoneUtils
    {
        public static void GetDddAndNumber(string phoneNumber, out string ddd, out string number)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            // Remove non-numeric characters from number
            string plainNumber = Regex.Replace(phoneNumber, "[^0-9]", "");

            if (plainNumber.StartsWith('0'))
            {
                plainNumber = plainNumber.Substring(1);
            }
            ddd = plainNumber.Substring(0, 2);
            number = plainNumber.Substring(2);
        }
    }
}

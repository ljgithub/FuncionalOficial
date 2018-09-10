using System;
using System.Text.RegularExpressions;

namespace FarmaEnlace.Helpers
{
    public static class RegexUtilities
	{
		public static bool IsValidEmail(string email)
		{
			var expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
			if (Regex.IsMatch(email, expresion))
			{
				if (Regex.Replace(email, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

        public static bool IsValidPassport(string passport)
        {
            var expresion = "^(?!^0+$)[a-zA-Z0-9]{3,20}$";
            if (Regex.IsMatch(passport, expresion))
            {
                if (Regex.Replace(passport, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool IsValidCellPhone(string cellPhone)
        {
            var expresion = "[0-9]{10}$";
            if (Regex.IsMatch(cellPhone, expresion))
            {
                if (Regex.Replace(cellPhone, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}

using FarmaEnlace.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FarmaEnlace.Helpers
{
    public static class Common
    {
        public static string RemoveAccentsWithNormalization(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return inputString;

            string normalizedString = inputString.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}

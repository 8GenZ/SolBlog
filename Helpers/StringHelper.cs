﻿using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SolBlog.Helpers
{
    public static class StringHelper
    {
        public static string BlogPostSlug(string? title)
        {
            string? output = RemoveAccents(title).ToLower();

            //remove special characters
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            //remove all additional spaces on favour of just one
            output = Regex.Replace(output, @"\s+", " ");

            //replave all spaces with the   hyphen
            output = Regex.Replace(output, @"\s", "-");

            return output;
        }

        private static string RemoveAccents(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return title!;
            }

            // Convert for Unicode
            title = title.Normalize(NormalizationForm.FormD);
            // Format unicode/ascii
            char[] chars = title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            // Convert and return the new title
            return new string(chars).Normalize(NormalizationForm.FormC);

        }
    }
}

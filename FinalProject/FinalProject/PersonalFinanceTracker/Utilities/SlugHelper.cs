namespace PersonalFinanceTracker.Utilities
{
    public static class SlugHelper
    {
        public static string ToKebab(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim().ToLowerInvariant();

            var slug = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9]+", "-");

            slug = slug.Trim('-');

            return slug;
        }
    }
}

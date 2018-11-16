namespace ExceptionLess.WebHook.DingTalk.Utilities
{
    public class StringUtility
    {
        public static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var first = str.Substring(0, 1);
            var last = str.Substring(1);
            return first.ToLower() + last;
        }
    }
}
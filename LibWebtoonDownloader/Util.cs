namespace LibWebtoonDownloader
{
    internal static class Util
    {
        internal static string ReplaceSpecialCharacter(this string str)
        {
            return str.Replace("\\", "＼")
                .Replace("/", "／")
                .Replace(":", "：")
                .Replace("*", "＊")
                .Replace("?", "？")
                .Replace("\"", "＂")
                .Replace("<", "＜")
                .Replace(">", "＞")
                .Replace("|", "｜");
        }
    }
}
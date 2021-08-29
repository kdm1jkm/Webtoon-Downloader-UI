namespace LibWebtoonDownloader
{
    public static class Util
    {
        public static string ReplaceSpecialCharacter(this string str)
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
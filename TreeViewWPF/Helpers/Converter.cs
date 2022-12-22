using System;

namespace TreeViewWPF.Helpers
{
    public class Converter
    {
        public static string GetFileFolderName(string path)
        {
            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }


        public static string ConvertBytes(long size)
        {
            switch (size)
            {
                case long s when s > 1000000 && s < 100000000:
                    return string.Format("{0:N0}", Convert.ToInt64(size) >> 10) + " KB";
                    break;
                case long s when s >= 100000000 && s < 10000000000:
                    return string.Format("{0:N0}", Convert.ToInt64(size) >> 20) + " MB";
                    break;
                case long s when s >= 10000000000:
                    return string.Format("{0:N0}", Convert.ToInt64(size) >> 30) + " GB";
                    break;
            }
            
            return string.Format("{0:N0}", size) +" B";
        }
    }
}

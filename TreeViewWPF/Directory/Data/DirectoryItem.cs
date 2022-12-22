
namespace TreeViewWPF.Directory.Data
{
    // Information about a directory item such as a drive, a file or a folder
    public class DirectoryItem
    {
        public DirectoryItemType Type { get; set; }
        public string FullPath { get; set; }
        public string Size { get; set; }

    }
}

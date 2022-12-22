using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TreeViewWPF.Directory.Data;
using TreeViewWPF.Directory.ViewModels.Base;
using TreeViewWPF.Helpers;

namespace TreeViewWPF.Directory.ViewModels
{
    public class DirectoryItemViewModel : BaseViewModel
    {
        public DirectoryItemType Type { get; set; }
        public string ImageName => Type == DirectoryItemType.Drive ? "drive" : (Type == DirectoryItemType.File ? "file" : (IsExpanded ? "open_folder" : "closed_folder"));
        public string FullPath { get; set; }
        public string Size { get; set; }
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : Converter.GetFileFolderName(this.FullPath); } }
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }
        public ICommand ExpandCommand { get; set; }

        public bool IsExpanded
        {
            get
            {
                return this.Children?.Count(f => f != null) > 0;
            }
            set
            {
                // If the UI tells us to expand...
                if (value == true)
                    // Find all children
                    Expand();
                // If the UI tells us to close
                else
                    this.ClearChildren();
            }
        }


        public DirectoryItemViewModel(string fullPath, DirectoryItemType type, string size)
        {
            // Create commands
            this.ExpandCommand = new RelayCommand(Expand);
           

            // Set path and type
            this.FullPath = fullPath;
            this.Type = type;
            this.Size = size;

            // Setup the children as needed
            this.ClearChildren();
        }


        private void ClearChildren()
        {
            // Clear items
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            // Show the expand arrow if we are not a file
            if (this.Type != DirectoryItemType.File)
                this.Children.Add(null);
        }


        private async void Expand()
        {
            // We cannot expand a file
            if (this.Type == DirectoryItemType.File)
                return;

            var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
            Children = new ObservableCollection<DirectoryItemViewModel>(
                                children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type, content.Size)));
            await DirectoryStructure.UpdateSize(Children);
        }
    }
}
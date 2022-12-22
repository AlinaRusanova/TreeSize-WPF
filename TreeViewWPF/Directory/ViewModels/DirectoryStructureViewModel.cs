using System.Collections.ObjectModel;
using System.Linq;
using TreeViewWPF.Directory.Data;
using TreeViewWPF.Directory.ViewModels.Base;

namespace TreeViewWPF.Directory.ViewModels
{
    public class DirectoryStructureViewModel : BaseViewModel
    {
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        public DirectoryStructureViewModel()
        {
            // Get the logical drives
                var children = DirectoryStructure.GetLogicalDrives();
                

            // Create the view models from the data
                this.Items = new ObservableCollection<DirectoryItemViewModel>( 
                children.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, drive.Size)));
        }
    }
}
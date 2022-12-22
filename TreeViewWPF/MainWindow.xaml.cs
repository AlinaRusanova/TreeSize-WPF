using System.Windows;
using TreeViewWPF.Directory.ViewModels;

namespace TreeViewWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new DirectoryStructureViewModel();
        }

    }    
}


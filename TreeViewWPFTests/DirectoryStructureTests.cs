using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using TreeViewWPF.Directory;
using TreeViewWPF.Directory.ViewModels;
using TreeViewWPF.Directory.Data;

namespace TreeViewWPFTests
{
    [TestClass]
    public class DirectoryStructureTests
    {
        private string path;
        private DirectoryInfo folder;
        private FileStream file;

        [TestInitialize]
        public void TestInitialize()
        {
            path = @"Tests";

            folder = Directory.CreateDirectory(path);
            Directory.CreateDirectory(path).CreateSubdirectory(@"Test");

            var fileName = @"Tests\\Test.txt";
            File.WriteAllText(fileName, "Hello word!");

            file = File.OpenRead(fileName);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            file.Dispose();
            Directory.Delete(path, true);
        }


        [TestMethod]
        public void GetLogicalDrives()
        {
            Assert.AreEqual(DirectoryStructure.GetLogicalDrives().Count > 0, true);
        }

        [TestMethod]
        public void GetDirectoryContents()
        {       
            Assert.AreEqual(DirectoryStructure.GetDirectoryContents(path).Count, 2);
            Assert.IsTrue(DirectoryStructure.GetDirectoryContents(path).Count(f => f.Type == DirectoryItemType.File) == 1);
            Assert.IsTrue(DirectoryStructure.GetDirectoryContents(path).Count(f => f.Type == DirectoryItemType.Folder) == 1);
        }



        [TestMethod]
        public async Task UpdateSize()
        {
            ObservableCollection<DirectoryItemViewModel> folders = new();

            folders.Add(new DirectoryItemViewModel(folder.FullName, DirectoryItemType.Folder, "0"));

            Assert.AreEqual(folders.First().Size, "0");        

            await DirectoryStructure.UpdateSize(folders);
            await Task.Delay(TimeSpan.FromSeconds(5));

            Assert.AreEqual(folders.First().Size, string.Concat(file.Length," B"));
        }

        [ExpectedException(typeof(ArgumentNullException), "Exception was not thrown")]
        [TestMethod]
        public Task UpdateSize_Exception()
        {
           return DirectoryStructure.UpdateSize(null);
        }
    }
}

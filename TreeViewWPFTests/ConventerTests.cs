using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeViewWPF.Helpers;


namespace TreeViewWPFTests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void GetFileFolderName_EmptyPath()
        {
            var expected = string.Empty;

            Assert.AreEqual(Converter.GetFileFolderName(null), expected);
        }

        [TestMethod]
        public void GetFileFolderName_NotEmptyPath()
        {
            var expected = "Autorun";

            Assert.AreEqual(Converter.GetFileFolderName(@"D:\\Autorun"), expected);
        }

        [TestMethod]
        public void ConvertBytes_100_return100B()
        {
            var expected = @"100 B";

            Assert.AreEqual(Converter.ConvertBytes(100), expected);
        }

        [TestMethod]
        public void ConvertBytes_100000000_return100B()
        {
            var expected = @"95 MB";

            Assert.AreEqual(Converter.ConvertBytes(100000000), expected);
        }

    }


}
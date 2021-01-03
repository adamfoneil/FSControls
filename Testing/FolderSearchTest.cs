using FSControls.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Testing
{
    [TestClass]
    public class FolderSearchTest
    {
        [TestMethod]
        public void OneDrive()
        {
            var fs = new DirectoryScanner();
            var result = fs.ExecuteAsync(@"c:\users\adamo\OneDrive").Result;
            Assert.IsTrue(result.Folders.All(dir => Directory.Exists(dir.Path)));
        }
    }
}

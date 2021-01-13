using FSUtil.Library;
using FSUtil.Library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Testing
{
    [TestClass]
    public class FolderSearchTest
    {
        [TestMethod]
        public void OneDrive()
        {
            var fs = new DirectoryScanner();
            var result = fs.ExecuteAsync(@"c:\users\adamo\OneDrive", loadFiles: false).Result;
            Assert.IsTrue(result.Folders.All(dir => Directory.Exists(dir.Path)));
        }

        [TestMethod]
        public void Serialization()
        {
            var fs = new DirectoryScanner()
            {
                IgnoreNames = new string[]
                {
                    ".git",
                    "bin",
                    "obj",
                    "Debug",
                    "Release",
                    ".vs",
                    "TestResults"
                }.ToHashSet()
            };
            var result = fs.ExecuteAsync(@"C:\Users\adamo\Source\Repos\Dapper.CX", loadFiles: false).Result;
            var json1 = JsonSerializer.Serialize(result);

            var structure = JsonSerializer.Deserialize<LocalDirectory>(json1);
            var json2 = JsonSerializer.Serialize(structure);
            Assert.IsTrue(json1.Equals(json2));
        }

        [TestMethod]
        public void ToFolderTest()
        {
            var items = new string[]
            {
                "this/that/other",
                "this/that/another",
                "this/willy/hello",
                "yambo/that/other",
                "yambo/yilma/hoopla"
            };

            var folders = FolderExtensions.ToFolder(items, (item) => item, '/');

            var expected = new LocalDirectory[] 
            {
                new LocalDirectory()
                {
                    Name = "this",
                    Path = "this",
                    Folders = new LocalDirectory[]
                    {
                        new LocalDirectory()
                        {
                            Name = "that",
                            Path = "this/that",
                            Folders = new LocalDirectory[]
                            {
                                new LocalDirectory()
                                {
                                    Name = "other",
                                    Path = "this/that/other"
                                },
                                new LocalDirectory()
                                {
                                    Name = "another",
                                    Path = "this/that/another"
                                }
                            }
                        },
                        new LocalDirectory()
                        {
                            Name = "willy",
                            Path = "this/willy",
                            Folders = new LocalDirectory[]
                            {
                                new LocalDirectory()
                                {
                                    Name = "hello",
                                    Path = "this/willy/hello"
                                }
                            }
                        }
                    }
                },
                new LocalDirectory()
                {
                    Name = "yambo",
                    Path = "yambo",
                    Folders = new LocalDirectory[]
                    {
                        new LocalDirectory()
                        {
                            Name = "that",
                            Path = "yambo/that",
                            Folders = new LocalDirectory[]
                            {
                                new LocalDirectory()
                                {
                                    Name = "other",
                                    Path = "yambo/that/other"
                                }
                            }
                        },
                        new LocalDirectory()
                        {
                            Name = "yilma",
                            Path = "yambo/yilma",
                            Folders = new LocalDirectory[]
                            {
                                new LocalDirectory()
                                {
                                    Name = "hoopla",
                                    Path = "yambo/yilma/hoopla"
                                }
                            }
                        }
                    }
                }
            };

            var srcJson = JsonSerializer.Serialize(folders);
            var expectedJson = JsonSerializer.Serialize(expected);
            Assert.IsTrue(srcJson.Equals(expectedJson));
        }
    }
}

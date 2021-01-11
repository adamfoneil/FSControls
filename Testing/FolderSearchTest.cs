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
            var result = fs.ExecuteAsync(@"c:\users\adamo\OneDrive").Result;
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
            var result = fs.ExecuteAsync(@"C:\Users\adamo\Source\Repos\Dapper.CX").Result;
            var json1 = JsonSerializer.Serialize(result);

            var structure = JsonSerializer.Deserialize<Folder>(json1);
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

            var folders = FolderExtensions.ToFolder(items, '/');

            var expected = new Folder[] 
            {
                new Folder()
                {
                    Name = "this",
                    Folders = new Folder[]
                    {
                        new Folder()
                        {
                            Name = "that",
                            Folders = new Folder[]
                            {
                                new Folder()
                                {
                                    Name = "other"
                                },
                                new Folder()
                                {
                                    Name = "another"
                                }
                            }
                        },
                        new Folder()
                        {
                            Name = "willy",
                            Folders = new Folder[]
                            {
                                new Folder()
                                {
                                    Name = "hello"
                                }
                            }
                        }
                    }
                },
                new Folder()
                {
                    Name = "yambo",
                    Folders = new Folder[]
                    {
                        new Folder()
                        {
                            Name = "that",
                            Folders = new Folder[]
                            {
                                new Folder()
                                {
                                    Name = "other"
                                }
                            }
                        },
                        new Folder()
                        {
                            Name = "yilma",
                            Folders = new Folder[]
                            {
                                new Folder()
                                {
                                    Name = "hoopla"
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

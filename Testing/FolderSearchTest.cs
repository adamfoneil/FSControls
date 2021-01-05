﻿using FSUtil.Library;
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
                "this/that/other/hello.jpg",
                "this/that/another/goodbye.jpg",
                "this/willy/hello/thiska.docx",
                "yambo/that/other/whenever.txt",
                "yambo/yilma/hoopla/thalamus.json"
            };

            var folder = FolderExtensions.ToFolder(items, '/');
        }
    }
}

using FSUtil.Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FSUtil.Library
{
    public class DirectoryScanner
    {
        public DirectoryScanner()
        {
        }

        public HashSet<string> IgnoreNames { get; set; }

        public async Task<Folder> ExecuteAsync(string rootPath)
        {
            var result = new Folder()
            {
                Name = rootPath
            };

            await Task.Run(() =>
            {
                result.Folders = GetChildren(result, rootPath);
            });

            return result;

            IEnumerable<Folder> GetChildren(Folder parent, string path)
            {                
                var folders = GetDirectories(path);

                List<Folder> results = new List<Folder>();
                foreach (var dir in folders)
                {
                    var name = dir.Split('\\').Last();
                    if (IgnoreNames?.Contains(name) ?? false) continue;

                    var folder = new Folder()
                    {
                        Parent = parent,
                        Name = name
                    };

                    folder.Folders = GetChildren(folder, dir);

                    results.Add(folder);
                }

                return results;                
            }
        }

        private static IEnumerable<string> GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            }
            catch 
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}

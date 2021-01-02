using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSControls.Library
{
    public class FolderSearch
    {
        private readonly string _cachePath;

        public FolderSearch(string cachePath)
        {
            _cachePath = cachePath;
        }

        public async Task<Folder> ExecuteAsync(string rootPath)
        {
            return await ExecuteAsync(rootPath, null);
        }

        public async Task<Folder> ExecuteAsync(string rootPath, string nameContains)
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
                    var folder = new Folder()
                    {
                        Parent = parent,
                        Name = name,
                        IsSearchResult = !string.IsNullOrEmpty(nameContains) ? name.ToLower().Contains(nameContains.ToLower()) : false
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

    public class Folder
    {
        public Folder Parent { get; set; }
        public string Name { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public bool IsSearchResult { get; set; }

        public override string ToString() => Path;

        public string Path
        {
            get
            {
                List<string> results = new List<string>();
                                
                var dir = this;
                do
                {
                    results.Add(dir.Name);
                    dir = dir.Parent;
                } while (dir != null);

                results.Reverse();
                return string.Join("\\", results);
            }
        }
    }
}

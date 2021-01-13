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

        public async Task<LocalDirectory> ExecuteAsync(string rootPath, bool loadFiles = true)
        {
            var result = new LocalDirectory()
            {
                Name = rootPath,
                Path = rootPath
            };

            await Task.Run(() =>
            {
                result.Folders = GetChildren(result, rootPath);
            });

            return result;

            IEnumerable<LocalDirectory> GetChildren(LocalDirectory parent, string path)
            {                
                var localDirectories = GetDirectories(path);

                List<LocalDirectory> results = new List<LocalDirectory>();
                foreach (var dir in localDirectories)
                {
                    var name = dir.Split('\\').Last();
                    if (IgnoreNames?.Contains(name) ?? false) continue;

                    var localDirectory = new LocalDirectory()
                    {
                        Parent = parent,
                        Name = name,                        
                    };

                    localDirectory.Path = GetPath(localDirectory);

                    if (loadFiles)
                    {
                        localDirectory.Object = Directory.GetFiles(path).Select(fileName => new FileInfo(fileName));
                    }
                    
                    localDirectory.Folders = GetChildren(localDirectory, dir);

                    results.Add(localDirectory);
                }

                return results;                

                string GetPath(LocalDirectory dir)
                {
                    List<string> names = new List<string>();
                    
                    do
                    {
                        names.Add(dir.Name);
                        dir = dir.Parent as LocalDirectory;
                    } while (dir != null);

                    names.Reverse();
                    return string.Join("\\", names);
                }
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

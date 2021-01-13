using FSUtil.Library.Models;
using FSUtil.Library.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSUtil.Library
{
    public static class FolderExtensions
    {
        /*
        public static IEnumerable<Folder<T>> Search<T>(this Folder<T> folder, string query)
        {
            var paths = GetAllPaths(folder).Where(path => IsSearchResult(path));

            return ToFolder<T>(paths, (path) => path.ToString(), '\\');

            bool IsSearchResult(string path)
            {
                var words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return words.All(wrd => path.ToLower().Contains(wrd.ToLower()));
            }
        }        

        public static IEnumerable<Folder<T>> GetAllPaths<T>(this Folder<T> folder)
        {
            List<string> results = new List<string>();

            AddChildPaths(folder);

            return results;

            void AddChildPaths(Folder<T> parent)
            {
                results.Add(parent.Name);
                foreach (var child in parent.Folders) AddChildPaths(parent);
            }
        }*/

        public static IEnumerable<Folder<T>> ToFolder<T>(IEnumerable<T> items, Func<T, string> pathAccessor, char pathSeparator)
        {
            var pathFolders = items.Select(item => new FolderAnalyzer<T>()
            {
                Folders = pathAccessor.Invoke(item).Split(pathSeparator).ToArray(),
                Object = item
            });

            var results = pathFolders
                .Where(folders => folders.Folders.Length >= 1)
                .GroupBy(folders => folders.Folders[0])
                .Select(grp =>
                {
                    var result = new Folder<T>()
                    {
                        Name = grp.Key,
                        Path = grp.Key
                    };

                    result.Folders = GetSubfolders(result, grp);

                    return result;
                });

            return results;

            IEnumerable<Folder<T>> GetSubfolders(Folder<T> parent, IEnumerable<FolderAnalyzer<T>> subtreeFolders)
            {
                var nextLevel = subtreeFolders
                    .Select(folders => new FolderAnalyzer<T>()
                    {
                        Folders = folders.Folders.Skip(1).ToArray(),
                        Object = folders.Object
                    })
                    .Where(folders => folders.Folders.Length > 0)
                    .ToArray();

                var result = nextLevel
                    .GroupBy(folders => folders.Folders[0])
                    .Select(grp =>
                    {
                        var child = new Folder<T>()
                        {
                            Parent = parent,
                            Name = grp.Key,
                            Path = GetPath(parent, grp.Key)                            
                        };

                        if (grp.Count() == 1) child.Object = grp.First().Object;

                        child.Folders = GetSubfolders(child, grp);
                        return child;
                    });

                return result;
            }

            string GetPath(Folder<T> parent, string name)
            {
                List<string> folders = new List<string>();
                folders.Add(name);
                while (parent != null)
                {
                    folders.Add(parent.Name);
                    parent = parent.Parent;
                }
                folders.Reverse();
                return string.Join(pathSeparator.ToString(), folders);
            }
        }
    }
}

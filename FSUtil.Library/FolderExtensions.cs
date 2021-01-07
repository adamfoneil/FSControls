using FSUtil.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSUtil.Library
{
    public static class FolderExtensions
    {
        public static IEnumerable<Folder> Search(this Folder folder, string query)
        {
            var paths = GetAllPaths(folder).Where(path => IsSearchResult(path));

            return ToFolder(paths, '\\');

            bool IsSearchResult(string path)
            {
                var words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return words.All(wrd => path.ToLower().Contains(wrd.ToLower()));
            }
        }        

        public static IEnumerable<string> GetAllPaths(this Folder folder)
        {
            List<string> results = new List<string>();

            AddChildPaths(folder);

            return results;

            void AddChildPaths(Folder parent)
            {
                results.Add(parent.Name);
                foreach (var child in parent.Folders) AddChildPaths(parent);
            }
        }

        public static IEnumerable<Folder> ToFolder(IEnumerable<string> paths, char pathSeparator)
        {
            var pathFolders = paths.Select(path => path.Split(pathSeparator).ToArray());            

            var results = pathFolders
                .Where(folders => folders.Length >= 2)
                .GroupBy(folders => folders[0])
                .Select(grp => new Folder()
                {
                    Name = grp.Key,
                    Folders = GetSubfolders(grp, 1)
                });

            return results;

            IEnumerable<Folder> GetSubfolders(IEnumerable<string[]> subtreeFolders, int index)
            {
                var analyzeSubtree = subtreeFolders
                    .Where(folders => folders.Length > index)
                    .Select(folders => folders.Skip(1).ToArray());

                var result = analyzeSubtree
                    .Where(folders => folders.Length == index + 1)
                    .GroupBy(folders => folders[index])
                    .Select(grp => new Folder()
                    {
                        Name = grp.Key,
                        Folders = GetSubfolders(grp, index + 1)
                    });

                return result;
            }
        }
    }
}

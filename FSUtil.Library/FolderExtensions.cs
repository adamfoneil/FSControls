using FSUtil.Library.Models;
using System.Collections.Generic;

namespace FSUtil.Library
{
    public static class FolderExtensions
    {
        /// <summary>
        /// returns a new folder structure where the
        /// </summary>
        public static Folder Search(this Folder folder, string query)
        {
            Folder result = new Folder() 
            { 
                Name = folder.Name,
                Folders = FindSubfolders(folder)
            };

            return result;

            IEnumerable<Folder> FindSubfolders(Folder parent)
            {
                List<Folder> results = new List<Folder>();

                foreach (var subdir in parent.Folders)
                {
                    if (subdir.Name.ToLower().Contains(query.ToLower()))
                    {
                        results.Add(subdir);
                    }
                }

                return results;
            }
        }
    }
}

using System.Collections.Generic;

namespace FSControls.Library.Models
{
    public class Folder
    {
        public Folder Parent { get; set; }
        public string Name { get; set; }
        public IEnumerable<Folder> Folders { get; set; }

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

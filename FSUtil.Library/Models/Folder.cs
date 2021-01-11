using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FSUtil.Library.Models
{
    public class Folder
    {
        [JsonIgnore]
        public Folder Parent { get; set; }
        public string Name { get; set; }
        public IEnumerable<Folder> Folders { get; set; } = Enumerable.Empty<Folder>();
        public string Path { get; set; }
        public override string ToString() => Path;
    }
}

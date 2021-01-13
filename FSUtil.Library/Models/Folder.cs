using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FSUtil.Library.Models
{
    public class Folder<T>
    {
        [JsonIgnore]
        public Folder<T> Parent { get; set; }
        public string Name { get; set; }
        public T Object { get; set; }
        public IEnumerable<Folder<T>> Folders { get; set; } = Enumerable.Empty<Folder<T>>();
        public string Path { get; set; }
        public override string ToString() => Path;
    }
}

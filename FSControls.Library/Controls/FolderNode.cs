using FSUtil.Library.Models;
using System.Windows.Forms;

namespace FSControls.Library.Controls
{
    public class FolderNode : TreeNode
    {
        public FolderNode(LocalDirectory dir) : base(dir.Name)
        {
            ImageKey = "folder";
            SelectedImageKey = "folder";
            Directory = dir;
        }

        public LocalDirectory Directory { get; }
    }
}

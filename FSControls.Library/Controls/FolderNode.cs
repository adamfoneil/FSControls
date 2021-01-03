using System.Windows.Forms;

namespace FSControls.Library.Controls
{
    public class FolderNode : TreeNode
    {
        public FolderNode(string name) : base(name)
        {
            ImageKey = "folder";
            SelectedImageKey = "folder";
        }
    }
}

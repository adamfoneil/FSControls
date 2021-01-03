using FSControls.Library.Controls;
using FSUtil.Library;
using FSUtil.Library.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSUtil.Controls
{
    public partial class FolderTree : UserControl
    {
        private List<Folder> _folders = new List<Folder>();

        public FolderTree()
        {
            InitializeComponent();
        }        

        public HashSet<string> IgnoreNames { get; set; }

        public new Font Font
        {
            get { return treeView1.Font; }
            set
            {
                treeView1.Font = value;
                tbSearch.Font = value;
            }
        }

        public async Task FillAsync(string[] rootFolders)
        {
            var dirScan = new DirectoryScanner();
            dirScan.IgnoreNames = IgnoreNames;

            treeView1.Nodes.Clear();
            treeView1.BeginUpdate();
            try
            {
                foreach (var root in rootFolders)
                {
                    var tree = await dirScan.ExecuteAsync(root);
                    _folders.Add(tree);
                    LoadNodes(tree, null);
                }
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void LoadNodes(Library.Models.Folder tree, FolderNode parent)
        {
            var node = new FolderNode(tree.Name);

            if (parent == null)
            {
                treeView1.Nodes.Add(node);
            }    
            else
            {
                parent.Nodes.Add(node);
            }

            foreach (var subdir in tree.Folders) LoadNodes(subdir, node);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {

                throw;
            }
        }
    }
}

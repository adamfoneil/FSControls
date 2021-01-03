using System;
using System.Windows.Forms;

namespace FSUtil.Controls
{
    public partial class FolderTree : UserControl
    {
        public FolderTree()
        {
            InitializeComponent();
        }

        public event EventHandler RootChanged;

        private string _root;
        public string Root
        {
            get => _root;
            set
            {
                if (value != _root)
                {
                    _root = value;
                    RootChanged?.Invoke(this, new EventArgs());
                }                
            }
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

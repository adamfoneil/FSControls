using System;
using System.Linq;
using System.Windows.Forms;

namespace FSUtil
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            folderTree1.IgnoreNames = new string[]
            {
                 ".git",
                "bin",
                "obj",
                "Debug",
                "Release",
                ".vs",
                "TestResults"
            }.ToHashSet();

            await folderTree1.FillAsync(new string[] 
            {
                @"c:\users\adamo\OneDrive",
                @"c:\users\adamo\Source\Repos"
            });
        }
    }
}

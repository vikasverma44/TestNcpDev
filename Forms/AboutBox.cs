using SQLDataMaskingConfigurator.Repository;
using System;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Forms
{
    internal partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            RepoAssembly asm = new RepoAssembly();
            Text = string.Format("About {0}", asm.AssemblyTitle());
            labelProductName.Text = asm.AssemblyProduct();
            labelVersion.Text = string.Format("Version {0}", asm.AssemblyVersion());
            labelCopyright.Text = asm.AssemblyCopyright();
            labelCompanyName.Text = asm.AssemblyCompany();
            textBoxDescription.Text = asm.AssemblyDescription();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

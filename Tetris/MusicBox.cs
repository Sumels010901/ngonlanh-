using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MusicBox : Form
    {
        public MusicBox()
        {
            InitializeComponent();
        }

        private void MusicBox_Load(object sender, EventArgs e)
        {
            addnhactumay.Multiselect = true;
        }
        string[] files, path;

        private void playlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mayphatnhac.URL = path[playlistBox.SelectedIndex];
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MusicBox_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (addnhactumay.ShowDialog() == System.Windows.Forms.DialogResult.OK)

            {

                files = addnhactumay.SafeFileNames;

                path = addnhactumay.FileNames;

                for (int i = 0; i < files.Length; i++)

                {

                    playlistBox.Items.Add(files[i]);

                }

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditorGui
{
    public partial class TileForm1 : Form
    {
        string path;
        Image img;
        int inCol, inRow, outCol, outRow;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out inRow) && int.TryParse(textBox2.Text, out inCol))
            {
                new TileLoad(img, new Point(inCol, inRow));
            }
            else
            {
                MessageBox.Show("make sure the textfields are numbers");
            }
        }

        public TileForm1()
        {
            img = Properties.Resources.tileset;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    path = dlg.FileName;
                    textBox5.Text = path;
                    img = new Bitmap(path);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out inRow) && int.TryParse(textBox2.Text, out inCol)
             && int.TryParse(textBox3.Text, out outRow) && int.TryParse(textBox4.Text, out outCol))
            {
                TileForm2 f = new TileForm2(img, new Point(inCol,inRow), new Point(outCol,outRow));
                f.ShowDialog();

            }
            else {
                MessageBox.Show("make sure the textfields are numbers");
            }
        }
    }
}

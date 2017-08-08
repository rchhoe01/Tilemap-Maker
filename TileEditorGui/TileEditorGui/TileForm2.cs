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
    public partial class TileForm2 : Form
    {
        MouseEventArgs me;
        TiledImage t;
        public Point input, output, scale;
        ToolStripDropDown popup;
        ToolStripControlHost host;
        PictureBox p;
        TilePaintingCanvas c;
        Point pressCanvas, pressImage;
        public List<TileLayers> l;
        public Image img;
        bool layerfill, brushmode, mousedown, grid = true;
        int brush = 0;
        public TileForm2(Image img, Point input, Point output)
        {
            InitializeComponent();

            this.img = img;
            //scale canvas
            this.input = input;
            this.output = output;
            scale = new Point(img.Width / input.X, img.Height / input.Y);
            this.Size = new Size((output.X * scale.X) + 164 + 16, (output.Y * scale.Y) + 100);
            pictureBox1.Size = new Size(output.X * scale.X, output.Y * scale.Y);

            //construct picturebox
            p = new PictureBox();
            p.Click += new EventHandler(clickimage);

            //bool fill event


            //construct objects
            t = new TiledImage(ref p, input, scale, img);
            t.drawGrid();
            c = new TilePaintingCanvas(ref pictureBox1, input, output, scale, img);
            c.drawGrid(grid);
            l = new List<TileLayers>();




            //construct click popup
            popup = new ToolStripDropDown();
            popup.Margin = Padding.Empty;
            popup.Padding = Padding.Empty;
            host = new ToolStripControlHost(p);
            host.Margin = Padding.Empty;
            host.Padding = Padding.Empty;
            popup.Items.Add(host);
        }
        private void clickimage(object sender, EventArgs e)
        {
            me = (MouseEventArgs)e;
            pressImage = new Point(me.X, me.Y);
            if (layerfill)
            {
                l[listBox1.SelectedIndex].fill(t.getTileNum(pressImage));
                c.refreshLayers(ref l, grid);
                layerfill = false;
                button1.BackColor = SystemColors.Control;
            }
            else if (brushmode)
            {
                brush = t.getTileNum(pressImage);
                popup.Hide();
            }
            else
            {
                l[listBox1.SelectedIndex].updateTileSet(pressCanvas, t.getTileNum(pressImage));
                c.refreshIndividual(new Point(pressCanvas.X / scale.X, pressCanvas.Y / scale.Y), l, t, grid);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            me = (MouseEventArgs)e;
            pressCanvas = new Point(me.X, me.Y);

            if (listBox1.SelectedIndex != -1)
            {
                popup.Show(this, new Point(Width, 0));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
            if (brushmode && listBox1.SelectedIndex != -1)
            {
                me = (MouseEventArgs)e;
                pressCanvas = new Point(me.X, me.Y);
                l[listBox1.SelectedIndex].updateTileSet(pressCanvas, brush);
                c.refreshIndividual(new Point(pressCanvas.X / scale.X, pressCanvas.Y / scale.Y), l, t, grid);
                //c.refreshAll(ref l);
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedown && brushmode && listBox1.SelectedIndex != -1) {
                me = (MouseEventArgs)e;
                pressCanvas = new Point(me.X, me.Y);
                l[listBox1.SelectedIndex].updateTileSet(pressCanvas, brush);
                c.refreshIndividual(new Point(pressCanvas.X/scale.X, pressCanvas.Y/scale.Y), l, t, grid);
                //c.refreshAll(ref l);
            }
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousedown && brushmode && listBox1.SelectedIndex != -1)
            {
                me = (MouseEventArgs)e;
                pressCanvas = new Point(me.X, me.Y);
                l[listBox1.SelectedIndex].updateTileSet(pressCanvas, brush);
                c.refreshIndividual(new Point(pressCanvas.X / scale.X, pressCanvas.Y / scale.Y), l, t, grid);
                //c.refreshAll(ref l);
            }
            mousedown = false;
            
        }

        private void addLayer_Click(object sender, EventArgs e)
        {
            String layname = TileDialog.ShowDialog(listBox1.Items.Count);
            if (!String.IsNullOrWhiteSpace(layname))
            {
                listBox1.Items.Add(layname);
                l.Add(new TileLayers(layname, input, output, scale, img));
            }
        }

        private void removeLayer_Click(object sender, EventArgs e)
        {
            if ((listBox1.SelectedIndex != -1))
            {
                l.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                c.refreshLayers(ref l, grid);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            l[listBox1.SelectedIndex].visibility = checkBox1.Checked;
            c.refreshLayers(ref l, grid);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                //checkBox1.Checked = false;
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = l[listBox1.SelectedIndex].visibility;

            }
        }
        //layer fill
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                layerfill ^= true;
                if (layerfill)
                {
                    button1.BackColor = SystemColors.ControlDark;
                    popup.Show(this, new Point(Width, 0));

                }
                else
                {
                    button1.BackColor = SystemColors.Control;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TileSave(ref l);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            grid = checkBox3.Checked;
            c.refreshLayers(ref l, grid);
        }

        private void exportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TileSaveImage(l);
        }

        private void layerUp_Click(object sender, EventArgs e)
        {

        }

        private void layerDown_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            brushmode = !brushmode;
            if (brushmode) popup.Show(this, checkBox2.Location);
        }
        //outside methods
        public void loadMe(string name)
        {
            listBox1.Items.Add(name);
        }
        public void loadDrawMe()
        {
            c.refreshLayers
                (ref l,grid);
        }
    }
}

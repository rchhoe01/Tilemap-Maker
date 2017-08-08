using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditorGui
{
    public class TiledImage
    {
        Point colRow, scale, size;
        Image tileImage;
        PictureBox pb;
        Graphics g;
        public TiledImage(ref PictureBox pb, Point input, Point scale, Image img) {
            tileImage = img;
            colRow = input;
            this.scale = scale;
            this.pb = pb;
            size = new Point(colRow.X*scale.X, colRow.Y*scale.Y);

            
            pb.Size = new Size(size);            
            pb.BackgroundImage = tileImage;
            pb.Image = new Bitmap(tileImage.Width, tileImage.Height);

        }

        public int getTileNum(Point imgclick) {
            Point clickedImage = new Point(imgclick.X / scale.X, imgclick.Y / scale.Y);
            return (clickedImage.Y + 1) * (colRow.X) - ((colRow.X) - (clickedImage.X));
        }
        public Point getTilePoint(int tilenum) {
            return new Point((tilenum % colRow.X) * scale.X, (tilenum / colRow.X) * scale.Y);
        }
        public void drawGrid(){            
            pb.Image = new Bitmap(tileImage.Width, tileImage.Height);
            g = Graphics.FromImage(pb.Image);
            g.Clear(Color.Transparent);
            for (int a = 0; a <= colRow.Y; a++)
            {
                g.DrawLine(new Pen(Color.Blue), 0, a * scale.Y, 
                                 scale.X*colRow.X, a * scale.Y);

            }
            for (int b = 0; b <= colRow.X; b++)
            {
                g.DrawLine(new Pen(Color.Blue), b * scale.X, 0, 
                                                b * scale.X, scale.Y*colRow.Y);
            }

        }

        public void drawNumbers() {
            pb.Image = new Bitmap(tileImage.Width, tileImage.Height);

        }

        //internal method
        public Point clicked(int x, int y)
        {
            x = x / scale.X;
            y = y / scale.Y;
            return new Point(x*scale.X, y*scale.Y);
        }
    }
}

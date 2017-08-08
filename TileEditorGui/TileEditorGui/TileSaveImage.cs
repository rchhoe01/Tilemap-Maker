using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditorGui
{
    class TileSaveImage
    {
        Image img;
        Graphics g;
        SaveFileDialog saveFileDialog1;
        public TileSaveImage(List<TileLayers> l) {
            img = new Bitmap(l[0].colRow.X * l[0].scale.X, l[0].colRow.Y * l[0].scale.Y);
            g = Graphics.FromImage(img);
            for (int a = 0; a < l.Count; a++) {
                g.DrawImage(l[a].layerImage, 0, 0);
            }
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        img.Save(fs,System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        img.Save(fs,System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        img.Save(fs,System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }

        }
    }
}

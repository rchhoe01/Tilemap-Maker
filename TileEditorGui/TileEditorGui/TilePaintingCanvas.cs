using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditorGui
{
    public class TilePaintingCanvas
    {
        public Point imgColRow, colRow, scale, size;
        public Graphics g;
        public PictureBox pb;
        public Image img, clearedTile;
        public List<Point> canvasCoordinates;
        GraphicsUnit units;
        public TilePaintingCanvas(ref PictureBox pb, Point input, Point output, Point scale, Image img) {
            this.imgColRow = input;
            this.colRow = output;
            this.scale = scale;
            this.size = new Point(output.X * scale.X, output.Y * scale.Y);
            this.pb = pb;
            this.pb.Image = new Bitmap(size.X, size.Y);
            this.img = img;
            this.g = Graphics.FromImage(pb.Image);
            this.g.Clear(Color.Transparent);
            this.canvasCoordinates = canvasPoints();
            units = GraphicsUnit.Pixel;
            //create a control colored tile
            this.clearedTile = new Bitmap(scale.X, scale.Y);
            Graphics temp = Graphics.FromImage(clearedTile);
            temp.Clear(SystemColors.Control);
            temp.Dispose();
        }

        public void drawGrid(bool grid) {
            if (grid)
            {
                for (int a = 0; a <= colRow.Y; a++)
                {
                    g.DrawLine(new Pen(Color.Red), 0, a * scale.Y,
                                              scale.X*colRow.X, a * scale.Y);

                }
                for (int b = 0; b <= colRow.X; b++)
                {
                    g.DrawLine(new Pen(Color.Red), b * scale.X, 0,
                                                   b * scale.X, scale.Y*colRow.Y);
                }
            }
        }
        public List<Point> canvasPoints() {
            List<Point> canvasPoints = new List<Point>();
            for (int a = 0; a < colRow.Y; a++) {
                for (int b = 0; b < colRow.X; b++) {
                    canvasPoints.Add(new Point(b*scale.X,a*scale.Y));
                }
            }
            return canvasPoints;
        }
        //pass drawlayer to paint
        public void paintLayer(Point[] points) {
            for (int a = 0; a < points.Length; a++) {
                g.DrawImage(img,
                            //new Rectangle(canvasCoordinates[a], new Size(scale.X, scale.Y)),
                            canvasCoordinates[a].X, canvasCoordinates[a].Y,
                            new Rectangle(points[a] ,new Size(scale.X, scale.Y)),
                            units);
            }
            
        }
        public void refreshIndividual(Point pressCan, List<TileLayers> l, TiledImage i, bool drawgrid) {
            Point pressCanvas = new Point(pressCan.X*scale.X, pressCan.Y*scale.Y);
            g.DrawImage(clearedTile,pressCanvas);
            for (int a = 0; a < l.Count; a++) {
                g.DrawImage(img, pressCanvas.X, pressCanvas.Y, new Rectangle(i.getTilePoint(  l[a].tileset[l[a].PointToTile(pressCan)]  ), new Size(scale)), units);
            }
            drawGrid(drawgrid);
            pb.Refresh();
        }

        public void clearCanvas() {
            
        }

        public void refreshAll(ref List<TileLayers> tl, bool drawgrid) {
            g.Clear(Color.Transparent);
            for (int a = 0; a < tl.Count; a++) {
                if (tl[a].visibility) { 
                    paintLayer(tl[a].drawLayer());
                }
            }
            drawGrid(drawgrid);
            pb.Refresh();
        }
        public void refreshLayers(ref List<TileLayers> tl, bool drawgrid)
        {
            g.Clear(Color.Transparent);
            for (int a = 0; a < tl.Count; a++)
            {
                if (tl[a].visibility)
                {
                    g.DrawImage(tl[a].layerImage, 0,0);
                }
            }
            drawGrid(drawgrid);
            pb.Refresh();
        }
        public void refresh(TileLayers tl, bool drawgrid)
        {
            if (tl.visibility)
            {
                paintLayer(tl.drawLayer());
            }
            drawGrid(drawgrid);
            pb.Refresh();
        }
    }
}

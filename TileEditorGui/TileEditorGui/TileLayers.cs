using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditorGui
{
    public class TileLayers
    {
        public bool visibility;
        public int [] tileset;
        public Point colRow, scale, imgColRow;
        public Image layerImage, img, clearedTile;
        public String name;
        public Graphics layerGraphic;
        GraphicsUnit units;
        public List<Point> canvasCoordinates;
        public TileLayers(String name, Point imgColRow, Point colRow,   Point scale, Image img) {
            this.visibility = true;
            this.name = name;
            this.colRow = colRow;
            this.scale = scale;
            this.imgColRow = imgColRow;
            
            //images and image Graphics
            this.img = img;
            this.layerImage = new Bitmap(colRow.X * scale.X, colRow.Y * scale.Y);
            this.layerGraphic = Graphics.FromImage(layerImage);
            this.units = GraphicsUnit.Pixel;

            //create a control colord tile
            this.clearedTile = new Bitmap(scale.X, scale.Y);
            Graphics temp = Graphics.FromImage(clearedTile);
            temp.Clear(SystemColors.Control);
            temp.Dispose();

            //directory of point coodinates 
            this.canvasCoordinates = canvasPoints();

            //initialize tilesetsize
            this.tileset = new int[colRow.X*colRow.Y];
            for (int a = 0; a < tileset.Length; a++) {
                tileset[a] = 0;
            }
        }

        public List<Point> canvasPoints()
        {
            List<Point> canvasPoints = new List<Point>();
            for (int a = 0; a < colRow.Y; a++)
            {
                for (int b = 0; b < colRow.X; b++)
                {
                    canvasPoints.Add(new Point(b * scale.X, a * scale.Y));
                }
            }
            return canvasPoints;
        }

        public Point[] drawLayer() {
            Point[] points = new Point[tileset.Length];
            for (int a = 0; a < tileset.Length; a++) {
                points[a] = TileToPoint(tileset[a]);
            }
            return points;
        }
        
        

        public void updateTileSet(Point coordinates, int imagenum) {

                Point clickedCanvas = new Point(coordinates.X / scale.X, coordinates.Y / scale.Y);
                tileset[PointToTile(clickedCanvas)] = imagenum;
                //update layer image
                clickedCanvas = new Point(clickedCanvas.X * scale.X, clickedCanvas.Y * scale.Y);
                updateImage(clickedCanvas, imagenum);

        }
        public void fill(int imagenum) {
            for (int a = 0; a < tileset.Length; a++)
            {
                tileset[a] = imagenum;
                updateImage(canvasCoordinates[a], imagenum);
            }
        }

        //specific to load  class
        public void loadUpdateTileSet(int index, int numchange)
        {
            tileset[index] = numchange;
            updateImage(canvasCoordinates[index], numchange);
        }

        public Point TileToPoint(int tilenum)
        {
            return new Point((tilenum % imgColRow.X) * scale.X, (tilenum / imgColRow.X) * scale.Y);
        }
        public int PointToTile(Point Point)
        {
            return (Point.Y + 1) * (colRow.X) - ((colRow.X) - (Point.X));
        }
        public void updateImage(Point coordinates, int imagenum) {

            layerGraphic.SetClip(new Rectangle(coordinates, new Size(scale)));
            layerGraphic.Clear(Color.Transparent);
            layerGraphic = Graphics.FromImage(layerImage);
            layerGraphic.DrawImage(img, coordinates.X, coordinates.Y,
                new Rectangle(TileToPoint(imagenum), new Size(scale)),
                units);
        }
    }
}

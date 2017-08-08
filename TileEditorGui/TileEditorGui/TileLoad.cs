using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//string text = "one\ttwo three:four,five six seven";
//System.Console.WriteLine("Original text: '{0}'", text);
//System.Console.WriteLine("{0} words in text:", words.Length);
//foreach (string s in words){System.Console.WriteLine(s);}
namespace TileEditorGui
{
    class TileLoad
    {

        OpenFileDialog openFileDialog1;
        List<int[]> layers;
        int rows = 16, cols = 16;

        public TileLoad(Image img, Point input)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                layers = new List<int[]>();
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                loading(lines);
                TileForm2 form = new TileForm2(img, input, new Point(cols, rows));
                updateLayers(ref form);
                form.ShowDialog();
            }


        }
        public void loading(string[] text)
        {
            for (int a = 0; a < text.Length; a++)
            {
                char[] delimiterChars = {' ',',','{', '}' };
                string[] words = text[a].Split(delimiterChars);
                int[] numbers = new int[rows * cols];
                int numcount = 0;
                int wordcount = 0;
                while (numcount<numbers.Length)
                {
                    if ( int.TryParse(words[wordcount], out numbers[numcount]) )
                    {
                        numcount++;
                    }
                    wordcount++;


                }
                layers.Add(numbers);
            }
        }
        public void updateLayers(ref TileForm2 form)
        {
            for (int a = 0; a < layers.Count; a++)
            {
                form.l.Add(new TileLayers("layer", form.input, form.output, form.scale, form.img));
                form.loadMe(form.l[form.l.Count-1].name);
                for (int b = 0; b < layers[a].Count(); b++) { 
                    form.l[form.l.Count - 1].loadUpdateTileSet(b, layers[a][b]);
                }
            }
            form.loadDrawMe();



        }


    }
}

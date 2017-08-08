using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
Example #3:
WriteAllLines creates a file, 
writes a collection of strings to the file, 
and then closes the file. 
You do NOT need to call Flush() or Close().
Example #2: Write one string to a text file. 
WriteAllText creates a file, 
writes the specified string to the file, 
and then closes the file. 
You do NOT need to call Flush() or Close().
Example #4: Append new text to an existing file.
The using statement automatically flushes AND CLOSES the stream and calls 
IDisposable.Dispose on the stream object.

directory = path + @"\WriteLines.txt";
directory2 = path + @"\WriteLines2.txt";
string[] lines = { "this", "is a", "test" };

System.IO.File.WriteAllLines(directory, lines);            
System.IO.File.WriteAllText(directory, directory);
*/
namespace TileEditorGui
{
    class TileSave
    {
        public String path;
        public String text;
        SaveFileDialog saveFileDialog1;
        public TileSave(ref List<TileLayers> bp)
        {
            /*
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                }
            }
            */


            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "text document|*.txt";
            saveFileDialog1.Title = "Save a text File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        path = saveFileDialog1.FileName;                        
                        break;
                    
                }

                fs.Close();
            }





            //path = path + @"/layers.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                foreach (TileLayers layer in bp)
                {
                    text = "{";
                    for (int a = 0; a < layer.tileset.Length; a++)
                    {
                        text = text + (layer.tileset[a]) + ",";
                    }
                    text = text + "}";
                    file.WriteLine(text);
                }
            }

        }

    }
}

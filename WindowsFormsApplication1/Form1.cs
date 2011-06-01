/*  
 * Adds a license header to all files in the given directory
 * Copyright (C) 2011  Revolution Smythe (asdfisbetterthanjkl_@_gmail.com)
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace LicenseHeader
{
    public partial class Form1 : Form
    {
        private string folder;
        private string[] license;
        public Form1 ()
        {
            InitializeComponent ();
        }

        private void Form1_Load (object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog ())
            {
                dialog.Description = "Open a folder which contains the files";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dialog.ShowDialog () == DialogResult.OK)
                {
                    folder = dialog.SelectedPath;
                }
            }
        }

        private void button1_Click (object sender, EventArgs e)
        {
            license = richTextBox1.Lines;
            IEnumerable<string> files = Directory.GetFiles (folder, textBox1.Text, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                List<string> newLines = new List<string> ();
                newLines.AddRange (license);
                string[] lines = File.ReadAllLines (file);

                if (lines[0].StartsWith ("/*"))
                {
                    int endLine = 0;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith (" */"))
                        {
                            endLine = i;
                            break;
                        }
                    }
                    List<string> l = new List<string>(lines);
                    l.RemoveRange(0, endLine + 1);
                    if (l[0] == "")
                        l.RemoveRange (0, 1);
                    newLines.AddRange (l);
                }
                else
                {
                    newLines.AddRange (lines);
                }
                File.WriteAllLines (file, newLines.ToArray ());
            }
        }
    }
}

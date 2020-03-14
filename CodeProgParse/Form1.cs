using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;

namespace CodeProgParse
{
    public partial class Form1 : Form
    {
        List<string> ext_list = new List<string>();
        List<string> dir_list = new List<string>();
        int index = 0;
        bool flag_stop = false;
        string result_path = "textProgramm.txt";

        public Form1()
        {
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.WriteAllText(result_path, "");
            index = 0;
            flag_stop = false;
            ext_list.Clear();
            ext_list.AddRange(listBox1.Items.OfType<string>());

            dir_list.Clear();
            dir_list.AddRange(listBox2.Items.OfType<string>());


            string path = textBox1.Text.Trim();
            if (File.Exists(path))
            {
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                ProcessDirectory(path);
            }
            else
            {
                MessageBox.Show("Задайте другой путь", "Ошибка выбранного пути", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (!flag_stop)
                    ProcessFile(fileName);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                bool valid = true;
                foreach (string param in dir_list)
                {
                    if (subdirectory.Contains(param))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                    continue;

                if (!flag_stop)
                    ProcessDirectory(subdirectory);
            }
        }

        public void ProcessFile(string path)
        {

            var t = Path.GetExtension(path).Replace(".", "");
            if (ext_list.Contains(t))
            {
                if (index > 0)
                    textBox2.Text += Environment.NewLine + (index + 1).ToString() + "\t" + getPath(path) + "\t" + getNumbersLines(path);
                else
                    textBox2.Text = (index + 1).ToString() + "\t" + getPath(path) + "\t" + getNumbersLines(path);
                index++;
                textBox2.Update();
            }
            else if (t.Equals("dll") || t.Equals("exe"))
            {
                if (index > 0)
                    textBox2.Text += Environment.NewLine + (index + 1).ToString() + "\t" + getPath(path) + "\t" + getSize(path);
                else
                    textBox2.Text = (index + 1).ToString() + "\t" + getPath(path) + "\t" + getSize(path);
                index++;
                textBox2.Update();
            }
                
            //if (index == 10)
            //    flag_stop = true;
        }

        public string getPath(string path)
        {
            return path.Replace(textBox1.Text,"");
        }

        public string getSize(string path)
        {
            long length = new System.IO.FileInfo(path).Length;
            return length.ToString();
        }

        public string getNumbersLines(string path)
        {
            string filename = Path.GetFileName(path);
            var lines = File.ReadAllLines(path).ToList();
            if (checkBox1.Checked)
            {
                File.AppendAllText(result_path, Environment.NewLine + "[Ё]" + filename + Environment.NewLine + Environment.NewLine);
                foreach (string SSS in lines)
                {
                    File.AppendAllText(result_path, SSS + Environment.NewLine);
                }
            }
            //File.AppendAllText(result_path, lines.ToString());
            return lines.Count().ToString();
        }
    }
}

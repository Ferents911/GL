using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using GL_tt.Models;
using System.Runtime.Serialization.Json;
using System.Collections;

namespace GL_tt
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string path;  

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
            label2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
          
            path = folderBrowserDialog1.SelectedPath;
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
            {
                FolderData di = new FolderData(new DirectoryInfo(path));
                string result = di.TreeToJson();
                File.WriteAllText("data.json", result);
                label2.Visible = true;
                textBox1.Clear();

            }
            else
            {
                MessageBox.Show("Choose the directory!");
            }



        }

    }
}

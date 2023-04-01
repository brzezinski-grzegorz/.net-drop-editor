using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;

namespace Drop_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // If file exist connect to database and show monster list.
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

            if (File.Exists(configFilePath))
            {
                NameValueCollection settings = new NameValueCollection();
                if (File.Exists(configFilePath))
                {
                    using (StreamReader reader = new StreamReader(configFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('=');
                            if (parts.Length == 2)
                            {
                                settings[parts[0]] = parts[1];
                            }
                        }
                    }
                }
                string server_name = settings["server_name"];
                string database_name = settings["database_name"];
                string table_name = settings["table_name"];
                string connection_string = settings["connection_string"];
            }
            else
            {
                MessageBox.Show("No configuration file found, you need to create one first.");
                this.Hide();
                Form2 form2 = new Form2();
                // Show Form2
                form2.ShowDialog();
            }
        }
    }
}

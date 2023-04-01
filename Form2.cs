using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Drop_Editor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the path to the config.ini file in the same folder as the executable
            string config_file_path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "config.ini");

            // Get the values from the textboxes
            string server_name = textBox1.Text;
            string database_name = textBox2.Text;
            string table_name = textBox3.Text;
            string connection_string = textBox4.Text;

            // Write the values to the config.ini file
            using (StreamWriter writer = new StreamWriter(config_file_path))
            {
                writer.WriteLine("[database_settings]");
                writer.WriteLine("server_name=" + server_name);
                writer.WriteLine("database_name=" + database_name);
                writer.WriteLine("table_name=" + table_name);
                writer.WriteLine("connection_string=" + connection_string);
            }

            // Show message that config file has been created.
            MessageBox.Show("Database settings saved to config.ini.");
            // Close this form after config file is created.
            this.Hide();
        }

    }
}

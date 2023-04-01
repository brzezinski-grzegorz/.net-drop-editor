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
            string server = textBox1.Text;
            string username = textBox2.Text;
            string password = textBox3.Text;
            string database = textBox4.Text;

            // Write the values to the config.ini file
            using (StreamWriter writer = new StreamWriter(config_file_path))
            {
                writer.WriteLine("[database_settings]");
                writer.WriteLine("server=" + server);
                writer.WriteLine("username=" + username);
                writer.WriteLine("password=" + password);
                writer.WriteLine("database=" + database);
            }

            // Show message that config file has been created.
            MessageBox.Show("Database settings saved to config.ini.");
            // Close this form after config file is created.
            this.Hide();
        }

    }
}

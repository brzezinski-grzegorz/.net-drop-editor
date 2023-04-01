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
using System.Data.Odbc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Cryptography.X509Certificates;

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
            string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");

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

                string server = settings["server"];
                string username = settings["username"];
                string password = settings["password"];
                string database = settings["database"];

                string connectionString = $"Driver={{SQL Server}};Server={server};Database={database};Uid={username};Pwd={password};";
                string queryOne = "SELECT * FROM Item";

                using (OdbcConnection connectionOne = new OdbcConnection(connectionString))
                {
                    connectionOne.Open();

                    using (OdbcCommand commandOne = new OdbcCommand(queryOne, connectionOne))
                    {
                        using (OdbcDataReader reader = commandOne.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access the data in each row by column name or index
                                int num = reader.GetInt32(0);
                                string strname = reader.GetString(1);
                                // ...

                                string rowText = $"{num} - {strname}";
                                Items.Items.Add(rowText);
                            }
                        }
                    }
                }

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

        private void button1_Click(object sender, EventArgs e)
        {
            // If file exist connect to database and show monster list.
            string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");

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

                string server = settings["server"];
                string username = settings["username"];
                string password = settings["password"];
                string database = settings["database"];

                string connectionString1 = $"Driver={{SQL Server}};Server={server};Database={database};Uid={username};Pwd={password};";
                // Construct a new query that filters by the selected row ID
                string query1 = "SELECT * FROM K_MONSTER_ITEM WHERE sIndex = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                OdbcConnection connection1 = new OdbcConnection(connectionString1);
                connection1.Open();

                OdbcCommand command1 = new OdbcCommand(query1, connection1);
                OdbcDataReader reader1 = command1.ExecuteReader();

                if (reader1.Read())
                {
                    // Access the data in the selected row by column name or index
                    int item01 = reader1.GetInt32(1);
                    string persent01 = reader1.GetString(2);
                    int item02 = reader1.GetInt32(3);
                    string persent02 = reader1.GetString(4);
                    int item03 = reader1.GetInt32(5);
                    string persent03 = reader1.GetString(6);
                    int item04 = reader1.GetInt32(7);
                    string persent04 = reader1.GetString(8);
                    int item05 = reader1.GetInt32(9);
                    string persent05 = reader1.GetString(10);
                    // ...

                    // Update the UI with the selected row data
                    textBox1.Text = item01.ToString();
                    textBox2.Text = item02.ToString();
                    textBox3.Text = item03.ToString();
                    textBox4.Text = item04.ToString();
                    textBox5.Text = item05.ToString();
                    textBox6.Text = persent01.ToString();
                    textBox7.Text = persent02.ToString();
                    textBox8.Text = persent03.ToString();
                    textBox9.Text = persent04.ToString();
                    textBox10.Text = persent05.ToString();
                    // ...
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // If file exist connect to database and show monster list.
            string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");

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

                string server = settings["server"];
                string username = settings["username"];
                string password = settings["password"];
                string database = settings["database"];

                string connectionString = $"Driver={{SQL Server}};Server={server};Database={database};Uid={username};Pwd={password};";


                string query = "SELECT sSid, strName FROM K_MONSTER WHERE strName Like '%" + textBox11.Text + "%'";

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        OdbcDataAdapter adapter = new OdbcDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set the DataGridView's DataSource property to the DataTable
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }
    }
}

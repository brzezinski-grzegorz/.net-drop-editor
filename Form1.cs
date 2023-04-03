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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Drop_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static bool ConfigFileExists()
        {
            string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");
            return File.Exists(configFilePath);
        }

        public static string[] GetDatabaseConnectionVariables()
        {
            string[] connectionVariables = new string[4];
            string configFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");
            if (File.Exists(configFile))
            {
                using (StreamReader reader = new StreamReader(configFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.StartsWith("server="))
                        {
                            connectionVariables[0] = line.Substring("server=".Length);
                        }
                        else if (line.StartsWith("database="))
                        {
                            connectionVariables[1] = line.Substring("database=".Length);
                        }
                        else if (line.StartsWith("username="))
                        {
                            connectionVariables[2] = line.Substring("username=".Length);
                        }
                        else if (line.StartsWith("password="))
                        {
                            connectionVariables[3] = line.Substring("password=".Length);
                        }
                    }
                }
            }
            
            return connectionVariables;

        }

        public static string CreateODBCConnectionString()
        {
            string[] connectionVariables = GetDatabaseConnectionVariables();
            string server = connectionVariables[0];
            string username = connectionVariables[2];
            string password = connectionVariables[3];
            string database = connectionVariables[1];

            string connectionString = "Driver={SQL Server};Server=" + server + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";";
            return connectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ConfigFileExists() == true)
            {
                // Get the connection string.
                string connectionStringItem = CreateODBCConnectionString();
                // Utilize ODBC driver.
                using (OdbcConnection connectionItem = new OdbcConnection(connectionStringItem))
                {
                    // Open connection.
                    connectionItem.Open();
                    // Create query.
                    string queryItem = "SELECT Num, strName From Item";
                    // Utilize ODBC command.
                    using (OdbcCommand commandItem = new OdbcCommand(queryItem, connectionItem))
                    {
                        OdbcDataAdapter adapterItem = new OdbcDataAdapter(commandItem);
                        DataTable dataTableItem = new DataTable();
                        adapterItem.Fill(dataTableItem);
                        dataGridView2.DataSource = dataTableItem;
                    }
                    // Close connection.
                    connectionItem.Close();
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

        // Load Drops into textboxes.
        private void button1_Click(object sender, EventArgs e)
        {
            // Get the connection string.
            string connectionStringDrops = CreateODBCConnectionString();

            string query1 = "SELECT * FROM K_MONSTER_ITEM WHERE sIndex = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            OdbcConnection connection1 = new OdbcConnection(connectionStringDrops);
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
            connection1.Close();
        }

        // Load Monsters from table.
        private void button2_Click(object sender, EventArgs e)
        {
            // Get the connection string.
            string connectionStringMonster = CreateODBCConnectionString();

            string queryMonster = "SELECT sSid, strName FROM K_MONSTER WHERE strName Like '%" + textBox11.Text + "%'";

            using (OdbcConnection connectionMonster = new OdbcConnection(connectionStringMonster))
            {
                connectionMonster.Open();

                using (OdbcCommand commandMonster = new OdbcCommand(queryMonster, connectionMonster))
                {
                    OdbcDataAdapter adapterMonster = new OdbcDataAdapter(commandMonster);
                    DataTable dataTableMonster = new DataTable();
                    adapterMonster.Fill(dataTableMonster);

                    // Set the DataGridView's DataSource property to the DataTable
                    dataGridView1.DataSource = dataTableMonster;
                }
            }
        }

        // Save Drops to table.
        private void button8_Click(object sender, EventArgs e)
        {
            // Get the connection string.
            string connectionStringUpdate = CreateODBCConnectionString();

            using (OdbcConnection connectionUpdate = new OdbcConnection(connectionStringUpdate))
            {
                try
                {
                connectionUpdate.Open();

                    // Update the row in the database with the value from the textbox
                    string sqlUpdate = "UPDATE K_MONSTER_ITEM SET" +
                    " iItem01 = '" + textBox1.Text + "'," +
                    " iItem02 = '" + textBox2.Text + "'," +
                    " iItem03 = '" + textBox3.Text + "'," +
                    " iItem04 = '" + textBox4.Text + "'," +
                    " iItem05 = '" + textBox5.Text + "'," +
                    " sPersent01 = '" + textBox6.Text + "'," +
                    " sPersent02 = '" + textBox7.Text + "'," +
                    " sPersent03 = '" + textBox8.Text + "'," +
                    " sPersent04 = '" + textBox9.Text + "'," +
                    " sPersent05 = '" + textBox10.Text + "' WHERE sIndex = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'"; // Change column1 and id to match the column and row you want to update
                    using (OdbcCommand commandUpdate = new OdbcCommand(sqlUpdate, connectionUpdate))
                    {
                    commandUpdate.ExecuteNonQuery();
                    MessageBox.Show("Drop updated succesfully.");
                }

                ClearTextboxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Inserting drops from Datagrid.
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }
        // Inserting drops from Datagrid.
        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }
        // Inserting drops from Datagrid.
        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }
        // Inserting drops from Datagrid.
        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }
        // Inserting drops from Datagrid.
        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }
        // Function to clear textboxes.
        private void ClearTextboxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "";
                }
            }
        }
    }
}

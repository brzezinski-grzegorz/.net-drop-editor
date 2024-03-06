using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data.Odbc;
using Label = System.Windows.Forms.Label;

namespace Drop_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Funciton to check if the config file exsists.
        public static bool ConfigFileExists()
        {
            string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.ini");
            return File.Exists(configFilePath);
        }
        // Function to retreive data from config file and parse it.
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
        // Function to create connection string to be used through entire app.
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
        // Main load of the form.
        private void Form1_Load(object sender, EventArgs e)
        {
            if (ConfigFileExists() != true)
            {
                MessageBox.Show("No configuration file found, you need to create one first.");
                // Hide main form.
                this.Hide();
                // Show form for configuration file creation.
                Form2 form2 = new Form2();
                // Show Form2
                form2.ShowDialog();
            }
        }
        // Load Monsters from table.
        private void button2_Click(object sender, EventArgs e)
        {
            // Get the connection string.
            string connectionString = CreateODBCConnectionString();
            // Query to get monsters from table.
            string queryMonster = "SELECT sSid, strName FROM K_MONSTER WHERE strName Like '%" + textBox11.Text + "%'";

            using (OdbcConnection connectionMonster = new OdbcConnection(connectionString))
            {
                connectionMonster.Open();

                using (OdbcCommand commandMonster = new OdbcCommand(queryMonster, connectionMonster))
                {
                    OdbcDataAdapter adapterMonster = new OdbcDataAdapter(commandMonster);
                    DataTable dataTableMonster = new DataTable();
                    // Populate datatable.
                    adapterMonster.Fill(dataTableMonster);
                    if (dataTableMonster.Rows.Count == 0)
                    {
                        // Show error message.
                        MessageBox.Show("Monster not found in the database!");
                    }
                    else
                    {
                        // View monsters in dataGrid.
                        dataGridView1.DataSource = dataTableMonster;
                    }
                    adapterMonster.Dispose();
                }
                connectionMonster.Close();
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

                connectionUpdate.Close();
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
                // Clear all of the textboxes after the update.
                if (control is TextBox)
                {
                    control.Text = "";
                }
            }


        }
        // Populating drop textboxes.
        // Once user uses Monster from first table it will automatically load it's drops.
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the connection string.
            string connectionStringDrops = CreateODBCConnectionString();
            // Create string to get monsters drops.
            string query1 = "SELECT * FROM K_MONSTER_ITEM WHERE sIndex = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            // Create string to get monster name based on dataviewgrid selection.
            string query2 = "SELECT strName FROM K_MONSTER WHERE sSid = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            // Open Connection.
            OdbcConnection connection1 = new OdbcConnection(connectionStringDrops);
            connection1.Open();
            // Get drops from Monsters.
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

            reader1.Close();
            connection1.Close();

            // Get data for monster name.
            OdbcConnection connection2 = new OdbcConnection(connectionStringDrops);
            connection2.Open();
            OdbcCommand command2 = new OdbcCommand(query2, connection2);
            OdbcDataAdapter adapterMonsterName = new OdbcDataAdapter(command2);
            DataTable dataTableMonsterName = new DataTable();
            // Populate datatable.
            adapterMonsterName.Fill(dataTableMonsterName);
            // Populate monster name.
            label16.Text = dataTableMonsterName.Rows[0]["strName"].ToString();
            connection2.Close();
        }
        // Search for Item.
        // Once button is pressed query will search through database for item named in textbox.
        private void button1_Click(object sender, EventArgs e)
        {
            // Get the connection string.
            string connectionString = CreateODBCConnectionString();
            // Query to get monsters from table.
            string queryMonster = "SELECT Num, strName FROM ITEM WHERE strName Like '%" + textBox12.Text + "%'";

            using (OdbcConnection connectionMonster = new OdbcConnection(connectionString))
            {
                connectionMonster.Open();

                using (OdbcCommand commandMonster = new OdbcCommand(queryMonster, connectionMonster))
                {
                    OdbcDataAdapter adapterMonster = new OdbcDataAdapter(commandMonster);
                    DataTable dataTableMonster = new DataTable();
                    // Populate datatable.
                    adapterMonster.Fill(dataTableMonster);
                    if (dataTableMonster.Rows.Count == 0)
                    {
                        // Show error message.
                        MessageBox.Show("Item not found in the database!");
                    }
                    else
                    {
                        // View monsters in dataGrid.
                        dataGridView2.DataSource = dataTableMonster;
                    }
                }
                connectionMonster.Close();
            }
        }
        // Populating Labels of item Preview.
        // Once item is selected in second table it will load it's data from table to labels.
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the connection string.
            string connectionString = CreateODBCConnectionString();
            // Query to get monsters from table.
            string queryItem = "SELECT * FROM ITEM WHERE Num = " + dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

            using (OdbcConnection connectionMonster = new OdbcConnection(connectionString))
            {
                connectionMonster.Open();

                using (OdbcCommand commandMonster = new OdbcCommand(queryItem, connectionMonster))
                {

                    // execute the command and get the data reader
                    OdbcDataReader reader = commandMonster.ExecuteReader();
                    // read the first row
                    if (reader.Read())
                    {
                        // set the label text to the value of the specific column
                        label14.Text = reader.GetInt32(6).ToString();
                        label40.Text = reader.GetInt32(10).ToString();
                        label41.Text = reader.GetInt32(36).ToString();
                        label42.Text = reader.GetInt32(37).ToString();
                        label43.Text = reader.GetInt32(38).ToString();
                        label44.Text = reader.GetInt32(39).ToString();
                        label45.Text = reader.GetInt32(42).ToString();
                        label46.Text = reader.GetByte(40).ToString();
                        label47.Text = reader.GetByte(41).ToString();
                        label48.Text = reader.GetInt32(43).ToString(); 
                        label49.Text = reader.GetInt32(50).ToString();
                        label50.Text = reader.GetInt32(51).ToString();
                        // Second Column
                        label51.Text = reader.GetInt16(13).ToString();
                        label52.Text = reader.GetInt16(30).ToString();
                        label53.Text = reader.GetInt16(31).ToString();
                        label54.Text = reader.GetInt16(32).ToString();
                        label55.Text = reader.GetInt16(33).ToString();
                        label56.Text = reader.GetInt16(34).ToString();
                        label57.Text = reader.GetInt16(35).ToString();
                        label58.Text = reader.GetInt16(45).ToString();
                        label59.Text = reader.GetInt16(46).ToString();
                        label60.Text = reader.GetInt16(47).ToString();
                        label61.Text = reader.GetInt16(48).ToString();
                        label62.Text = reader.GetInt16(49).ToString();

                        foreach (Control control in Controls)
                        {
                            if (control is Label)
                            {
                                Label label = (Label)control;

                                // Assuming the text represents an integer value
                                if (int.TryParse(label.Text, out int value) && value != 0)
                                {
                                    label.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    // Set color back to green for labels with value 0
                                    label.ForeColor = System.Drawing.Color.Green;
                                }
                            }
                        }

                    }

                }
                connectionMonster.Close();
            }
        }

    }
}

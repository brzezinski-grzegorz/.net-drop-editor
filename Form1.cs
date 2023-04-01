using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

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
            // Get the path to the application folder
            string app_folder_path = AppDomain.CurrentDomain.BaseDirectory;

            // Replace "file_name" with the name of the file you want to check for in the application folder
            string file_name = "config.ini";

            // Combine the application folder path and the file name to get the full file path
            string file_path = Path.Combine(app_folder_path, file_name);

            // Replace "variables_to_check" with an array of variables you want to check for in the file
            string[] variables_to_check = {"server", "user", "pass", "db_name" };

            // Check if the file exists
            if (File.Exists(file_path))
            {
                // Read the contents of the file
                StreamReader sr = new StreamReader(file_path);
                string file_contents = sr.ReadToEnd();
                sr.Close();

                // Check if all variables exist in the file
                bool all_variables_exist = true;
                foreach (string variable in variables_to_check)
                {
                    if (!file_contents.Contains(variable))
                    {
                        all_variables_exist = false;
                        break;
                    }
                }

                if (all_variables_exist)
                {
                    // Replace "server_name" with the name of your MSSQL server
                    string server_name = "YOUR_SERVER_NAME";

                    // Replace "database_name" with the name of your MSSQL database
                    string database_name = "YOUR_DATABASE_NAME";

                    // Replace "table_name" with the name of the table you want to retrieve data from
                    string table_name = "YOUR_TABLE_NAME";

                    // Replace "connection_string" with the connection string for your MSSQL server and database
                    string connection_string = "Server=" + server_name + ";Database=" + database_name + ";Trusted_Connection=True;";

                    // Create a connection to the database
                    SqlConnection connection = new SqlConnection(connection_string);

                    try
                    {
                        // Open the connection to the database
                        connection.Open();

                        // Replace "query" with the SQL query to retrieve data from the table
                        string query = "SELECT column1, column2, column3 FROM " + table_name;

                        // Create a command object with the SQL query and the connection to the database
                        SqlCommand command = new SqlCommand(query, connection);

                        // Execute the SQL query and retrieve the results
                        SqlDataReader reader = command.ExecuteReader();

                        // If the query returns any rows, retrieve the data and put it into textboxes
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // Replace "textbox1" with the name of the first textbox you want to put data into
                            // Replace "textbox2" with the name of the second textbox you want to put data into
                            // Replace "textbox3" with the name of the third textbox you want to put data into
                            string column1_value = reader.GetString(0);
                            string column2_value = reader.GetString(1);
                            string column3_value = reader.GetString(2);

                            TextBox textbox1 = new TextBox();
                            textbox1.Text = column1_value;

                            TextBox textbox2 = new TextBox();
                            textbox2.Text = column2_value;

                            TextBox textbox3 = new TextBox();
                            textbox3.Text = column3_value;

                            // Add the textboxes to a form or panel to display them to the user
                            // For example:
                            Form form = new Form();
                            form.Controls.Add(textbox1);
                            form.Controls.Add(textbox2);
                            form.Controls.Add(textbox3);
                            form.Show();
                        }
                        else
                        {
                            MessageBox.Show("No data was found in the table.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    finally
                    {
                        // Close the connection to the database
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("The file exists but does not contain all required variables.");
                }
            }
            else
            {
                MessageBox.Show("The file does not exist in the application folder.");
            }

            Application.Exit();
        }
    }
}

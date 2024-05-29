using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadClients();
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientsDB"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        private void LoadClients()
        {
            try
            {
                sqlConnection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Clients", sqlConnection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewClients.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Clients (Nom, Prenom, Numero) VALUES (@Nom, @Prenom, @Numero)", sqlConnection);
                command.Parameters.AddWithValue("@Nom", txtNom.Text);
                command.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                command.ExecuteNonQuery();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du client : " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dataGridViewClients.SelectedRows[0].Cells["Id"].Value);
                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand("UPDATE Clients SET Nom = @Nom, Prenom = @Prenom, Numero = @Numero WHERE Id = @Id", sqlConnection);
                    command.Parameters.AddWithValue("@Nom", txtNom.Text);
                    command.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                    command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                    command.Parameters.AddWithValue("@Id", clientId);
                    command.ExecuteNonQuery();
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification du client : " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier.");
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dataGridViewClients.SelectedRows[0].Cells["Id"].Value);
                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Clients WHERE Id = @Id", sqlConnection);
                    command.Parameters.AddWithValue("@Id", clientId);
                    command.ExecuteNonQuery();
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression du client : " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
            }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=PC-INFO\\SQLEXPRESS;Database=Louistest;User Id=sa;Password=Mdp56350;";

        public Form1()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Clients", sqlConnection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridViewClients.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message);
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Clients (Nom, Prenom, Numero) VALUES (@Nom, @Prenom, @Numero)", sqlConnection))
                    {
                        command.Parameters.AddWithValue("@Nom", txtNom.Text);
                        command.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                        command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                        command.ExecuteNonQuery();
                        LoadClients();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du client : " + ex.Message);
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(GetCellValue(dataGridViewClients.SelectedRows[0].Cells["Id"]));
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        using (SqlCommand command = new SqlCommand("UPDATE Clients SET Nom = @Nom, Prenom = @Prenom, Numero = @Numero WHERE Id = @Id", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@Nom", txtNom.Text);
                            command.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                            command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                            command.Parameters.AddWithValue("@Id", clientId);
                            command.ExecuteNonQuery();
                            LoadClients();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification du client : " + ex.Message);
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
                int clientId = Convert.ToInt32(GetCellValue(dataGridViewClients.SelectedRows[0].Cells["Id"]));
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM Clients WHERE Id = @Id", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@Id", clientId);
                            command.ExecuteNonQuery();
                            LoadClients();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression du client : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
            }
        }

        private object GetCellValue(DataGridViewCell cell)
        {
            return cell.Value == DBNull.Value ? null : cell.Value;
        }
    }
}

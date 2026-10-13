using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace grocery
{
    public partial class frmProduct : Form
    {
        private int selectedProductId = 0; // Tracks which product is selected for update/delete

        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string query = "SELECT ProductID, ProductName, UnitPrice, StockQuantity FROM Products";
                DataTable dt = db.ExecuteQuery(query);
                dgvProducts.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Unused event
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Unused event
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicked a valid row (not the header)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                // Save the ProductID into our private variable
                selectedProductId = Convert.ToInt32(row.Cells["ProductID"].Value);

                // Populate text boxes with selected row data
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();
                txtStockQuantity.Text = row.Cells["StockQuantity"].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            decimal price;
            int stock;

            if (string.IsNullOrEmpty(name) || !decimal.TryParse(txtUnitPrice.Text, out price) || !int.TryParse(txtStockQuantity.Text, out stock))
            {
                MessageBox.Show("Please enter valid product details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string query = "INSERT INTO Products (ProductName, UnitPrice, StockQuantity) VALUES (@Name, @Price, @Stock)";

                SqlParameter[] parameters = {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Stock", stock)
                };

                int rows = db.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Please select a product from the grid to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtProductName.Text.Trim();
            decimal price;
            int stock;

            if (string.IsNullOrEmpty(name) || !decimal.TryParse(txtUnitPrice.Text, out price) || !int.TryParse(txtStockQuantity.Text, out stock))
            {
                MessageBox.Show("Please enter valid product details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string query = "UPDATE Products SET ProductName = @Name, UnitPrice = @Price, StockQuantity = @Stock WHERE ProductID = @ID";

                SqlParameter[] parameters = {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Stock", stock),
                    new SqlParameter("@ID", selectedProductId)
                };

                int rows = db.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Please select a product from the grid to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper db = new DatabaseHelper();
                    string query = "DELETE FROM Products WHERE ProductID = @ID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ID", selectedProductId)
                    };

                    int rows = db.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            selectedProductId = 0;
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtStockQuantity.Clear();
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace grocery
{
    public partial class frmBuyerOrder : Form
    {
        private DataTable productTable;

        public frmBuyerOrder()
        {
            InitializeComponent();
        }

        private void frmBuyerOrder_Load(object sender, EventArgs e)
        {
            LoadProductCatalog();
        }

        private void LoadProductCatalog()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string query = "SELECT ProductID, ProductName, UnitPrice, StockQuantity FROM Products WHERE StockQuantity > 0";
                productTable = db.ExecuteQuery(query);

                cmbProducts.DataSource = productTable;
                cmbProducts.DisplayMember = "ProductName";
                cmbProducts.ValueMember = "ProductID";

                cmbProducts.SelectedIndex = -1;
                txtUnitPrice.Clear();
                txtTotalPay.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading catalog: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is DataRowView row)
            {
                decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                txtUnitPrice.Text = unitPrice.ToString("0.00");
                CalculateTotal();
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) &&
                int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0)
            {
                decimal total = unitPrice * quantity;
                txtTotalPay.Text = total.ToString("0.00");
            }
            else
            {
                txtTotalPay.Text = "0.00";
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedValue == null)
            {
                MessageBox.Show("Please select a product to order.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid order quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(cmbProducts.SelectedValue);

            DataRowView selectedRow = (DataRowView)cmbProducts.SelectedItem;
            int availableStock = Convert.ToInt32(selectedRow["StockQuantity"]);

            if (quantity > availableStock)
            {
                MessageBox.Show($"Sorry, only {availableStock} items are available in stock.", "Stock Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string updateStockQuery = "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductID = @ID";

                SqlParameter[] stockParams = {
                    new SqlParameter("@Qty", quantity),
                    new SqlParameter("@ID", productId)
                };

                db.ExecuteNonQuery(updateStockQuery, stockParams);

                MessageBox.Show($"Order placed successfully! Total Pay: {txtTotalPay.Text}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadProductCatalog();
                txtQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message, "Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
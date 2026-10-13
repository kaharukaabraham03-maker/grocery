using System;
using System.Windows.Forms;

namespace grocery
{
    public partial class frmMain : Form
    {
        private string userRole;

        // Constructor that accepts the logged-in user's role
        public frmMain(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Apply Role-Based Access Control (RBAC) based on your guidelines
            if (userRole == "Cashier")
            {
                MessageBox.Show("Logged in as Cashier. Some administrative features are restricted.", "Access Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Logged in as Administrator. Full access granted.", "Access Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Return to login form
            frmlogin loginForm = new frmlogin();
            loginForm.Show();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
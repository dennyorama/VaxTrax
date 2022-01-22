using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VaxTrax_2._0_
{
    // Author: Dennis Steven Dyer II
    //   Date: 10/13/2021
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public static string UserName;
        public static string Password;

        private void loginButton_Click(object sender, EventArgs e)
        {
            User newUser = new User();
            newUser.UserName = userIDTextBox.Text.Trim();
            newUser.Password = passwordTextBox.Text.Trim();

            UserName = newUser.UserName;
            Password = newUser.Password;

            try
            {
                bool status = LoginMethod.IsValid(newUser.UserName, newUser.Password);

                if (status)
                {
                    MessageBox.Show("Login Good! Welcome " + newUser.UserName);

                    MainForm newMainForm = new MainForm();

                    newMainForm.Show();

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Bad");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.IdentityModel.Tokens;

namespace LEALANG_V0
{
    public partial class SignUp : Form
    {
        int curloop = 6;
        SQLiteConnection conn;
        DatabaseFunctions DBFuncs = new DatabaseFunctions();
        bool check=false;
        bool secondcheck = false;
        public SignUp()
        {
            InitializeComponent();
            
            comboBox1.DataSource = DBFuncs.GetLangs(); // addding the data to combo box to allow me to add more languages.

        }

        //on button click
        private void button1_Click(object sender, EventArgs e)
        {
            //goes from 1 to 5
            for (int i = 1; i < 6; i++)
            {
                Control lab = this.Controls["label" + i.ToString()]; //get the control with the name of label
                lab.Visible = false; 
                lab.Text = "This field is required";
                check = false;
            }
            foreach ( TextBox textbox in this.Controls.OfType<TextBox>()) //checking each textbox in the form
            {
                curloop--;
                if (String.IsNullOrEmpty(textbox.Text)) //if the textbox its currently on is empty or null
                {
                    Control lab = this.Controls["label" + curloop.ToString()];
                    lab.Visible = true;
                    check = true;
                }
            }
            curloop = 6;
            if (check == false)
            {
                if (!(String.IsNullOrEmpty(DBFuncs.DBReadValstr("SELECT * FROM users WHERE Username = @username", textBox3.Text.ToLower(), "@username", "Username")))){ //checking if the user exists
                    label3.Text = "Username is already in use!";
                    label3.Visible = true;
                    secondcheck = true;
                }
                if (!(String.IsNullOrEmpty(DBFuncs.DBReadValstr("SELECT * FROM users WHERE Email = @email", textBox2.Text.ToLower(), "@email", "Email")))){ //checks if the email is in use
                    label2.Text = "Email is already in use!";
                    label2.Visible = true;
                    secondcheck = true;
                }
                if (textBox5.Text != textBox4.Text) //checks if the two inputs for the passwords match
                {
                    label5.Text = "Passwords do not match!";
                    label5.Visible = true;
                    secondcheck = true;
                }
                if (textBox1.Text.Split(' ').Count() != 2) //seeing if there are two seperate strings in the fullname textbox
                {
                    label1.Text = "Please enter full name as FIRSTNAME LASTNAME";
                    label1.Visible = true;
                    secondcheck = true;
                }
                if (secondcheck == false)
                {
                    try
                    {
                        DBFuncs.makenewuser(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, comboBox1.Text);
                        MessageBox.Show("Thank you for creating an account, " + textBox1.Text.Split(' ')[0].ToString());
                    }
                    catch ( Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Unexpected error has occured! Please try again!");
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LogIn().ShowDialog();
            this.Close();
        }

    }
}

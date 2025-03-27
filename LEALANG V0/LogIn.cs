using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace LEALANG_V0
{
    
    public partial class LogIn : Form
    {

        //basic login variables that are needed
        string uname;
        string pword;

        //To track userID
        int userID;
        int langID;
        string lang;

        string Query;
        string ValLoc;
        DatabaseFunctions DBfuncs = new DatabaseFunctions();

        bool YN = false;
        private void UserLanguageID()
        {
            Query = "SELECT * FROM userlang WHERE UserID = @userid";
            ValLoc = "@userid";
            langID = Convert.ToInt32(DBfuncs.DBReadValint(Query, userID.ToString(), ValLoc,"LangID"));
        }
        private void LanguageChosen()
        {
            Query = "SELECT * FROM languages WHERE langID = @langid";
            ValLoc = "@langid";
            lang = DBfuncs.DBReadValstr(Query, langID.ToString(), ValLoc,"Language");
        }

        public LogIn()
        {
            InitializeComponent();

        }

        //checks the details entered to see if they can log in
        private void button2_Click(object sender, EventArgs e)
        {
            uname = textBox1.Text;
            pword = textBox2.Text;;
            YN = DBfuncs.PassCheck(uname, pword); //Boolean value by default is false
            if (YN == true)
            {
                userID = Convert.ToInt32(DBfuncs.DBReadValint("SELECT * FROM users WHERE Username = @uname",uname.ToLower(),"@uname","UserID")); //figure out userid
                UserLanguageID(); // figure out the userlanguage id
                LanguageChosen(); // figure out the chosen language

                this.Hide();
                new Home(lang, userID.ToString()).ShowDialog(); //open home with details we figured out
                this.Close();
            }
            else if (YN == false)
            {

                MessageBox.Show("Incorrect Details!");
            }
        }


        //opens The password forget form when clicked
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new PasswordForget().ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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
        
        //gets the users languageid
        private void UserLanguageID()
        {
            Query = "SELECT * FROM userlang WHERE UserID = @userid";
            ValLoc = "@userid";
            langID = Convert.ToInt32(DBfuncs.DBReadValint(Query, userID.ToString(), ValLoc,"LangID"));
        }
        //sees what the language is based on language id
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

                //checks if the user is in a valid range for updating their daily streak
                int streak = DBfuncs.DBReadValint("SELECT * FROM userpoints WHERE UserID = @userid", userID.ToString(), "@userid", "Streak");
                string log = DBfuncs.LastLoginCheck(userID.ToString());
                
                if (log == "false") //if theyre not
                {
                    DBfuncs.StreakChange(userID.ToString(), 0); //sets it to 0
                }

                this.Hide();
                new Home(lang, userID.ToString()).ShowDialog(); //open home with details we figured out
                this.Close();
            }
            else if (YN == false)
            {

                MessageBox.Show("Incorrect Details!");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SignUp().ShowDialog();
            this.Close();
        }

        //Show password on button click
        private void button3_Click(object sender, EventArgs e)
        {
            switch (textBox2.UseSystemPasswordChar)
            {
                case true:
                    textBox2.UseSystemPasswordChar = false;
                    button3.Text = "👁";
                    break;
                case false:
                    textBox2.UseSystemPasswordChar = true;
                    button3.Text = "-";
                    break;
            }


        }
    }
}

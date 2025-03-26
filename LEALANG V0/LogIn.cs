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
        //Sqlite Variables to use. Pretty self explanitory 
        SqliteConnection conn;
        SqliteCommand cmd;
        SqliteDataReader read;

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
            //Making the connection to a new data source
            conn = new SqliteConnection("Data Source=LEALANG.db");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uname = textBox1.Text;
            pword = textBox2.Text;;
            YN = DBfuncs.PassCheck(uname, pword);
            if (YN == true)
            {
                userID = Convert.ToInt32(DBfuncs.DBReadValint("SELECT * FROM users WHERE Username = @uname",uname.ToLower(),"@uname","UserID"));
                UserLanguageID();
                LanguageChosen();

                this.Hide();
                new Home(lang, userID.ToString()).ShowDialog();
                this.Close();
            }
            else if (YN == false)
            {

                MessageBox.Show("Incorrect Details!");
            }
        }
    }
}

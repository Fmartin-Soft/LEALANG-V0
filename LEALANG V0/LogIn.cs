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
        private void UserLanguageID()
        {
            Query = "SELECT * FROM userlang WHERE UserID = @userid";
            ValLoc = "@userid";
            read = DBfuncs.DBReadVal(Query, userID.ToString(), ValLoc);
            while (read.Read())
            {
                langID = Convert.ToInt32(read["LangID"]);
            }
            conn.Close();

        }
        private void LanguageChosen()
        {

            Query = "SELECT * FROM languages WHERE langID = @langid";
            ValLoc = "@langid";
            read = DBfuncs.DBReadVal(Query, langID.ToString(), ValLoc);
            while (read.Read())
            {
                lang = read["Language"].ToString();
            }
            conn.Close();
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
            pword = textBox2.Text;
            Query = "SELECT * FROM users WHERE Username = @uname";
            ValLoc = "@uname";
            read = DBfuncs.DBReadVal(Query, uname, ValLoc);
            //While its reading do stuff
            while (read.Read())
            {
                //In this instant the text between the [] is the table column name
                if (read["Password"].ToString() == pword)
                {
                    userID = Convert.ToInt32(read["UserID"]);
                    conn.Close();
                    UserLanguageID();
                    LanguageChosen();
                    MessageBox.Show(lang);
                    read.Close();
                    conn.Close();
                    new Home(lang,0,userID.ToString()).ShowDialog();
                    this.Hide();
                    this.Close();
                    
                }
            }
            MessageBox.Show("Incorrect Details!");
        }
    }
}

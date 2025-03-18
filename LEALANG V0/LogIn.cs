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

        private void UserLanguageID()
        {
            conn.Open();
            cmd = new SqliteCommand("SELECT * FROM userlang WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", userID);
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                langID = Convert.ToInt32(read["LangID"]);
            }
            conn.Close();

        }
        private void LanguageChosen()
        {


            cmd = new SqliteCommand("SELECT * FROM languages WHERE langID = @langid",conn);
            cmd.Parameters.AddWithValue("@langid", langID);
            conn.Open();
            read = cmd.ExecuteReader();
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
            //SQLite connection commands
            //Opens the connection, allowing us to manipulate the DB
            conn.Open();
            //Specifying the command to use. In this it is Selecting all from the table of users wherever the Username is "uname"
            cmd = new SqliteCommand("SELECT * FROM users WHERE Username = @uname", conn);
            //Specifies what uname is, in this case its the username the user entered
            cmd.Parameters.AddWithValue("@uname", uname);
            //Reads the result
            read = cmd.ExecuteReader();
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
                    new Home(lang).ShowDialog();
                    this.Hide();
                    this.Close();
                    
                }
            }
            MessageBox.Show("Incorrect Details!");
        }
    }
}

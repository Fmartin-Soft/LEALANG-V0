using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace LEALANG_V0
{
    public partial class Home : Form
    {
        string ChosenLang;
        string ChosenDif;
        string Name;
        string Query;
        string ValLoc;
        string LangDif;
        SqliteDataReader read;
        SqliteConnection conn;
        DatabaseFunctions DBFuncs = new DatabaseFunctions();
        public Home(string LangChosen,int score= 0,string nameid="1")
        {
            
            ChosenLang = LangChosen;
            InitializeComponent();
            Query = "SELECT * FROM users WHERE UserID = @userID";
            ValLoc = "@userID";
            read = DBFuncs.DBReadVal(Query, nameid, ValLoc);
            while (read.Read())
            {
                Name = read["FirstName"].ToString();
            }
            label2.Text = "Welcome: " + Name;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            

            this.Hide();
            new Quiz(ChosenLang).ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

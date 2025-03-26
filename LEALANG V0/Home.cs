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
        string nameID;
        SqliteDataReader read;
        DatabaseFunctions DBFuncs = new DatabaseFunctions();
        public Home(string LangChosen, string nameid , int score= 0)
        {
            nameID = nameid;
            ChosenLang = LangChosen;
            InitializeComponent();
            if (score > 0)
            {
                score = DBFuncs.GetScore(score, nameID);
                DBFuncs.UpdateScore(score, nameID);

            }
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
            new Quiz(ChosenLang,nameID).ShowDialog();
            this.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

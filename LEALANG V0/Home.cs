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
        int streak;
        string lastlogday;
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
            score = DBFuncs.DBReadValint("SELECT * FROM userpoints WHERE UserID = @userid",nameid,"userid","Points");
            label3.Text = "Total Correct Answers: " + score;
            streak = DBFuncs.DBReadValint("SELECT * FROM userpoints WHERE UserID = @userid", nameid, "@userid", "Streak");
            lastlogday = DBFuncs.LastLoginCheck(nameid);
            if (lastlogday != "neut")
            {
                //If the user hasnt updated their streak set the streak image to the off one?
                pictureBox1.Image = Properties.Resources.Fireless;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.Fire;
            }
            label4.Text = "Streak: " + streak + " Days!";
            Query = "SELECT * FROM users WHERE UserID = @userID";
            ValLoc = "@userID";
            Name = DBFuncs.DBReadValstr(Query, nameid, ValLoc, "FirstName");
            Name = Char.ToUpper(Name[0]) + Name.Substring(1); //just makes the name go from name to Name
            label2.Text = "Welcome back " + Name + "!";

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

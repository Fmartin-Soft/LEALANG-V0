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

        //when the home form is generated
        public Home(string LangChosen, string nameid , int score= 0)
        {

            
            nameID = nameid;
            ChosenLang = LangChosen;
            InitializeComponent();
            
            if (score > 0) //sees if they need to update the score of the user, no point if they score 0 (or if they somehow score under)
            {
                score = DBFuncs.GetScore(score, nameID);
                DBFuncs.UpdateScore(score, nameID);
            }
            score = DBFuncs.DBReadValint("SELECT * FROM userpoints WHERE UserID = @userid",nameid,"userid","Points"); //gets the score anyway
            label3.Text = "Total Correct Answers: " + score; 
            streak = DBFuncs.DBReadValint("SELECT * FROM userpoints WHERE UserID = @userid", nameid, "@userid", "Streak"); //gets the users name
            lastlogday = DBFuncs.LastLoginCheck(nameid);
            if (lastlogday != "neut")
            { 
                pictureBox1.Image = Properties.Resources.Fireless; //If the user hasnt updated their streak set the streak image to the off one?
            }
            else
            {
                pictureBox1.Image = Properties.Resources.Fire; //if the user has updated their streak then 
            }
            label4.Text = "Streak: " + streak + " Days!"; 
            Query = "SELECT * FROM users WHERE UserID = @userID";
            ValLoc = "@userID";
            Name = DBFuncs.DBReadValstr(Query, nameid, ValLoc, "FirstName");
            Name = Char.ToUpper(Name[0]) + Name.Substring(1); //just makes the name go from "name" to "Name"
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

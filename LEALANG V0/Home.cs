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
        SqliteDataReader read;
        SqliteConnection conn;
        DatabaseFunctions DBFuncs = new DatabaseFunctions();
        public Home(string LangChosen,int score= 0,string nameid="Default")
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
            label1.Text = "Welcome:" + Name;
        }
    }
}

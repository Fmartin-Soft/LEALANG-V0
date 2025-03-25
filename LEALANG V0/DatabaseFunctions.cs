using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace LEALANG_V0
{
    public partial class DatabaseFunctions
    {
        //Sqlite Variables to use. Pretty self explanitory 
        SqliteConnection conn;
        SqliteCommand cmd;
        SqliteCommand cmd1;
        SqliteDataReader read;

        int point;
        int IntegerPassed;
        string StringPassed;
        public string DBReadValint(string Query, string Value, string ValLoc, string ReadLoc)
        {
            //specifying the connection.
            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query is specified in code
            cmd = new SqliteCommand(Query, conn);
            //Specifies what the missing entry is in the query
            cmd.Parameters.AddWithValue(ValLoc, Value);
            //Reads the result
            read = cmd.ExecuteReader();
            //Returns Read to be able to manipulate it and read what we want after the query
            while (read.Read())
            {
                IntegerPassed = Convert.ToInt32(read[ReadLoc]);
            }
            return IntegerPassed.ToString();
        }
        public string DBReadValstr(string Query, string Value, string ValLoc, string ReadLoc)
        {
            //specifying the connection.
            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query is specified in code
            cmd = new SqliteCommand(Query, conn);
            //Specifies what the missing entry is in the query
            cmd.Parameters.AddWithValue(ValLoc, Value);
            //Reads the result
            read = cmd.ExecuteReader();
            //Returns Read to be able to manipulate it and read what we want after the query
            while (read.Read())
            {
                StringPassed = read[ReadLoc].ToString();
            }
            return StringPassed;
        }

        public SqliteDataReader DBReadVal(string Query, string Value, string ValLoc)
        {
            //specifying the connection.
            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query is specified in code
            cmd = new SqliteCommand(Query, conn);
            //Specifies what the missing entry is in the query
            cmd.Parameters.AddWithValue(ValLoc, Value);
            //Reads the result
            read = cmd.ExecuteReader();
            return read;

        }
        public SqliteDataReader DBRead(string Query)
        {
            //specifying the connection.
            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query is specified in code
            cmd = new SqliteCommand(Query, conn);
            //Reads the result
            read = cmd.ExecuteReader();
            //Returns Read to be able to manipulate it and read what we want after the query
            return read;
        }

        public int GetScore(int score,string userID)
        {

            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query here is specifying to select the column of points where the userID is the one specified
            cmd = new SqliteCommand("SELECT * FROM userpoints WHERE UserID = @userID", conn);
            //Adding the value into the query
            cmd.Parameters.AddWithValue("@userID", userID);
            //Reads the result
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                point = Convert.ToInt32(read["Points"]);
            }
            point += score;
            conn.Close();
            return point;
        }
        public void UpdateScore(int score, string userID)
        {
            try
            {
                conn = new SqliteConnection("Data Source=LEALANG.db");
                conn.Open();
                cmd1 = new SqliteCommand("UPDATE userpoints SET Points = @points WHERE UserID = @userID", conn);
                cmd1.Parameters.AddWithValue("@points", score);
                cmd1.Parameters.AddWithValue("@userID", userID);
                cmd1.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

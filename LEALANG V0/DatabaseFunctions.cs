using System;
using System.Collections.Generic;
using System.IdentityModel;
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

        //Values to return. C# doesnt like it if theyre all local to the functions :/
        int point;
        int IntegerPassed;
        string StringPassed;
        bool yn = false;
        DateTime lastloginday;
        string check;
        public int DBReadValint(string Query, string Value, string ValLoc, string ReadLoc)
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
            return IntegerPassed;
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

        //this is for the login form. Returns True if the password has been found
        public bool PassCheck(string Uname, string Pword)
        {
            //specifying the connection. Only use it while using the "using" method.
            using (conn = new SqliteConnection("Data Source=LEALANG.db"))
            {
                conn.Open();
                //Specifying the command to use. The Query is specified in code
                cmd = new SqliteCommand("SELECT * FROM users WHERE Username = @uname", conn);
                //Specifies what the missing entry is in the query
                cmd.Parameters.AddWithValue("@uname", Uname.ToLower());
                //Reads the result
                using (read = cmd.ExecuteReader())
                {
                    //While its reading do stuff
                    while (read.Read())
                    {
                        //In this instant the text between the [] is the table column name
                        if (read["Password"].ToString() == Pword)
                        {

                            yn = true;
                        }

                    }
                }

                return yn;
            }

        }

        //This function gets the score from the database
        public int GetScoreBase(string userID)
        {

            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            cmd = new SqliteCommand("SELECT * FROM userpoints WHERE UserID = @userID", conn); //Specifying the command to use. The Query here is specifying to select the column of points where the userID is the one specified

            cmd.Parameters.AddWithValue("@userID", userID); //Adding the value into the query

            read = cmd.ExecuteReader(); //Reads the result

            while (read.Read()) //While theres rows to read
            {
                point = Convert.ToInt32(read["Points"]); //Get the users total score info
            }
            conn.Close();
            return point; // return it 
        }


        //This function gets the score from the database and adds the user current score
        public int GetScore(int score,string userID)
        {

            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            cmd = new SqliteCommand("SELECT * FROM userpoints WHERE UserID = @userID", conn); //Specifying the command to use. The Query here is specifying to select the column of points where the userID is the one specified
            
            cmd.Parameters.AddWithValue("@userID", userID); //Adding the value into the query
            
            read = cmd.ExecuteReader(); //Reads the result

            while (read.Read()) //While theres rows to read
            {
                point = Convert.ToInt32(read["Points"]); //Get the users total score info
            }
            point += score; // add it to the score the user got
            conn.Close();
            return point; // return it 
        }

        //this updates the score
        public void UpdateScore(int score, string userID)
        {
            try // try and catch command
            {
                conn = new SqliteConnection("Data Source=LEALANG.db"); //db connection statement
                conn.Open(); //open the db
                cmd = new SqliteCommand("UPDATE userpoints SET Points = @points WHERE UserID = @userID", conn); //use a query to make a command
                cmd.Parameters.AddWithValue("@points", score); //adding the values into the query
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.ExecuteNonQuery(); //execute it. This updates the score
            }
            catch (Exception ex) // catching exceptions, hopefully never happens 
            {
                MessageBox.Show(ex.Message); // show exception
            }
        }
        

        //checks when the last login was
        public string LastLoginCheck(string userID)
        {
            using (conn = new SqliteConnection("Data Source = LEALANG.db")) //testing the using thing
            {
                conn.Open();
                cmd = new SqliteCommand("SELECT * FROM userpoints WHERE UserID = @userid",conn); // calling the table
                cmd.Parameters.AddWithValue("@userid", userID);
                read = cmd.ExecuteReader();
                
                    while (read.Read())
                    {
                        lastloginday = Convert.ToDateTime(read["LastDay"]); // where the last day the use logged in was
                    }
                
               
                if (lastloginday.AddDays(1) == DateTime.Today) 
                {
                    check = "true"; 
                }
                else if (lastloginday == DateTime.Today)
                {
                    check = "neut"; //use of string as neutral value needed
                }
                else
                {
                    check = "false";
                }
            }
            return check;
        }
        public void StreakChange(string userID, int streak)
        {
            try // try and catch command
            {
                conn = new SqliteConnection("Data Source=LEALANG.db"); //db connection statement
                conn.Open(); //open the db
                cmd = new SqliteCommand("UPDATE userpoints SET Streak = @streak WHERE UserID = @userID", conn); //use a query to make a command
                cmd.Parameters.AddWithValue("@streak", streak); //adding the values into the query
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.ExecuteNonQuery(); //execute it. This updates the score
            }
            catch (Exception ex) // catching exceptions, hopefully never happens 
            {
                MessageBox.Show(ex.Message); // show exception
            }
        }
        public void UpdateDay(string userID)
        {
            try // try and catch command
            {   
                conn = new SqliteConnection("Data Source=LEALANG.db"); //db connection statement
                conn.Open(); //open the db
                cmd = new SqliteCommand("UPDATE userpoints SET LastDay = @login WHERE UserID = @userID", conn); //use a query to make a command
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@login", DateTime.Today.ToString());
                cmd.ExecuteNonQuery(); //execute it. This updates the score
            }
            catch (Exception ex) // catching exceptions, hopefully never happens 
            {
                MessageBox.Show(ex.Message); // show exception
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        List<string> langs = new List<string>();
        long row;
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
            StringPassed = null;
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

        public List<string> GetLangs()
        {

            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            cmd = new SqliteCommand("SELECT * FROM languages", conn); //Specifying the command to use.

            read = cmd.ExecuteReader(); //Reads the result

            while (read.Read()) //While theres rows to read
            {
                langs.Add(read["Language"].ToString());  //adding each language found to the list
            }
            conn.Close();
            return langs; // return it 
        }


        //This makes a completely new user and inserts the appropriate data into the right tables
        public void makenewuser(string fullname, string username, string email, string password, string lang)
        {
            int langid = DBReadValint("SELECT * FROM languages WHERE Language = @lang", lang, "@lang", "LangID"); //Gettting the langID so I can insert info into userlang
            //This is the the users table
            using (conn = new SqliteConnection("Data Source = LEALANG.db")) //using a using so i can do multiple connectinos
            {
                conn.Open();
                using (cmd = new SqliteCommand("INSERT INTO users (FirstName, LastName, Username, Password, Email) VALUES (@firstname, @lastname, @username, @password, @email); SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@firstname", fullname.Split(' ')[0].ToLower());
                    cmd.Parameters.AddWithValue("@lastname", fullname.Split(' ')[1].ToLower());
                    cmd.Parameters.AddWithValue("@username", username.ToLower());
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@email", email.ToLower());
                    row = (long)cmd.ExecuteScalar(); // this just gives me the result of the CMD. so execute scalar gives me a single output, which would be the last row
                }
            }
            //this is for userlang
            using (conn = new SqliteConnection("Data Source = LEALANG.db")) //using a using so i can do multiple connectinos
            {
                conn.Open();
                using (cmd = new SqliteCommand("INSERT INTO userlang (LangID,UserID) VALUES (@langid, @userid)", conn))
                {
                    cmd.Parameters.AddWithValue("@langid", langid);
                    cmd.Parameters.AddWithValue("@userid", row);
                    cmd.ExecuteNonQuery();
                }
            }
            //this is for userpoints
            using (conn = new SqliteConnection("Data Source = LEALANG.db")) //using a using so i can do multiple connectinos
            {
                conn.Open();
                using (cmd = new SqliteCommand("INSERT INTO userpoints (UserID,Points,Streak,LastDay) VALUES (@userid, @points, @streak, @lastday)", conn))
                {
                    cmd.Parameters.AddWithValue("@userid", row);
                    cmd.Parameters.AddWithValue("@points", 0);
                    cmd.Parameters.AddWithValue("@streak", 0);
                    cmd.Parameters.AddWithValue("@lastday", DateTime.Today.AddDays(-1));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}

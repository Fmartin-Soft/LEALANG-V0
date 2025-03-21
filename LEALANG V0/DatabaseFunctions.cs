using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace LEALANG_V0
{
    public partial class DatabaseFunctions
    {
        //Sqlite Variables to use. Pretty self explanitory 
        SqliteConnection conn;
        SqliteCommand cmd;
        SqliteDataReader read;

        public SqliteDataReader DBReadVal(string Query, string Value, string ValLoc)
        {
            //specifying the connection.
            conn = new SqliteConnection("Data Source=LEALANG.db");
            conn.Open();
            //Specifying the command to use. The Query is specified in code
            cmd = new SqliteCommand(Query,conn);
            //Specifies what the missing entry is in the query
            cmd.Parameters.AddWithValue(ValLoc,Value);
            //Reads the result
            read = cmd.ExecuteReader();
            //Returns Read to be able to manipulate it and read what we want after the query
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
    }
}

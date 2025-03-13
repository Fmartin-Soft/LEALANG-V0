using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LEALANG_V0
{
    internal class MakeDatabase
    {
        String db = "CREATE DATABASE userinfo ON PRIMARY"+"(NAME = "
        SqlConnection con = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Ado.net_Tutorial
{
    class SqlCommandDemo
    {

        public string commandTest()
        {
            string results = "";
            SqlConnection conn = new SqlConnection(
                "Data Source=192.168.40.151;Initial Catalog=Northwind;persist security info=True;user id=sa;password=1black3#;MultipleActiveResultSets=True;");

            SqlCommand cmd = new SqlCommand("select CategoryName from Categories", conn);

            // 2. Call Execute reader to get query results
            SqlDataReader rdr = cmd.ExecuteReader();


            // prepare Insert command string

            /*The SqlCommand instantiation is just a little different from what you’ve seen before, but it is basically the same. 
             * Instead of a literal string as the first parameter of the SqlCommand constructor, we are using a variable, insertString. 
             * The insertString variable is declared just above the SqlCommand declaration.*/
            string insertString = @"
                insert into Categories
                (CategoryName, Description)
                values ('Miscellaneous', 'Whatever doesn''t fit elsewhere')";

            // 1. Instantiate a new command with a query and connection
            SqlCommand cmd1 = new SqlCommand(insertString, conn);

            // 2. Call ExecuteNonQuery to send command
            cmd1.ExecuteNonQuery();


            // prepare command string
            string updateString = @"
                 update Categories
                 set CategoryName = 'Other'
                 where CategoryName = 'Miscellaneous'";

            /*Again, we put the SQL command into a string variable, but this time we used a different SqlCommand constructor that takes only the command. 
             * In step 2, we assign the SqlConnection object, conn, to the Connection property of the SqlCommand object, cmd.
             * It demonstrates that you can change the connection object assigned to a command at any time.
             */

            // 1. Instantiate a new command with command text only
            SqlCommand cmdUpdate = new SqlCommand(updateString);

            // 2. Set the Connection property
            cmdUpdate.Connection = conn;

            // 3. Call ExecuteNonQuery to send command
            cmdUpdate.ExecuteNonQuery();



            // prepare command string
            string deleteString = @"
                 delete from Categories
                 where CategoryName = 'Other'";

            // 1. Instantiate a new command
            SqlCommand cmdDelete = new SqlCommand();

            // 2. Set the CommandText property
            cmdDelete.CommandText = deleteString;

            // 3. Set the Connection property
            cmdDelete.Connection = conn;

            // 4. Call ExecuteNonQuery to send command
            cmdDelete.ExecuteNonQuery();

            return results;
        }

    }
}

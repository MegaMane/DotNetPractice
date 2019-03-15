using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Ado.net_Tutorial
{
    class SqlConnectionDemo
    {
        public String Connect()
        {
            String customerList = "";
            // 1. Instantiate the connection
            SqlConnection conn = new SqlConnection(
            "Data Source=192.168.40.151;Initial Catalog=Northwind;persist security info=True;user id=sa;password=1black3#;MultipleActiveResultSets=True;");

            SqlDataReader rdr = null;

            try
            {
                // 2. Open the connection
                conn.Open();

                // 3. Pass the connection to a command object
                SqlCommand cmd = new SqlCommand("select * from Customers", conn);

                //
                // 4. Use the connection
                //

                // get query results
                rdr = cmd.ExecuteReader();

                // print the CustomerID of each record
                while (rdr.Read())
                {
                    customerList += (rdr[0]) + "|";
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return customerList;
        }
    }
}

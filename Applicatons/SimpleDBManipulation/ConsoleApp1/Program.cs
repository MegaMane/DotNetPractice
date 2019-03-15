using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;



namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlDataReader rdr = null;
            string results = "";
            SqlConnection conn = new SqlConnection(
                "Data Source=172.30.0.68;Initial Catalog=Fanatics_OpsuiteDB;persist security info=True;user id=sa;password=1black3#;MultipleActiveResultSets=True;");

            SqlCommand cmd = new SqlCommand(@"SELECT MessageLogId, 
                                                     RequestMessage,
                                                     Processed
                                                FROM dbo.OpSync2_MessageLog
                                                WHERE LocationId = 561
                                                AND ErrorMessage LIKE '%Store update,%'
                                                AND Date < '20180620'
                                                AND Processed = 0
                                                And MessageLogId < 10927
                                                ORDER BY MessageLogId DESC", conn);

            try
            {
                // open the connection
                conn.Open();

                rdr = cmd.ExecuteReader();

                int rowCount = 1;

                while (rdr.Read())
                {
                    // get the results of each column
                    int MessageLogId = (int) rdr["MessageLogId"];
                    string RequestMessage = Regex.Replace((string)rdr["RequestMessage"], @"""PriceSourceId"":""\d+""", "\"PriceSourceId\":\"1\"");
                    string Processed = rdr["Processed"].ToString();

                    Console.WriteLine(MessageLogId);
                    Console.WriteLine(rowCount);
                    

                    cmd = new SqlCommand(@"UPDATE dbo.OpSync2_MessageLog
                                            SET RequestMessage = @RequestMessage
                                                WHERE LocationId = 561
                                                AND ErrorMessage LIKE '%Store update,%'
                                                AND Date < '20180620'
                                                AND Processed = 0
                                                AND MessageLogId = @MessageLogID
                                                ", conn);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@RequestMessage";
                    param1.Value = RequestMessage;

                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName =  "@MessageLogID";
                    param2.Value = MessageLogId;

                    cmd.Parameters.Add(param2);

                    cmd.ExecuteNonQuery();

                    rowCount += 1;
                    // print out the results
                    //Console.Write("{ 0,-25}", MessageLogId);
                    //Console.Write( RequestMessage);
                    //Console.Write("{0,-25}", Processed);
                    //Console.WriteLine();
                    //Console.ReadLine();
                }
            }
        
            finally
            {
                // 3. close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        
        }
    }
}

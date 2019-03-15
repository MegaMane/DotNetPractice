using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using CsvHelper;
using System.IO;




namespace FanaticsPreprocessor
{
    class CSV
    {



        public static List<String[]> readFile (string file, char delimiter = ',', bool quotedFields = true)
        {
            int counter = 0;
            List<string[]> records = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(file))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter.ToString());
                parser.HasFieldsEnclosedInQuotes = quotedFields;
        

                //parser.Delimiters = new string[] { "," };

                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    records.Add(fields);
                    //Console.WriteLine("{0} field(s)", fields.Length);
                    counter++;

                }



            }

            return records;

        }
            


        public static void WriteFile(List<string[]> records, string outPutFilePath)
        {
            //if body is valid
            //Write out new file(overwrite) with stripped headers and quoted fields
            //must make the input here an array of strings after processing
            //string[] output = records.ToArray();
            //System.IO.File.WriteAllLines(@"C:\testFTP\Fanatics\Imports\test.csv", output);
            List<string> output = new List<string>();

            foreach(var record in records)
            {
                StringBuilder builder = new StringBuilder();
                bool firstColumn = true;
                foreach (string value in record)
                {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(',');
                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                        //builder.AppendFormat("\"{0}\"", value);
                    else
                        builder.Append(value);
                    firstColumn = false;
                }

                output.Add(builder.ToString());
                

            }

            //System.IO.File.WriteAllLines(outPutFilePath, output.ToArray());

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(outPutFilePath))
            {
                foreach (string line in output)
                {

                    file.WriteLine(line);
                    
                }
            }


        }
    }
}

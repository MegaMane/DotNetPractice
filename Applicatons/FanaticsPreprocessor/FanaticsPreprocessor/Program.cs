using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace FanaticsPreprocessor
{

    class Program
    {
        

        static void sendFailure(string filePath, string emailBody, string mailTo, string subject)
        {

            SendEmail myEmail = new SendEmail("notifications@ptpos.com", mailTo, subject, emailBody);

            string attachment = filePath;
            myEmail.sendMail(attachment);
            
        }

        static void Main(string[] args)
        {
            /*
             //production paths
            string[] ftpPaths = { @"\\172.30.0.70\ftp\Fanatics\Imports\",
                                  @"\\172.30.0.70\ftp\Fanatics\Updates\",
                                  @"\\172.30.0.70\ftp\FanaticsCanada\Imports\",
                                  @"\\172.30.0.70\ftp\FanaticsCanada\Updates",
                                  @"\\172.30.0.70\ftp\FanaticsNascar\Imports\",
                                  @"\\172.30.0.70\ftp\FanaticsNascar\Updates",
                                };
             
            
             // Debug Paths
            string[] ftpPaths = { @"C:\testFTP\Fanatics\Imports\",
                                  @"C:\testFTP\Fanatics\Updates\",
                                  @"C:\testFTP\FanaticsCanada\Imports\",
                                  @"C:\testFTP\FanaticsCanada\Updates",
                                  @"C:\testFTP\FanaticsNascar\Imports\",
                                  @"C:\testFTP\FanaticsNascar\Updates",
                                };
            */

            string[] ftpPaths = args;

            foreach (string directoryPath in ftpPaths)
            {
                Console.WriteLine($" Now processing files in {directoryPath }");
                bool canadianImport = directoryPath.IndexOf("Canada") > -1 ? true : false;
                bool nascarImport = directoryPath.IndexOf("Nascar") > -1 ? true : false;

                string enterprise = "";

                if (canadianImport)
                {
                    enterprise = "Canada ";
                }

                else if (nascarImport)
                {
                    enterprise = "Nascar ";
                }

                string emailBody;
                string mailTo;
                string subject;
                string[] importFiles = Directory.GetFiles(directoryPath);
                string dirName = new DirectoryInfo(directoryPath).Name;


                foreach (string file in importFiles)
                {

                    string fileName = Path.GetFileName(file);
                    string filePath = Path.GetDirectoryName(file);
                    string extension = Path.GetExtension(file);
                    


                    //Don't process anything other than csv files
                    if (extension != ".csv")
                    {
                        //notify otis
                        emailBody = "<p>The file " + file + " is not in a valid format and will not be imported." +
                                    "Please open the file and determine the correct person to notify at Fanatics.</p>" +
                                     "<br /><br />Thanks<br />The Opsuite Team";

                        mailTo = "jludena@ptpos.com,rbennett@ptpos.com,luke@ptpos.com";
                        subject = "Fanatics " + enterprise +  "Item " + dirName + ": Invalid Format";
                        sendFailure(file, emailBody, mailTo, subject);

                        //Move bad file to history
                        string badFileDestination = filePath + "\\history\\" + fileName + "_invalid" + extension;
                        File.Move(file, badFileDestination);
                        continue;
                    }


                    //read csv file into list of string arrays
                    List<string[]> records = CSV.readFile(file);

                    if (records.Count < 2)
                    {
                        //Move empty csv file to history
                        string badFileDestination = filePath + "\\history\\" + fileName + "_empty" + extension;
                        File.Move(file, badFileDestination);
                        continue;
                    }

                    //create a new instance of the appropriate doctype and pass in the document contents
                    FanaticsFile impFile;

                    if (dirName == "Imports")
                    {
                        if(canadianImport)
                        {
                            impFile = new ImportFileCanada(records);
                        }

                        else
                        {
                            impFile = new ImportFile(records);
                        }
                        
                    }
                    
                    else
                    {
                        if (canadianImport)
                        {
                            impFile = new UpdateFileCanada(records);
                        }

                        else
                        {
                            impFile = new UpdateFile(records);
                        }
                    }

                    //validate headers
                    if (!impFile.ValidHeaders())
                    {
                        emailBody = impFile.HeaderComparison();
                        mailTo = "otis@ptpos.com," + impFile.EmailRecipient;
                        subject = "Fanatics " + enterprise + "Item " + dirName + ": Validation Errors";
                        sendFailure(file, emailBody, mailTo, subject);

                        //Move bad file to history
                        string badFileDestination = filePath + "\\history\\" + fileName + "_invalid" + extension;
                        File.Move(file, badFileDestination);
                        continue;
                    }


                    //check valid field lengths for all lines
                    if (!impFile.ValidFieldLengths())
                    {
                        emailBody = impFile.FieldLengthComparison();
                        mailTo = "otis@ptpos.com," + impFile.EmailRecipient;
                        subject = "Fanatics " + enterprise + "Item " + dirName + ": Validation Errors";
                        sendFailure(file, emailBody, mailTo, subject);

                        //Move bad file to history
                        string badFileDestination = filePath + "\\history\\" + fileName + "_invalid" + extension;
                        File.Move(file, badFileDestination);
                        continue;
                    }


                    //enforce validation rules and send email if file is not valid
                    string validationResults = impFile.ValidateBody();

                    if (validationResults != "")
                    {
                        emailBody = validationResults;
                        mailTo = "otis@ptpos.com," + impFile.EmailRecipient;
                        //subject = "Fanatics " + enterprise + "Item " + dirName + ": Validation Errors";
                        subject = "Fanatics " + enterprise + "Item " + dirName + ": Validation Errors";
                        sendFailure(file, emailBody, mailTo, subject);

                        //Move bad file to history
                        string badFileDestination = filePath + "\\history\\" + fileName + "_invalid" + extension;
                        File.Move(file, badFileDestination);
                    }


                    else
                    {
                        //if body is valid
                        //Write out new file(overwrite) with stripped headers and quoted fields
                        Console.WriteLine("{0} File is Valid", file);
                        CSV.WriteFile(impFile.docList, file);

                    }


                    //if any unexpected errors occur the file name should be logged and processing should coninue with the next file 
                    //basically let Sql try anyway.
                    Console.WriteLine("Processing the next file");

                }







            }

            //Console.WriteLine("Press Any Key To Continue");
            //Console.ReadKey();

        } //end foreach ftpPaths


    } //end main
} //end class

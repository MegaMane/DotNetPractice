using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Collections;


namespace OpComm_SFTPUpload
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (string arg in args) { Console.WriteLine(arg); }
            string sourcePath = args[0];
            string destination = args[1];
            string historyPath = args[2];
            string archivePath = args[3];
            string host = args[4];
            string username = args[5];
            string password = args[6];

            //string sourcePath = @"\\172.30.0.70\DataImportExport\IronMan\Working";
            //string destination = @"\Inbox\test";
            //string historyPath = @"\\172.30.0.70\DataImportExport\IronMan\Working\Completed";
            //string archivePath = @"\\172.30.0.70\DataImportExport\IronMan\Working\Completed\Archive";
            //string host = @"mftqa.jaggedpeak.com";
            //string username = "OpSuiteSFTP";
            //string password = "mE*HAcuruD$8";

            int port = 22;  //Port 22 is defaulted for SFTP upload

            if (IsDirectoryEmpty(sourcePath))
            {
                Console.WriteLine("There are no files");
            }

            else
            {
                //Files exist work to be done
                Console.WriteLine("found at least one file to upload to sftp");
      
                //upload files to ftp and move to completed folder when done
                sftp.UploadSFTPFile(host, username, password, sourcePath, destination, port);

                string todaysFiles = "FilesUploaded_" + System.DateTime.Now.ToString("yyyyMMdd");
                string yesterdaysFiles = "FilesUploaded_" + System.DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

                //Move Completed files into temp folder for today to be archived tomorrow

                //DateTime.Now.ToString("yyyyMMddHHmmss") if directory does not exist create it else do nothing;
                System.IO.Directory.CreateDirectory(historyPath + "\\" + todaysFiles);

                DirectoryInfo info = new DirectoryInfo(historyPath);
                FileInfo[] files = info.GetFiles().ToArray();
                foreach (FileInfo file in files)
                {
                    String sourcefilePath = historyPath + "\\" + file.ToString();
                    String destinationPath = historyPath + "\\" + todaysFiles + "\\" + file.ToString();
                    File.Move(sourcefilePath, destinationPath);
                }

                //Zip Up and Archive Yesterday's Files
                if (Directory.Exists(historyPath + "\\" + yesterdaysFiles))
                {
                    Console.WriteLine("Zipping Yesterday's Files");
                    string startPath = historyPath + "\\" + yesterdaysFiles;
                    string zipPath = archivePath + "\\" + yesterdaysFiles;


                    //http://dotnetzip.herobo.com/DNZHelp/Index.html
                    //Namespace for DotNetZip is Ionic
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(startPath, yesterdaysFiles);
                        zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                        zip.Save(zipPath + ".zip");
                    }

                    //Clean up directory with unzipped files
                    System.IO.Directory.Delete(startPath, true);

                }

                //Delete any archived files older than two weeks from current date
                var folders = new DirectoryInfo(archivePath).GetFiles("*.zip");
                foreach (var folder in folders)
                {
                    if (DateTime.UtcNow - folder.CreationTimeUtc > TimeSpan.FromDays(14))
                    {
                        File.Delete(folder.FullName);
                    }
                }



            }


            Console.WriteLine("Done");
            //Console.ReadLine();

        }


        public static bool IsDirectoryEmpty(string sourcePath)
        {
            IEnumerable<string> items = Directory.EnumerateFiles(sourcePath);
            using (IEnumerator<string> en = items.GetEnumerator())
            {
                return !en.MoveNext();
            }
        }


     

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.IO;

namespace OpComm_SFTPUpload
{
    class sftp
    {
        public static void UploadSFTPFile(string host, string username,
        string password, string sourcePath, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 180);
                client.Connect();
                client.ChangeDirectory(destinationpath);

                DirectoryInfo info = new DirectoryInfo(sourcePath);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.LastWriteTime).ToArray();
                foreach (FileInfo file in files)
                {
                    try
                    {
                        String sourcefile = sourcePath + "\\" + file.ToString();

                        using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024;
                            client.UploadFile(fs, Path.GetFileName(sourcefile), true);
                        }

                        File.Move(sourcefile, sourcePath + "\\Completed\\" + file.ToString());
                        
                        //throw new ArgumentNullException("This was a null exception");

                        //ArgumentNullException input is null.
                        //ArgumentException path is null or contains whitespace characters. 
                        //SshConnectionException Client is not connected.
                        //SftpPermissionDeniedException Permission to upload the file was denied by the remote host. 
                                    //-or -
                                    //A SSH command was denied by the server.
                        //SshException A SSH error where Message is the message from the remote host.
                        //ObjectDisposedException The method was called after the client was disposed. 


                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }

                client.Disconnect();
            }
        }
    }
}

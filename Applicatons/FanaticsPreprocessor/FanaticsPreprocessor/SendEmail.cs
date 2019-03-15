using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace FanaticsPreprocessor
{
    class SendEmail
    {
        MailMessage htmlMail;
        SmtpClient smtpClient;
        Attachment fileAttachment;

        public string mailBody { get; set; }
        public string mailFrom { get; set; }
        public string mailTo { get; set; }
        public string mailSubject { get; set; }

        public SendEmail(string mailFrom, string mailTo, string mailSubject, string mailBody)
        {
            this.mailTo = mailTo;
            this.mailFrom = mailFrom;
            this.mailSubject = mailSubject;
            this.mailBody = mailBody;
        }

        public void sendMail (string attachmentPath)
        {



            htmlMail = new MailMessage(mailFrom, mailTo, mailSubject, mailBody);
            htmlMail.IsBodyHtml = true;

            fileAttachment = new Attachment(attachmentPath);
            htmlMail.Attachments.Add(fileAttachment);

            smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("notifications@ptpos.com", "1black3#");
            smtpClient.EnableSsl = true;

            smtpClient.Send(htmlMail);

            //must call Dispose() o/w SSIS will not release the lock on the attached file and you won't be able to perform other operations on it.
            htmlMail.Attachments.Dispose();
            fileAttachment = null;  // release references to disposed Attachment object
        }

        
    }
}

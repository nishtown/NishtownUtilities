using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Nishtown.Utilities
{
    class Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Server { get; set; }
        public string Subject { get; set; }
        public string Filename { get; set; }
        public string Body { get; set; }

        public void send()
        {
            MailMessage emlMessage = new MailMessage();

            emlMessage.To.Add(To);
            emlMessage.From = new MailAddress(From);
            emlMessage.Subject = Subject;
            emlMessage.Body = Body;
            if (Filename != null)
            {
                emlMessage.Attachments.Add(new Attachment(Filename));
            }

            SmtpClient emlSmtp = new SmtpClient(Server);
            emlSmtp.Send(emlMessage);
            clear();
        }
        private void clear()
        {
            Body = null;
            Subject = null;
            Filename = null;
        }
    }
}

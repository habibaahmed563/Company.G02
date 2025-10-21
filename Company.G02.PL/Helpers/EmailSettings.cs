using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool sendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP

           try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("habibaahmed15935700@gmail.com", "olyumbqxgaerjrdj"); // Sender
                client.Send("habibaahmed15935700@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

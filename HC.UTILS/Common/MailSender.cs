using System.Net;
using System.Net.Mail;

namespace Common
{
    public class MailSender
    {

        public static void SendEmail(string email, string subject, string message)
        {
            // Mail gönderebilmek için bu namespace içerisinde bulunan MailMessage ve SMTPClient siniflarindan faydalanırız.using System.Net ve using System.Net.Mail kütüphanelerinin olması gerekir.

            //Sender
            MailMessage sender = new MailMessage(); //Mail mesajimi olusturabilmek için MailMessage sinifi türünden bir degisken olusturmamiz gerekmektedir.sender degiskenimiz, göndecegimiz e-posta'mizin bütün elemanlarini bulundurmaktadir.

            sender.From = new MailAddress("hotcatproject@gmail.com", "HotCatProject");//E-Posta'nin kimden gönderilecegi bilgisini tutar. MailAddress türünden bir degisken istemektedir.
            sender.To.Add(email);//: E-Postanin kime/kimlere gönderilecegi bilgisini tutar.
            sender.Subject = subject;//E-Posta'nin konusu bilgisini tutar.
            sender.Body = message;//E-Posta'nin içerik bilgisini tutar.
            sender.IsBodyHtml = true;// her türlü html kodu yazabilmek için.


            //Mail Gönderme Protokolü: SMTP(Simple Mail Tranfer Protocol)
            //Smtp
            SmtpClient smtp = new SmtpClient();// SMTPClient : E-Posta'nin gönderilecegi SMTP sunucu ve gönderen kullanicinin bilgilerinin yazilip, MailMessage türünde olusturulan mailin gönderildigi siniftir.
            smtp.Credentials = new NetworkCredential("hotcatproject@gmail.com", "wovykwfjmdybtfnu");//: E-Posta'yi gönderen kullanicinin kimlik bilgilerini tutar.
            smtp.Port = 587;//SMTP sunucusunun port bilgisini tutar.587 nolu portta kimlik denetimi zorunludur buyuzden 25 yerine 587 kullanılır ve bu sayade spam e-postalarında önüne geçilmeye çalışılır.
            smtp.Host = "smtp.gmail.com"; //SMTP sunucusunun isim bilgisini tutar.
            smtp.EnableSsl = true;//Sunucu SSL istiyorsa bu degeri true yapacagiz.

            smtp.Send(sender);//E-Posta'yi gönderme islemini yapar. Sunucuya göre Send ya da SendAsync metodlarindan birisi kullanilir.

        }
    }
}

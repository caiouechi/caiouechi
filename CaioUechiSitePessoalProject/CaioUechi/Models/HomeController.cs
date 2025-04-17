using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CaioUechi.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpPost]
        public ActionResult ConfirmSubmission(List<string> names)
        {
            if (names == null || names.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No names provided.");
            }

            // Get the first person's name and sanitize it (remove spaces and special characters)
            var firstName = names.FirstOrDefault();
            var safeFirstName = new string(firstName.Where(char.IsLetterOrDigit).ToArray());

            // Create a file name with the first name and the current date/time
            string dateStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = safeFirstName + "_" + dateStr + ".txt";

            // Save the file in the bin folder
            string filePath = Server.MapPath("~/bin/" + fileName);
            System.IO.File.WriteAllLines(filePath, names);

            // Return the file name in the JSON response
            return Json(new { success = true, fileName = fileName });
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviaEmail(string destinatario, string mensagem, string name)
        {
            bool retorno;

            try
            {
                //'cria objeto para receber os dados do email
                MailMessage oEmail = new MailMessage();

                //'remetente do email
                oEmail.From = new MailAddress("caio.uechi@gmail.com");
                //'destinatario do email
                oEmail.To.Add("caio_uechi@hotmail.com");

                //'destinatario de copia do email
                //oEmail.To.Add("diretoria@takaki.com.br");
                //'destinatario de copia oculta
                //oEmail.Bcc.Add("copiaOculta");
                //'prioridade de envio
                oEmail.Priority = MailPriority.Normal;
                //'define o assunto do email
                oEmail.Subject = name + " - " + destinatario;
                //'define a mensagem principal do email
                oEmail.Body = mensagem;

                //'cria o objeto SMTP
                SmtpClient oSmtp = new SmtpClient();

                oSmtp.EnableSsl = true;

                try
                {
                    //'envia o email
                    oSmtp.Send(oEmail);
                }
                catch (Exception e)
                {
                    var b = e.InnerException.ToString();
                }

                retorno = true;
            }
            catch (Exception e)
            {
                var b = e.Message;
                retorno = false;
            }


            return new JsonResult { Data = retorno, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}

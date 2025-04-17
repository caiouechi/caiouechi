using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CaioUechi.Models;

namespace CaioUechi.Controllers
{
    public class TemplateController : Controller
    {
        // GET: /Template/
        [HttpGet]
        public ActionResult Index()
        {
            var obj = new TemplateModel();
            obj.Telefone1 = "11 - 5070.4470";
            obj.Posicao = "Consultor de Imóveis";
            // Render empty form
            return View(obj);
        }

        // POST: /Template/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(TemplateModel model)
        {
            // Load the HTML template from disk
            var path = Server.MapPath("~/Templates/Template.html");
            string html = System.IO.File.ReadAllText(path, Encoding.UTF8);

            // Replace placeholders
            html = html
                .Replace("#NAME#", HttpUtility.HtmlEncode(model.Name))
                .Replace("#POSICAO#", HttpUtility.HtmlEncode(model.Posicao))
                .Replace("#TELEFONE1#", HttpUtility.HtmlEncode(model.Telefone1))
                .Replace("#TELEFONE2#", HttpUtility.HtmlEncode(model.Telefone2))
                .Replace("#CRECI#", HttpUtility.HtmlEncode(model.Creci));

            // Return as downloadable file
            byte[] bytes = Encoding.UTF8.GetBytes(html);
            return File(
                bytes,
                "text/html",
                "GeneratedTemplate.html"
            );
        }
    }
}

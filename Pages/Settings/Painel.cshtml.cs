using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SiteVideos.Classes;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static SiteVideos.Pages.Settings.PainelModel;

namespace SiteVideos.Pages.Settings
{
    [Authorize(Policy = "AdminOnly")]
    public class PainelModel : PageModel
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        public PainelModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [BindProperty]
        public Configuracao configuracao { get; set; }

        public List<PastaDir> painelModels { get; set; } = new List<PastaDir>();

        public class PastaDir
        {
            public string? pastas { get; set; }
            public List<string> subPastas { get; set; } = new List<string>();
        }

        public class Configuracao
        {
            public string? Titulo { get; set; }

            public IFormFile Arquivo { get; set; }

            public string? departamento { get; set; }

            public string? newSubPasta { get; set; }
            public string? subPasta { get; set; }
        }

        public void OnGet()
        {
            CarregaCombos();
        }

        public void OnPost()
        {
            if (Request.Form.Keys.Contains("btnNewSub"))
            {
                if (!string.IsNullOrEmpty(configuracao.departamento) && !string.IsNullOrEmpty(configuracao.newSubPasta))
                {
                    CriaDiretorio(configuracao.newSubPasta, configuracao.departamento);
                }
            }

            else if (Request.Form.Keys.Contains("SalvarConfig"))
            {
                if (!string.IsNullOrEmpty(configuracao.departamento) && !string.IsNullOrEmpty(configuracao.Titulo)
                    && !string.IsNullOrEmpty(configuracao.subPasta) && configuracao.Arquivo != null)
                {
                    Salva();
                }
            }

            CarregaCombos();
        }

        public void CarregaCombos()
        {
            var departamentos = new List<PastaDir>();

            List<string> source = Directory.GetDirectories(Global.CaminhoPastasVideos)
                     .Select(c => new DirectoryInfo(c).Name)
                     .ToList();

            foreach (var item in source)
            {
                var dep = new PastaDir { pastas = item };

                List<string> sourceDir = Directory.GetDirectories(Global.CaminhoPastasVideos + item)
                     .Select(c => new DirectoryInfo(c).Name)
                     .ToList();

                foreach (var d in sourceDir)
                {
                    dep.subPastas.Add(d);
                }

                departamentos.Add(dep);
            }

            painelModels.AddRange(departamentos);
        }

        public bool CriaDiretorio(string novoTema, string departamento)
        {
            string dir = Global.CaminhoPastasVideos + departamento + "\\" + novoTema;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Salva()
        {
            string wwwPath = this.Environment.WebRootPath;

            string path = Path.Combine(wwwPath, "Upload");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(configuracao.Arquivo.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                configuracao.Arquivo.CopyTo(stream);
            }

            if (configuracao.Arquivo.ContentType == "video/mp4")
            {
                string origem = Path.Combine(wwwPath, "Upload", fileName);
                string videoName = configuracao.Titulo + Path.GetExtension(configuracao.Arquivo.FileName);
                string destino = Path.Combine(Global.CaminhoPastasVideos, configuracao.departamento + "\\" + configuracao.subPasta + "\\" + videoName);
                System.IO.File.Move(origem, destino);
            }
        }
    }
}
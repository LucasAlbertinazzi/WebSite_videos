using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SiteVideos.Classes;
using System.ComponentModel.DataAnnotations;
using static SiteVideos.Pages.Settings.PainelModel;

namespace SiteVideos.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private string departamentoHttp;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public Temas cardvideos { get; set; }

        public List<Temas> listaCard { get; set; } = new List<Temas>();
        public List<CardVideos> listaVideos { get; set; } = new List<CardVideos>();

        public string DefineDep { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var d = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            departamentoHttp = d.Value;

            if (departamentoHttp == "T.I" || departamentoHttp == "DIRETORIA")
            {
                DefineDep = "Configurações";
            }
            else
            {
                DefineDep = "";
            }

            CarregaTemas();
            CarregaTemasVideos();
        }

        public class Temas
        {
            public string Departamento { get; set; }

            public List<string> TemasVideos { get; set; } = new List<string>();
        }

        public class CardVideos
        {
            public string TemasDep { get; set; }

            public List<InfoVideos> Videos { get; set; } = new List<InfoVideos>();
        }

        public class InfoVideos
        {
            public string Url { get; set; }
            public string TituloVideo { get; set; }
        }

        public void CarregaTemas()
        {
            var lista = new List<Temas>();


            string caminhoDep = Global.CaminhoPastasVideos + departamentoHttp;

            List<string> sourceDir = Directory.GetDirectories(caminhoDep)
                     .Select(c => new DirectoryInfo(c).Name)
                     .ToList();

            var dep = new Temas { Departamento = departamentoHttp };

            if (sourceDir.Count > 0)
            {
                foreach (var item in sourceDir)
                {
                    dep.TemasVideos.Add(item);
                }

                lista.Add(dep);

                listaCard.AddRange(lista);
            }
        }

        public void CarregaTemasVideos()
        {
            var arq = new List<CardVideos>();

            if (listaCard.Count > 0)
            {
                foreach (var item in listaCard[0].TemasVideos)
                {
                    string caminho = Global.CaminhoPastasVideos + departamentoHttp + "\\" + item;

                    string[] arquivos = Directory.GetFiles(caminho);

                    if (arquivos.Length > 0)
                    {
                        var lista = new CardVideos { TemasDep = item };

                        foreach (string videos in arquivos)
                        {
                            FileInfo file = new FileInfo(videos);

                            string nome = file.Name.Replace(".mp4", "");
                            string url = "/videos/" + departamentoHttp + "/" + item + "/" + nome + ".mp4";
                            lista.Videos.Add(new InfoVideos
                            {
                                Url = url,
                                TituloVideo = nome
                            });
                        }

                        arq.Add(lista);
                    }

                }

                listaVideos.AddRange(arq);
            }
        }
    }
}
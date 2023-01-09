using Newtonsoft.Json;
using SiteVideos.Classes;
using System.Net;
using System.Text;

namespace SiteVideos.Api
{
    public static class APIUser
    {
        public static async Task<InfoUser> ValidaUser(string user, string senha)
        {
            try
            {
                using (var cliente = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        userName = user,
                        password = senha
                    });

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", Global.tokenApi);

                    var resposta = cliente.PostAsync(Global.urlApi, content);
                    await resposta;

                    if (resposta.Result.StatusCode == HttpStatusCode.OK)
                    {
                        var retorno = resposta.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<InfoUser>(retorno.Result);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
namespace SiteVideos.Classes
{
    public static class Global
    {
        public static string tokenApi = "Bearer 'token da api'";

        public static string urlApi = "http........";

        public static string CaminhoPastasVideos = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + "\\wwwroot\\videos\\";
    }
}
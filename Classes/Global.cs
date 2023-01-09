namespace SiteVideos.Classes
{
    public static class Global
    {
        public static string tokenApi = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI3ODhjZmMxNy04MmQ1LTQ1ZGQtYjRkOC1lMGIwZTYxYTA3NmUiLCJuYW1laWQiOiJDYW5hbFZpZGVvIiwicm9sZSI6InUyIiwibmJmIjoxNjYyMDM3NjY2LCJleHAiOjIyOTMxODk2NjYsImlhdCI6MTY2MjAzNzY2Nn0.F0CG41LrL3urILrYth3zwgCAwQJDgfSBvqbAXaRgFvRmHhW06H37ZvW1lio9AakaxOHdc28DFXsDXXC41QCmzw";

        public static string urlApi = "http://192.168.10.3:9875/users/valid-login";

        public static string CaminhoPastasVideos = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + "\\wwwroot\\videos\\";
        //public static string CaminhoPastasVideos = "C:\\inetpub\\wwwroot\\CanalMarcius\\wwwroot\\videos\\";
    }
}
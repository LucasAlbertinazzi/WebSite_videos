using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SiteVideos.Api;
using SiteVideos.Classes;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SiteVideos.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential credential { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var dt = await APIUser.ValidaUser(credential.UserName, credential.Password);

            if (dt != null)
            {
                string senhaCrp = Security.Criptografar(credential.Password);

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, dt.nomeCompleto),
                    new Claim(ClaimTypes.NameIdentifier, dt.usuario),
                    new Claim(ClaimTypes.Email, dt.departamento)
                };

                if (dt.codDepartamento == 1 || dt.codDepartamento == 10)
                {
                    claims.Add(new Claim("Admin", "true"));
                }

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = credential.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return RedirectToPage("/Index");
            }

            return Page();
        }

        public class Credential
        {
            [Required(ErrorMessage = "* Este campo é obrigatório")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "* Este campo é obrigatório")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lembre-me")]
            public bool RememberMe { get; set; }
        }
    }
}
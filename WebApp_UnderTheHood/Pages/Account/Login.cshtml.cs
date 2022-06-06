using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            //verification credential
            if (Credential.UserName == "Admin" && Credential.Password == "admin")
            {
                //Creating  the security context
                var claims = new List<Claim>
                {
                    new Claim( ClaimTypes.Name,"admin"),
                    new Claim( ClaimTypes.Email, "admin@website.com"),
                    new Claim( "Department","HR" ),
                    new Claim( "Admin", "true"),
                    new Claim( "Manager", "true"),
                    new Claim( "EmploymentDate","2022-05-01")
                };
                var identity =  new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe,
                };

                await HttpContext.SignInAsync("MyCookieAuth",claimsPrincipal, authProperties);

                return RedirectToPage("/Index");
             }
            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remeber Me")]
        public bool RememberMe { get; set; }
    }
}

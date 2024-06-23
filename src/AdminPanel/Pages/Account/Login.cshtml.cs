using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(IUserService userService, SignInManager<IdentityUser> signInManager)
    {
        _userService = userService;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync(string username, string password)
    {
        var user = await _userService.Authenticate(username, password);
        if (user != null)
        {
            var identityUser = new IdentityUser { UserName = user.Username };
            await _signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToPage("/Index");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}

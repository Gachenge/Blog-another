using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;



namespace Blog.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class _UpdateProfilePictureModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public void OnGet()
        {
        }

        public _UpdateProfilePictureModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public IFormFile ProfilePicture { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var avatarDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                    if (!Directory.Exists(avatarDirectory))
                    {
                        Directory.CreateDirectory(avatarDirectory);
                    }

                    // Delete the old avatar if it exists
                    if (!string.IsNullOrEmpty(user.Avatar) && System.IO.File.Exists(user.Avatar))
                    {
                        System.IO.File.Delete(user.Avatar);
                    }

                    // Combine the new avatar path
                    var newAvatarFileName = $"{user.Id}_avatar.jpg";
                    var newAvatarPath = Path.Combine(avatarDirectory, newAvatarFileName);

                    // Save the new avatar path to the user's claims
                    await _userManager.AddClaimAsync(user, new Claim("Avatar", $"/avatars/{newAvatarFileName}"));

                    // Set the Avatar property of ApplicationUser
                    user.Avatar = newAvatarPath;

                    // Save the file to the new avatar path
                    using (var stream = new FileStream(newAvatarPath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    // Update the user in the database
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToPage("/Account/Manage/Index");
            }

            return Page();
        }
    }
}

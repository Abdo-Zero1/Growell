// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using NuGet.Packaging.Signing;

namespace Growell.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Profile Picture")]
            public string ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                PhoneNumber = phoneNumber,
                ProfilePicture= user.ProfilePicture
                
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var FirstName=user.FirstName;
            var LastName=user.LastName;

            if(Input.FirstName!=FirstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.LastName != LastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                string random = Guid.NewGuid().ToString(); // إنشاء اسم عشوائي للصورة
                string extension = Path.GetExtension(file.FileName); // الحصول على امتداد الصورة

                // الحصول على دور المستخدم
                var roles = await _userManager.GetRolesAsync(user);
                string userRole = roles.FirstOrDefault(); // تحديد الدور الأول للمستخدم

                // تحديد المجلد المناسب بناءً على الدور
                string roleFolder = userRole switch
                {
                    "Admin" => "Admin",
                    "Doctor" => "Doctor",
                    "User" => "User",
                    _ => "General" // مجلد افتراضي إذا لم يكن هناك دور محدد
                };

                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", roleFolder);
                string fileName = random + extension; // اسم الصورة الجديد
                string filePath = Path.Combine(directoryPath, fileName); // المسار الكامل للصورة الجديدة

                // حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    // التأكد من وجود الملف القديم في المسار الصحيح
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", roleFolder, user.ProfilePicture);

                    // تحقق إذا كانت الصورة القديمة موجودة
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldFilePath); // حذف الصورة القديمة
                        }
                        catch (Exception ex)
                        {
                            // تسجيل الخطأ أو التعامل معه
                            Console.WriteLine($"Error deleting old profile picture: {ex.Message}");
                        }
                    }
                }

                // إنشاء المجلد إذا لم يكن موجودًا
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // حفظ الصورة الجديدة
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // تحديث مسار الصورة في قاعدة البيانات
                user.ProfilePicture = Path.Combine(roleFolder, fileName); // تخزين المسار النسبي للصورة
                await _userManager.UpdateAsync(user);
            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

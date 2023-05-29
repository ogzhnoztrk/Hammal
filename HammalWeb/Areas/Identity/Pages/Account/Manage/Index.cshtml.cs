// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HammalWeb.Areas.Identity.Pages.Account.Manage
{

    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;


        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        [BindProperty(SupportsGet = true)]
        public int AbilityId { get; set; }
        public IEnumerable<AltCategory> AltCategories{ get; set; }

        public double DegiskenDeneme = 3.5;
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string FullAdress{ get; set; }

        public Address Address { get; set; }
        public UserVM UserVM { get; set; }
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
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var applicationUser  = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == user.Id);

            var addresses = _unitOfWork.Address.GetAll(x => x.ApplicationUserId == user.Id,includeProperties:"District");
            foreach (var address in addresses)
            {
                address.District.City = _unitOfWork.City.GetFirstOrDefault(i => i.Id == address.District.CityId);
            }

            var userAbilites= _unitOfWork.UserAbility.GetAll(x => x.ApplicationUserId == user.Id, includeProperties:"AltCategory");
            


            Username = userName;
            Name = applicationUser.Name;
            Email = applicationUser.Email;
         

            UserVM =new() {
                ApplicationUser = applicationUser,
                Addresses = addresses,
                UserAbilities =  userAbilites
            };
            AltCategories = _unitOfWork.AltCategory.GetAll();


            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = Name,
                Email = Email,

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
            var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == user.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (applicationUser.Email != Input.Email) { 
                if (_unitOfWork.ApplicationUser.GetAll(x => x.Email == Input.Email) == null)
                {
                    await _userManager.SetUserNameAsync(user, Input.Email);
                    await _userManager.SetEmailAsync(user, Input.Email);
                    await _userManager.SetUserNameAsync(user, Input.Email);
                    return RedirectToPage();
                }
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Name != applicationUser.Name)
            {
                applicationUser.Name = Input.Name;
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();


            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        

        //AbilityDelete
        public IActionResult OnPostAbilityDelete()
        {
            var userAbility = _unitOfWork.UserAbility.GetFirstOrDefault(x=> x.Id == AbilityId);
            _unitOfWork.UserAbility.Remove(userAbility);
            _unitOfWork.Save();
            
            return RedirectToPage();
        }

        //AbilityPost
        public async Task<IActionResult> OnPostAbilityPostAsync(List<int> selectedCheckboxes)
        {
            //List<string> selectedAbilityIds = Request.Form["checkbox"].ToList();
            var user = await _userManager.GetUserAsync(User);
            
            foreach (int selectedAbilityId in selectedCheckboxes)
            {
                if (_unitOfWork.UserAbility.GetFirstOrDefault(x => x.AltCategoryId == selectedAbilityId) == null)
                {
                    _unitOfWork.UserAbility.Add(new()
                    {
                        AltCategoryId = selectedAbilityId,
                        ApplicationUserId = user.Id,
                    });
                }
              
            }
            _unitOfWork.Save();
            return RedirectToPage();
        }

    }
}

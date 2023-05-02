// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace HammalWeb.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

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
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <title></title>\r\n\r\n    <style type=\"text/css\">\r\n        @media only screen and (min-width: 620px) {{\r\n            .u-row {{\r\n                width: 600px !important;\r\n            }}\r\n\r\n                .u-row .u-col {{\r\n                    vertical-align: top;\r\n                }}\r\n\r\n                .u-row .u-col-100 {{\r\n                    width: 600px !important;\r\n                }}\r\n        }}\r\n\r\n        @media (max-width: 620px) {{\r\n            .u-row-container {{\r\n                max-width: 100% !important;\r\n                padding-left: 0px !important;\r\n                padding-right: 0px !important;\r\n            }}\r\n\r\n            .u-row .u-col {{\r\n                min-width: 320px !important;\r\n                max-width: 100% !important;\r\n                display: block !important;\r\n            }}\r\n\r\n            .u-row {{\r\n                width: 100% !important;\r\n            }}\r\n\r\n            .u-col {{\r\n                width: 100% !important;\r\n            }}\r\n\r\n                .u-col > div {{\r\n                    margin: 0 auto;\r\n                }}\r\n        }}\r\n\r\n        body {{\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n\r\n        table,\r\n        tr,\r\n        td {{\r\n            vertical-align: top;\r\n            border-collapse: collapse;\r\n        }}\r\n\r\n        .ie-container table,\r\n        .mso-container table {{\r\n            table-layout: fixed;\r\n        }}\r\n\r\n        * {{\r\n            line-height: inherit;\r\n        }}\r\n\r\n        a[x-apple-data-detectors='true'] {{\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n        }}\r\n\r\n        table, td {{\r\n            color: #000000;\r\n        }}\r\n\r\n        #u_body a {{\r\n            color: #0000ee;\r\n            text-decoration: underline;\r\n        }}\r\n\r\n        @media (max-width: 480px) {{\r\n            #u_content_image_1 .v-container-padding-padding {{\r\n                padding: 40px 10px 10px !important;\r\n            }}\r\n\r\n            #u_content_image_1 .v-src-width {{\r\n                width: auto !important;\r\n            }}\r\n\r\n            #u_content_image_1 .v-src-max-width {{\r\n                max-width: 50% !important;\r\n            }}\r\n\r\n            #u_content_heading_1 .v-container-padding-padding {{\r\n                padding: 10px 10px 20px !important;\r\n            }}\r\n\r\n            #u_content_heading_1 .v-font-size {{\r\n                font-size: 22px !important;\r\n            }}\r\n\r\n            #u_content_button_1 .v-container-padding-padding {{\r\n                padding: 30px 10px 40px !important;\r\n            }}\r\n\r\n            #u_content_button_1 .v-size-width {{\r\n                width: 65% !important;\r\n            }}\r\n        }}\r\n    </style>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Raleway:400,700&display=swap\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]-->\r\n</head>\r\n<body class=\"clean-body u_body\" style=\"margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f9f9ff;color: #000000\">\r\n    <table id=\"u_body\" style=\"border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f9f9ff;width:100%\" cellpadding=\"0\" cellspacing=\"0\">\r\n        <tbody>\r\n            <tr style=\"vertical-align: top\">\r\n                <td style=\"word-break: break-word;border-collapse: collapse !important;vertical-align: top\">\r\n                    <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\">\r\n                        <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\">\r\n                            <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\">\r\n\r\n                                <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;\">\r\n                                    <div style=\"background-color: #ffffff;height: 100%;width: 100% !important;\">\r\n                                        <div style=\"box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\">\r\n\r\n\r\n                                            <table id=\"u_content_image_1\" style=\"font-family:'Raleway',sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n                                                <tbody>\r\n                                                    <tr>\r\n                                                        <td class=\"v-container-padding-padding\" style=\"overflow-wrap:break-word;word-break:break-word;padding:60px 10px 10px;font-family:'Raleway',sans-serif;\" align=\"left\">\r\n                                                            <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n                                                                <tr>\r\n                                                                    <td style=\"padding-right: 0px;padding-left: 0px;\" align=\"center\">\r\n                                                                        <img align=\"center\" border=\"0\" src=\"https://cdn.templates.unlayer.com/assets/1676547950700-Asset%201.png\" alt=\"image\" title=\"image\" style=\"outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 35%;max-width: 203px;\" width=\"203\" class=\"v-src-width v-src-max-width\" />\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </table>\r\n                                                        </td>\r\n                                                    </tr>\r\n                                                </tbody>\r\n                                            </table>\r\n\r\n                                            <table id=\"u_content_heading_1\" style=\"font-family:'Raleway',sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n                                                <tbody>\r\n                                                    <tr>\r\n                                                        <td class=\"v-container-padding-padding\" style=\"overflow-wrap:break-word;word-break:break-word;padding:10px 10px 30px;font-family:'Raleway',sans-serif;\" align=\"left\">\r\n                                                            <h1 class=\"v-font-size\" style=\"margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-size: 28px; \"><strong>Forget password ?</strong></h1>\r\n                                                        </td>\r\n                                                    </tr>\r\n                                                </tbody>\r\n                                            </table>\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\">\r\n                        <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\">\r\n                            <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\">\r\n\r\n                                <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;\">\r\n                                    <div style=\"background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\">\r\n                                        <div style=\"box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\">\r\n\r\n\r\n                                            <table id=\"u_content_button_1\" style=\"font-family:'Raleway',sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n                                                <tbody>\r\n                                                    <tr>\r\n                                                        <td class=\"v-container-padding-padding\" style=\"overflow-wrap:break-word;word-break:break-word;padding:30px 10px 40px;font-family:'Raleway',sans-serif;\" align=\"left\">\r\n\r\n                                                            <div align=\"center\">\r\n\r\n                                                                <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' target=\"_blank\" class=\"v-button v-size-width v-font-size\" style=\"box-sizing: border-box;display: inline-block;font-family:'Raleway',sans-serif;text-decoration: none;-webkit-text-size-adjust: none;text-align: center;color: #000000; background-color: #fdb441; border-radius: 25px;-webkit-border-radius: 25px; -moz-border-radius: 25px; width:38%; max-width:100%; overflow-wrap: break-word; word-break: break-word; word-wrap:break-word; mso-border-alt: none;font-size: 14px;\">\r\n                                                                    <span style=\"display:block;padding:10px 20px;line-height:120%;\"><span style=\"line-height: 16.8px;\">Reset Your Password</span></span>\r\n                                                                </a>\r\n\r\n                                                            </div>\r\n\r\n                                                        </td>\r\n                                                    </tr>\r\n                                                </tbody>\r\n                                            </table>\r\n\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n\r\n\r\n\r\n\r\n\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </table>\r\n\r\n</body>\r\n\r\n</html>\r\n");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}

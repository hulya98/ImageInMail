using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SendMailSmtp.Services.Abstract;
using SendMailSmtp.ViewModels;

namespace SendMailSmtp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<bool> SendMail()
        {
            VM_EmailVerification vM_EmailVerification = new VM_EmailVerification()
            {
                Body = new HtmlString("This mail created for test.With this way you can put image your mails"),
                CompanyName = "Company",
                Header = "Header",
                Subject = "Subject",
                Token = "your token behind the button"
            };
            var result = await _mailService.SendUsingTemplate("Test Mail", vM_EmailVerification, @"EmailVerification.cshtml", @"verification.png", new List<string> { "emails" });
            return result;
        }


    }
}
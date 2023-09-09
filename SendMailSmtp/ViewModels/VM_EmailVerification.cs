using Microsoft.AspNetCore.Html;

namespace SendMailSmtp.ViewModels
{
    public class VM_EmailVerification
    {
        private readonly IConfiguration _configuration;
        public VM_EmailVerification()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public HtmlString? Body { get; set; }
        public string? Header { get; set; }
        public string? CompanyName { get; set; }

        private int ActualYear => DateTime.UtcNow.Year;

        public string GenerateCopyRightText()
        {
            return "generate copy right text";
        }

        public string GenerateAutomatedEmailText()
        {
            return "This is an automatically generated email - please do not reply";
        }

        public string GenerateCompanyNameTeamText()
        {
            return "your company";
        }

        public string GenerateDevelopedInformationText()
        {
            return "developed by your team";
        }

        public string? Token { get; set; }
        public string? Subject { get; set; }
        public string GenerateButtonText()
        {
            return "Verify Email Now";
        }

        public string TemplateName()
        {
            return @"EmailVerification.cshtml";
        }

        public string ImageName()
        {
            return @"verification.png";
        }

        public string GetEmailVerifiedLink()
        {
            return "button link for verify";
        }

    }
}

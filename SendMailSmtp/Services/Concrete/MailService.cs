using RazorLight;
using SendMailSmtp.Services.Abstract;
using System.Net;
using System.Net.Mail;

namespace SendMailSmtp.Services.Concrete
{
    public class MailService : IMailService
    {
        public async Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos)
        {

            if (tos.Count == 0)
                return false;

            var fileRootPath = Path.GetFullPath(@"wwwroot/sources/templates");
            var imageRootPath = Path.GetFullPath(@"wwwroot/sources/images");

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(fileRootPath)
                .EnableEncoding()
                .UseMemoryCachingProvider()
                .Build();

            string renderedHtml = await engine.CompileRenderAsync(templateName, viewModel);
            var processedBody = PreMailer.Net.PreMailer.MoveCssInline(renderedHtml, true).Html;

            Attachment? imageAttachment = null;
            if (!string.IsNullOrEmpty(imageName))
            {
                imageAttachment = new Attachment(Path.Combine(imageRootPath, imageName));
                // Attach the image file
                imageAttachment.ContentId = "image1"; //this name must be match with the cshtml template's src id

                // Include the image reference in the HTML body
                processedBody = processedBody.Replace("cid:image1", $"cid:{imageAttachment.ContentId}");
            }
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential("mail address", "password");

                smtpClient.Host = "mail.apertech.net";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;

                using (MailMessage message = new MailMessage())
                {
                    foreach (string item in tos)
                    {
                        if (IsValidEmail(item))
                        {
                            message.From = new MailAddress("mail adress", "generalHeader");
                            message.Subject = subject;
                            message.IsBodyHtml = true;

                            // Add the image attachment to the message
                            if (message.Attachments.Count == 0 && !string.IsNullOrEmpty(imageName))
                                message.Attachments.Add(imageAttachment);

                            // Set the processed HTML body as the email body
                            message.Body = processedBody;

                            // Add recipients
                            message.To.Add(item);
                        }
                    }

                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions or logging if necessary
                        // ...
                    }
                }
            }

            return true;

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

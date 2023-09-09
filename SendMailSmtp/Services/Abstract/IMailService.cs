namespace SendMailSmtp.Services.Abstract
{
    public interface IMailService
    {
        Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos);
    }
}

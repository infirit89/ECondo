namespace ECondo.Application.Services;

public interface IEmailTemplateService
{
    public ValueTask<string> RenderTemplateAsync(string templateName, object model);
}
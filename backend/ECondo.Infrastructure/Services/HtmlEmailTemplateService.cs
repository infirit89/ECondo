using ECondo.Application.Services;
using Scriban;
using Scriban.Runtime;

namespace ECondo.Infrastructure.Services;

internal sealed class HtmlEmailTemplateService : IEmailTemplateService
{
    public async ValueTask<string> RenderTemplateAsync(string templateName, object model)
    {
        Template template = await GetTemplateAsync(templateName);

        var scriptObject = new ScriptObject();
        scriptObject.Import(model);

        var context = new TemplateContext();
        context.PushGlobal(scriptObject);
        
        return await template.RenderAsync(context);
    }

    private async Task<Template> GetTemplateAsync(string templateName)
    {
        if (_templateCache.TryGetValue(templateName, out var cachedTemplate))
        {
            return cachedTemplate;
        }
        
        var templatePath = Path.Combine(
            Directory.GetCurrentDirectory(), _templateDirectory);
        
        templatePath = Path.Combine(templatePath, templateName);
        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Email template not found {templatePath}");

        string templateContent = await File.ReadAllTextAsync(templatePath);
        var template = Template.Parse(templateContent);

        if (template.HasErrors)
            throw new Exception($"Error parsing template {template} {string.Join(", ", template.Messages)}");
        
        _templateCache.Add(templateName, template);
        return template;
    }

    private readonly Dictionary<string, Template> _templateCache = new();
    private readonly string _templateDirectory = "EmailTemplates";
}
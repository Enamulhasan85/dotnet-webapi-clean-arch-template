namespace Template.Application.Common.Settings;

/// <summary>
/// Email configuration settings
/// </summary>
public class EmailSettings
{
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    
    // For SendGrid or other email services
    public string? ApiKey { get; set; }
    public string? TemplateId { get; set; }
}

namespace MailDelivery.Model;

[Serializable]
public class EmailOptions
{
    public const string Position = "EmailConfig";
    
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool IsUseSsl { get; set; }
}
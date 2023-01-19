using Domain.Models;

namespace Domain.Model;

public class User : ITimeStampedModel
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Session { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public void SetSession(string session)
    {
        Session = session;
    }
}
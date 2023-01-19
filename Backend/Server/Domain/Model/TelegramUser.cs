using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Model;

public class TelegramUser : ITimeStampedModel
{
    [Key]
    public long IdChat { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public TelegramUser(long idChat)
    {
        IdChat = idChat;
    }
}
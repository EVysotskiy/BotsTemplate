using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Model;

public class VkUser : ITimeStampedModel
{
    [Key]
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public VkUser(long userId)
    {
        UserId = userId;
    }
}
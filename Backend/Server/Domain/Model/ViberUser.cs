using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Model;

public class ViberUser : ITimeStampedModel
{
    [Key]
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public ViberUser(string userId)
    {
        UserId = userId;
    }
}
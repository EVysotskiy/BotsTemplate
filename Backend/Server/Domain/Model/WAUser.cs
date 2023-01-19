using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Model;

public class WAUser : ITimeStampedModel
{
    [Key]
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public WAUser(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace ContactManager.Models
{
public class Contact
{
    public int ContactId { get; set; }
    // FK to AspNetUser table Id         
    public string ApplicationUser_Id { get; set; }
    [ForeignKey("ApplicationUser_Id")]
    public ApplicationUser AppUser { get; set; }

    public string Name { get; set; }

    // Code removed for brevity.
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
}
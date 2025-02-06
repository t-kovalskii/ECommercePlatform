namespace ECommercePlatform.Services.User.Infrastructure.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public Guid MerchantId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}

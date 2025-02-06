namespace ECommercePlatform.Services.User.Infrastructure.Entities;

public partial class Address
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual Entities.User User { get; set; } = null!;
}

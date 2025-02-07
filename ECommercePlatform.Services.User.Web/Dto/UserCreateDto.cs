using Newtonsoft.Json;

namespace ECommercePlatform.Services.User.Web.Dto;

public class UserCreateDto
{
    [JsonProperty("merchantId")]
    public Guid MerchantId { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonProperty("address")]
    public AddressField Address { get; set; } = new();

    public class AddressField
    {
        [JsonProperty("street")]
        public string Street { get; } = string.Empty;
        
        [JsonProperty("city")]
        public string City { get; } = string.Empty;
        
        [JsonProperty("state")]
        public string State { get; } = string.Empty;
        
        [JsonProperty("zipCode")]
        public string ZipCode { get; } = string.Empty;
        
        [JsonProperty("country")]
        public string Country { get; } = string.Empty;
    }
}

namespace c_sharp_eCommerce.Core.Models.Contracts
{
    public interface IAuditable
    {
        DateTime UpdatedAt { get; set; }
    }
}
using System;
namespace TestDelivery.DAL
{
    interface IRespondent
    {
        string Email { get; set; }
        Guid Id { get; set; }
        string Name { get; set; }
        string Notes { get; set; }
        string PhoneNumber { get; set; }
        string SecretCode { get; set; }
        string Url { get; set; }
    }
}

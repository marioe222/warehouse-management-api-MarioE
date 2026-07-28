using Warehouse.Domain.Entities;

namespace Warehouse.Api.IntegrationTests.Utilities.Builders;

public class SupplierBuilder
{
    private string _name = "Test Supplier";
    private string _country = "Lebanon";
    private string _contactEmail = "supplier@test.com";
    private string _phoneNumber = "03111111";


    public SupplierBuilder WithName(string name)
    {
        _name = name;
        return this;
    }


    public SupplierBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }


    public SupplierBuilder WithEmail(string email)
    {
        _contactEmail = email;
        return this;
    }


    public SupplierBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }


    public Supplier Build()
    {
        return new Supplier(
            _name,
            _contactEmail
        );
    }


    public string Name => _name;

    public string Country => _country;

    public string ContactEmail => _contactEmail;

    public string PhoneNumber => _phoneNumber;
}
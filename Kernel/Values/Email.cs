using System.ComponentModel.DataAnnotations;

namespace Kernel.Values;

public struct Email
{
    private readonly string _email;

    public Email(string email)
    {
        if (!new EmailAddressAttribute().IsValid(email))
        {
            throw new ArgumentException("Invalid email address");
        }

        _email = email;
    }
}
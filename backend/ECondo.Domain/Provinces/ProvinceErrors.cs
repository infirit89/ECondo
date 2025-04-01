using ECondo.Domain.Shared;

namespace ECondo.Domain.Provinces;

public static class ProvinceErrors
{
    public static Error InvalidProvince(string name) => new Error()
    {
        Code = nameof(InvalidProvince),
        Description = $"The province with name {name} was not found",
    };
}
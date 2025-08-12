using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Provinces;

public static class ProvinceErrors
{
    public static Error InvalidProvince(string name) => 
        Error.NotFound("Province.NotFound", 
            $"The province with name {name} was not found");
}
using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Shared;

internal static class EntranceSeedData
{
    public static readonly Entrance Entrance1 = new Entrance
    {
        Id = Guid.Parse("917b8255-12ff-468c-a46f-c1bde3686f0d"),
        BuildingId = BuildingSeedData.TestBuildingId,
        ManagerId = UserSeedData.BasicUser.User.Id,
        Number = "1",
    };

    public static readonly Entrance Entrance2 = new Entrance
    {
        Id = Guid.Parse("7dfaa790-9a53-4ac0-9c38-722043c8bbdc"),
        BuildingId = BuildingSeedData.TestBuildingId,
        ManagerId = UserSeedData.BasicUser.User.Id,
        Number = "2",
    };

    public static readonly IEnumerable<Entrance> Entrances =
    [
        Entrance1,
        Entrance2,
    ];
}
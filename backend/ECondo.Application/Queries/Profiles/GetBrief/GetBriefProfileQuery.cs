using ECondo.Application.Data;

namespace ECondo.Application.Queries.Profiles.GetBrief;

public sealed record GetBriefProfileQuery 
    : IQuery<BriefProfileResult>;

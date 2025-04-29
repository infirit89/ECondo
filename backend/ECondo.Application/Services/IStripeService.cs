namespace ECondo.Application.Services;

public record StripeStatus(bool ChargesEnabled, bool DetailsSubmitted);

public interface IStripeService
{
    Task<string> CreatePaymentIntent(decimal amount, string stripeAccountId);
    Task<string> CreateExpressAccount();
    Task<string> CreateOnboardingAccountLink(string accountId, string returnUri);
    Task<StripeStatus> GetEntranceAccountStatus(string account);
    Task<string> GetLoginLinkForAccount(string accountId);
}
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECondo.Infrastructure.Services;

internal sealed class StripeService : IStripeService
{
    public StripeService(IOptions<StripeSettings> stripeOptions)
    {
        var stripeSettings = stripeOptions.Value;
        StripeConfiguration.ApiKey = stripeSettings.SecretKey;
    }
    
    public async Task<string> CreatePaymentIntent(decimal amount, string stripeAccountId)
    {
        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(new PaymentIntentCreateOptions()
        {
            Amount = (long)(amount * 100),
            Currency = "bgn",
            PaymentMethodTypes = [ "card" ],
            TransferData = new PaymentIntentTransferDataOptions()
            {
                Destination = stripeAccountId,
            },
        });

        return paymentIntent.ClientSecret;
    }

    public async Task<string> CreateExpressAccount()
    {
        var accountService = new AccountService();
        var account = await accountService.CreateAsync(new AccountCreateOptions()
        {
            Type = "express",
        });

        return account.Id;
    }

    public async Task<string> CreateOnboardingAccountLink(string accountId, string returnUri)
    {
        var accountLinkService = new AccountLinkService();
        var accountLink = await accountLinkService.CreateAsync(new AccountLinkCreateOptions()
        {
            Account = accountId,
            RefreshUrl = $"{returnUri}/stripe/reauth",
            ReturnUrl = $"{returnUri}/stripe/success",
            Type = "account_onboarding",
        });

        return accountLink.Url;
    }

    public async Task<StripeStatus> GetEntranceAccountStatus(string accountId)
    {
        var accountService = new AccountService();
        var account = await accountService.GetAsync(accountId);
        return new StripeStatus(account.ChargesEnabled, account.DetailsSubmitted);
    }

    public async Task<string> GetLoginLinkForAccount(string accountId)
    {
        var loginLinkService = new AccountLoginLinkService();
        var loginLink = await loginLinkService.CreateAsync(accountId);
        return loginLink.Url;
    }
}
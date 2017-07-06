﻿namespace Virgil.SDK.Tests
{
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    using Virgil.SDK.Common;
    using Virgil.SDK.Client;
    using Virgil.SDK.Cryptography;

    public class IntegrationHelper
    {
        public static CardsClient GetCardsClient()
        {
            var parameters = new CardsClientParams(AppAccessToken);
            
            parameters.SetCardsServiceAddress(ConfigurationManager.AppSettings["virgil:CardsServicesAddress"]);
            parameters.SetReadCardsServiceAddress(ConfigurationManager.AppSettings["virgil:CardsReadServicesAddress"]);

            var client = new CardsClient(parameters);

            return client;
        }

        public static IdentityClient GetIdentityClient()
        {
            var parameters = new IdentityClientParams();

            parameters.SetIdentityServiceAddress(ConfigurationManager.AppSettings["virgil:IdentityServiceAddress"]);

            var client = new IdentityClient(parameters);

            return client;
        }

        public static string AppID => ConfigurationManager.AppSettings["virgil:AppID"];
        public static byte[] AppKey => File.ReadAllBytes(ConfigurationManager.AppSettings["virgil:AppKeyPath"]);
        public static string AppKeyPath => ConfigurationManager.AppSettings["virgil:AppKeyPath"];
        public static string AppKeyPassword = ConfigurationManager.AppSettings["virgil:AppKeyPassword"];
        public static string AppAccessToken = ConfigurationManager.AppSettings["virgil:AppAccessToken"];

        public static VirgilApiContext VirgilApiContext()
        {
            var cardsParameters = new CardsClientParams(AppAccessToken);

            cardsParameters.SetCardsServiceAddress(ConfigurationManager.AppSettings["virgil:CardsServicesAddress"]);
            cardsParameters.SetReadCardsServiceAddress(ConfigurationManager.AppSettings["virgil:CardsReadServicesAddress"]);
            cardsParameters.SetRAServiceAddress(ConfigurationManager.AppSettings["virgil:RAServicesAddress"]);

            var identityParameters = new IdentityClientParams();
            identityParameters.SetIdentityServiceAddress(ConfigurationManager.AppSettings["virgil:IdentityServiceAddress"]);


            return new VirgilApiContext
            {
                CardsClientParams = cardsParameters,
                IdentityClientParams = identityParameters,
                Credentials = new AppCredentials
                {
                    AppKey = VirgilBuffer.From(AppKey),
                    AppKeyPassword = AppKeyPassword,
                    AppId = AppID
                }
            };
        }

        public static async Task RevokeCard(string cardId)
        {
            var client = GetCardsClient();
            var crypto = new VirgilCrypto();
            var requestSigner = new RequestSigner(crypto);

            var appKey = crypto.ImportPrivateKey(AppKey, AppKeyPassword);

            var revokeRequest = new RevokeCardRequest(cardId, RevocationReason.Unspecified);
            requestSigner.AuthoritySign(revokeRequest, AppID, appKey);

            await client.RevokeUserCardAsync(revokeRequest);
        }

        public static string RandomText =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eget ipsum augue. " +
                "Nulla facilisi. Praesent eu laoreet felis. Vivamus scelerisque justo magna, variu" +
                "s pharetra augue ultricies non. Vestibulum convallis in urna nec vehicula. Maecen" +
                "as mattis suscipit cursus. In iaculis dui ut quam molestie, vel facilisis lacus s" +
                "celerisque. Morbi at fermentum felis. Vivamus tincidunt ultricies feugiat. In hac" +
                " habitasse platea dictumst. Donec metus diam, luctus et nisi ac, ornare tincidunt" +
                " mauris. Fusce posuere lacus non ligula cursus convallis. Proin facilisis ut diam" +
                " in cursus. Vivamus in velit a nisi rhoncus vestibulum."                           +
                "Curabitur blandit aliquam ex, et pellentesque elit pharetra at. Donec elementum r" +
                "honcus finibus. Duis risus est, porttitor vitae ante vel, sagittis tincidunt veli" +
                "In enim ipsum, sagittis at vehicula ut, accumsan nec sapien. Vestibulum conse"     +
                "ctetur eros a nulla tristique, in lobortis nibh consectetur. Phasellus tincidunt " +
                "luctus bibendum. Praesent orci turpis, luctus eget interdum ultricies, vestibulum" +
                " a eros. Phasellus sagittis vulputate justo ut condimentum.";
    }
}
﻿namespace Virgil.SDK.Http
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Virgil.SDK.Exceptions;

    /// <summary>
    ///     A connection for making HTTP requests against URI endpoints for public api services.
    /// </summary>
    /// <seealso cref="ConnectionBase" />
    /// <seealso cref="IConnection" />
    public class PublicServicesConnection : ConnectionBase, IConnection
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PublicServicesConnection" /> class.
        /// </summary>
        /// <param name="accessToken">Application token</param>
        /// <param name="baseAddress">The base address.</param>
        public PublicServicesConnection(string accessToken, Uri baseAddress) : base(accessToken, baseAddress)
        {
            this.Errors = new Dictionary<int, string>
            {
                [10000] = "Internal application error",
                [10100] = "JSON specified as a request body is invalid",
                [20100] = "The request ID header was used already",
                [20101] = "The request ID header is invalid",
                [20200] = "The request sing header not found",
                [20201] = "The VirgilCard ID header not specified or incorrect",
                [20202] = "The request sign header is invalid",
                [20203] = "Public Key value is required in request body",
                [20204] = "Public Key value in request body must be base64 encoded value",
                [20205] = "Public Key IDs in URL part and X-VIRGIL-REQUEST-SIGN-VIRGIL-CARD-ID header must match",
                [20206] = "The public key id in the request body is invalid ",
                [20207] = "Public Key IDs in Request and X-VIRGIL-REQUEST-SIGN-VIRGIL-CARD-ID header must match",
                [20208] = "Virgil card ids in url and authentication header must match",
                [20300] = "The Virgil application token was not specified or invalid",
                [20301] = "The Virgil statistics application error",
                [30001] = "The entity not found by specified ID",
                [30100] = "Public Key object not found by specified ID",
                [30101] = "Public key length invalid",
                [30102] = "Public key must be base64-encoded string",
                [30200] = "Identity object is not found for id specified",
                [30201] = "Identity type is invalid. Valid types are: 'email', 'application'",
                [30202] = "Email value specified for the email identity is invalid",
                [30203] = "Cannot create unconfirmed application identity",
                [30300] = "Virgil Card object not found for id specified",
                [30301] = "Virgil Card custom data list must be an array",
                [30302] = "Virgil Card custom data entries cannot start with reserved \"vc_\" prefix",
                [30303] = "Virgil Card custom data entries cannot have empty and too long keys",
                [30304] = "Virgil Card custom data entries keys contains invalid characters",
                [30305] = "Virgil Card custom data entry value length validation failed",
                [30400] = "Sign object not found for id specified",
                [30402] = "The signed digest value is invalid",
                [30403] = "Sign Signed digest must be base64 encoded string",
                [30404] = "Cannot save the Sign because it exists already",
                [31000] = "Value search parameter is mandatory",
                [31010] = "Search value parameter is mandatory for the application search",
                [31020] = "VirgilCard's signs parameter must be an array",
                [31030] = "Identity validation token is invalid",
                [31040] = "VirgilCard revokation parameters do not match VirgilCard's identity",
                [31050] = "Virgil Identity service error",
                [31060] = "Identities parameter is invalid",
                [31070] = "Identity validation failed",
            };
        }

        /// <summary>
        ///     Handles public keys service exception responses
        /// </summary>
        /// <param name="message">The http response message.</param>
        protected override void ExceptionHandler(HttpResponseMessage message)
        {
            this.ThrowException(message, (code, msg) => new VirgilPublicServicesException(code, msg));
        }
    }
}
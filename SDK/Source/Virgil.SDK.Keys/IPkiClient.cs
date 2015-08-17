﻿namespace Virgil.SDK.Keys
{
    using Virgil.SDK.Keys.Clients;

    /// <summary>
    /// A Client for the Virgil PKI Client for API v1. You can read more about
    /// the api here: http://developer.virgilsecurity.com.
    /// </summary>
    public interface IPkiClient
    {
        /// <summary>
        /// Gets the public keys client implementation.
        /// </summary>
        /// <value>
        /// The public keys client.
        /// </value>
        IPublicKeysClient PublicKeys { get; }

        /// <summary>
        /// Gets the user data client implementation.
        /// </summary>
        /// <value>
        /// The user data client.
        /// </value>
        IUserDataClient UserData { get; }
    }
}
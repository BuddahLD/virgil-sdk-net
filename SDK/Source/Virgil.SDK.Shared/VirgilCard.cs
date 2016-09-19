﻿namespace Virgil.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Virgil.SDK.Client;
    using Virgil.SDK.Client.Models;
    using Virgil.SDK.Cryptography;
    using Virgil.SDK.Exceptions;

    /// <summary>
    /// A Virgil Card is the main entity of the Virgil Security services, it includes an information about the user 
    /// and his public key. The Virgil Card identifies the user by one of his available types, such as an email, 
    /// a phone number, etc.
    /// </summary>
    public sealed class VirgilCard 
    {
        private readonly VirgilCardModel model;
        private IPublicKey publicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirgilCard"/> class.
        /// </summary>
        internal VirgilCard(VirgilCardModel model)
        {
            this.model = model;
            if (this.model.Data != null)
            {
                this.Data = new ReadOnlyDictionary<string, string>(this.model.Data);
            }
        }

        /// <summary>
        /// Gets the unique identifier for the Virgil Card.
        /// </summary>
        public string Id => this.model.Id;

        /// <summary>
        /// Gets the value of current Virgil Card identity.
        /// </summary>
        public string Identity => this.model.Identity;

        /// <summary>
        /// Gets the type of current Virgil Card identity.
        /// </summary>
        public string IdentityType => this.model.IdentityType;
        
        /// <summary>
        /// Gets the Public Key of current Virgil Card.
        /// </summary>
        public IPublicKey PublicKey 
        {
            get
            {
                if (this.publicKey != null)
                {
                    return this.publicKey;
                }

                var crypto = ServiceLocator.Resolve<ICrypto>();
                this.publicKey = crypto.ImportPublicKey(this.model.PublicKey);

                return this.publicKey;
            }
        }
        
        /// <summary>
        /// Gets the custom <see cref="VirgilCard"/> parameters.
        /// </summary>
        public IReadOnlyDictionary<string, string> Data { get; private set; }

        /// <summary>
        /// Encrypts the specified data for current <see cref="VirgilCard"/> recipient.
        /// </summary>
        /// <param name="data">The data to be encrypted.</param>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var crypto = ServiceLocator.Resolve<ICrypto>();
            var cipherdata = crypto.Encrypt(data, this.PublicKey);

            return cipherdata;
        }

        /// <summary>
        /// Verifies the specified data and signature with current <see cref="VirgilCard"/> recipient.
        /// </summary>
        /// <param name="data">The data to be verified.</param>
        /// <param name="signature">The signature used to verify the data integrity.</param>
        public bool Verify(byte[] data, byte[] signature)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            var crypto = ServiceLocator.Resolve<ICrypto>();
            var isValid = crypto.Verify(data, signature, this.PublicKey);

            return isValid;
        }

        /// <summary>
        /// Gets the <see cref="VirgilCard"/> by specified identifier.
        /// </summary>
        /// <param name="cardId">The identifier that represents a <see cref="VirgilCard"/>.</param>
        public static async Task<VirgilCard> GetAsync(string cardId)
        {
            var client = ServiceLocator.Resolve<IVirgilClient>();
            var virgilCardDto = await client.GetAsync(cardId);

            if (virgilCardDto == null)
            {
                throw new VirgilCardIsNotFoundException();
            }

            return new VirgilCard(virgilCardDto);
        }

        /// <summary>
        /// Finds the <see cref="VirgilCard" />s in global scope by specified criteria.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="type">Type of the identity.</param>
        /// <returns>
        /// A list of found <see cref="VirgilCard" />s.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static Task<IEnumerable<VirgilCard>> FindGlobalAsync
        (
            string identity,
            GlobalIdentityType type = GlobalIdentityType.Email
        )
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));
            
            return FindGlobalAsync(new[] { identity }, type);
        }

        /// <summary>
        /// Finds the <see cref="VirgilCard" />s in global scope by specified criteria.
        /// </summary>
        /// <param name="identities">The identity.</param>
        /// <param name="type">Type of the identity.</param>
        /// <returns>
        /// A list of found <see cref="VirgilCard" />s.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<IEnumerable<VirgilCard>> FindGlobalAsync
        (
            IEnumerable<string> identities,
            GlobalIdentityType type = GlobalIdentityType.Email
        )
        {
            if (identities == null)
                throw new ArgumentNullException(nameof(identities));

            var client = ServiceLocator.Resolve<IVirgilClient>();

            var criteria = new SearchCardsCriteria
            {
                Identities = identities,
                IdentityType = type.ToString().ToLower(),
                Scope = VirgilCardScope.Global
            };

            var cards = await client.SearchCardsAsync(criteria).ConfigureAwait(false);

            return cards.Select(c => new VirgilCard(c)).ToList();
        }

        /// <summary>
        /// Finds the <see cref="VirgilCard" />s by specified criteria.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="type">Type of the identity.</param>
        /// <returns>
        /// A list of found <see cref="VirgilCard" />s.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static Task<IEnumerable<VirgilCard>> FindAsync
        (
            string identity,
            string type = null
        )
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            return FindAsync(new[] {identity}, type);
        }

        /// <summary>
        /// Finds the <see cref="VirgilCard" />s by specified criteria.
        /// </summary>
        /// <param name="identities">The identities.</param>
        /// <param name="type">Type of the identity.</param>
        /// <returns>
        /// A list of found <see cref="VirgilCard" />s.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static async Task<IEnumerable<VirgilCard>> FindAsync
        (
            IEnumerable<string> identities, 
            string type = null
        )
        {
            var identityList = identities as IList<string> ?? identities.ToList();

            if (identities == null || !identityList.Any())
                throw new ArgumentNullException(nameof(identities));

            var client = ServiceLocator.Resolve<IVirgilClient>();

            var criteria = new SearchCardsCriteria
            {
                Identities = identityList,
                IdentityType = type,
                Scope = VirgilCardScope.Global
            };

            var cardModels = await client.SearchCardsAsync(criteria).ConfigureAwait(false);

            return cardModels.Select(model => new VirgilCard(model)).ToList();
        }
    }
}
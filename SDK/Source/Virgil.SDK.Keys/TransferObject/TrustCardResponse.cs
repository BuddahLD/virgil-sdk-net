﻿namespace Virgil.SDK.TransferObject
{
    using System;
    using Newtonsoft.Json;

    public class TrustCardResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("signer_virgil_card_id")]
        public Guid SignerVirgilCardId { get; set; }

        [JsonProperty("signed_virgil_card_id")]
        public Guid SignedVirgilCardId { get; set; }

        [JsonProperty("signed_digest")]
        public byte[] SignedDigest { get; set; }
    }
}
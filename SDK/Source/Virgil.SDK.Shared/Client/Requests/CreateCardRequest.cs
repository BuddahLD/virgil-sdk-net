﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virgil.SDK.Client.Models;
using Virgil.SDK.Common;
using Virgil.SDK.Cryptography;

namespace Virgil.SDK.Client.Requests
{
    public abstract class CreateCardRequest : SignedRequest
    {
        private string identity;
        private byte[] publicKeyData;
        private IReadOnlyDictionary<string, string> customFields;
        protected string identityType;
        protected CardInfoModel info;    
        protected CardScope scope;

        public string Identity
        {
            get { return this.identity; }
            set
            {
                if (this.IsSnapshotTaken)
                {
                    throw new InvalidOperationException();
                }

                this.identity = value;
            }
         }

        public byte[] PublicKeyData
        {
            get { return this.publicKeyData; }
            set
            {
                if (this.IsSnapshotTaken)
                {
                    throw new InvalidOperationException();
                }

                this.publicKeyData = value;
            }
            }
        public IReadOnlyDictionary<string, string> CustomFields
        {
            get { return this.customFields; }
            set
            {
                if (this.IsSnapshotTaken)
                {
                    throw new InvalidOperationException();
                }

                this.customFields = value;
            }
        }

        public CardInfoModel Info
        {
            get { return this.info; }
            set
            {
                if (this.IsSnapshotTaken)
                {
                    throw new InvalidOperationException();
                }

                this.info = value;
            }
        }


        protected virtual void RestoreFromSnapshot(byte[] snapshot)
        {
            var snapshotMaster = new SnapshotMaster();
            var model = snapshotMaster.ParseSnapshot<CardSnapshotModel>(snapshot);

            this.Identity = model.Identity;
            this.PublicKeyData = model.PublicKeyData;
            this.CustomFields = model.CustomFields;
            this.identityType = model.IdentityType;
            this.scope = model.Scope;
        }
        protected override byte[] CreateSnapshot()
        {
            var snapshotMaster = new SnapshotMaster();
            var model = new CardSnapshotModel
            {
                Identity = this.Identity,
                PublicKeyData = this.PublicKeyData,
                CustomFields = this.CustomFields,
                IdentityType = this.identityType,
                Scope = this.scope
            };

            return snapshotMaster.TakeSnapshot(model);
        }

        protected override bool IsValidData()
        {
            return (this.identity != null && this.publicKeyData != null && this.identityType != null);
        }

        public virtual void Import(string exportedRequest)
        {
            var jsonRequestModel = Encoding.UTF8.GetString(Convert.FromBase64String(exportedRequest));
            var requestModel = JsonSerializer.Deserialize<SignableRequestModel>(jsonRequestModel);

            this.RestoreFromSnapshot(requestModel.ContentSnapshot);
            this.signatures = requestModel.Meta.Signatures;

        }

        public virtual string Export()
        {
            var requestModel = this.GetRequestModel();

            var json = JsonSerializer.Serialize(requestModel);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            return base64;
        }

        public void SelfSign(ICrypto crypto, IPrivateKey privateKey)
        {
            var snapshotFingerprint = crypto.CalculateFingerprint(this.Snapshot);

            this.Sign(crypto, snapshotFingerprint.ToHEX(), privateKey);
        }

  
    }
}

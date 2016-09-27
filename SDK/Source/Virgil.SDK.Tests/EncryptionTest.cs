﻿namespace Virgil.SDK.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cryptography;
    using FluentAssertions;
    using NUnit.Framework;

    public class EncryptionTest
    {
        [Test]
        public void EncryptData_SinglePublicKeyGiven_ShouldBeDecrypted()
        {
            var crypto = new VirgilCrypto();

            var keyPair = crypto.GenerateKeys();

            var data = Encoding.UTF8.GetBytes("Encrypt me!!!");
            var encryptedData = crypto.Encrypt(data, keyPair.PublicKey);
            var decryptedData = crypto.Decrypt(encryptedData, keyPair.PrivateKey);
                
            data.ShouldAllBeEquivalentTo(decryptedData);
        }

        [Test]
        public void EncryptData_MultiplePublicKeysGiven_ShouldBeDecrypted()
        {
            var crypto = new VirgilCrypto();
            var keyPairs = new List<KeyPair>();

            for (var index = 0; index < 10; index++)
            {
                keyPairs.Add(crypto.GenerateKeys());
            }

            var data = Encoding.UTF8.GetBytes("Encrypt me!!!");
            var encryptedData = crypto.Encrypt(data, keyPairs.Select(it => it.PublicKey).ToArray());

            foreach (var keyPair in keyPairs)
            {
                var decryptedData = crypto.Decrypt(encryptedData, keyPair.PrivateKey);
                data.ShouldAllBeEquivalentTo(decryptedData);
            }
        }

        [Test]
        public void EncryptData_MultiplePublicKeysWithDifferentTypesGiven_ShouldBeDecrypted()
        {
            var crypto = new VirgilCrypto();
            var keyPairs = new List<KeyPair>
            {
                crypto.GenerateKeys(KeysType.EC_SECP256R1),
                crypto.GenerateKeys(KeysType.EC_SECP384R1),
                crypto.GenerateKeys(KeysType.EC_SECP521R1),
                crypto.GenerateKeys(KeysType.EC_BP256R1),
                crypto.GenerateKeys(KeysType.EC_BP384R1),
                crypto.GenerateKeys(KeysType.EC_BP512R1),
                crypto.GenerateKeys(KeysType.EC_SECP256K1),
                crypto.GenerateKeys(KeysType.EC_CURVE25519),
                crypto.GenerateKeys(KeysType.FAST_EC_ED25519),
                crypto.GenerateKeys(KeysType.FAST_EC_X25519)
            };

            var data = Encoding.UTF8.GetBytes("Encrypt me!!!");
            var encryptedData = crypto.Encrypt(data, keyPairs.Select(it => it.PublicKey).ToArray());

            foreach (var keyPair in keyPairs)
            {
                var decryptedData = crypto.Decrypt(encryptedData, keyPair.PrivateKey);
                data.ShouldAllBeEquivalentTo(decryptedData);
            }
        }

        [Test]
        public void SignThenEncryptData_MultiplePublicKeysAndPrivateKeyGiven_ShouldBeDecryptedThenVerified()
        {
        }
    }
}
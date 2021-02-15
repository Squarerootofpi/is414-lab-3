using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace steedcoin
{
    class Transaction
    {
        public PublicKey FromAddress { get; set; }
        public PublicKey ToAddress { get; set; }
        public decimal Amount { get; set; }
        public Signature Signature { get; set; }
        public Transaction(PublicKey from, PublicKey to, decimal amount)
        {
            this.FromAddress = from;
            this.ToAddress = to;
            this.Amount = amount;

        }
        public void SignTransaction(PrivateKey signingKdy)
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string signAddressDER = BitConverter.ToString(signingKdy.publicKey().toDer()).Replace("-", "");

            if(fromAddressDER != signAddressDER)
            {
                throw new Exception("You can't sign transactionf or another wallet!");
            }

            string toHash = this.CalculateHash();
            this.Signature = Ecdsa.sign(toHash, signingKdy);
        }

        public string CalculateHash()
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string toAddressDER = BitConverter.ToString(ToAddress.toDer()).Replace("-", "");

            string transactionData = fromAddressDER + toAddressDER + Amount;
            byte[] tdBytes = Encoding.ASCII.GetBytes(transactionData);
            return BitConverter.ToString(SHA256.Create().ComputeHash(tdBytes)).Replace("-", "");
        }

        public bool IsValid()
        {
            if (this.FromAddress is null)
            {
                return true;
            }
            if (this.Signature is null)
            {
                throw new Exception("No signature in this transaction.");
            }
            return Ecdsa.verify(this.CalculateHash(), this.Signature, this.FromAddress);
        }
    }
}

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
    class Program
    {
        static void Main(string[] args)
        {
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();


            BlockChain steedcoin = new BlockChain(6, 100);

            Console.WriteLine("start the miner.");
            steedcoin.MinePendingTransactions(wallet1);
            decimal bal1 = steedcoin.GetBalanceInWallet(wallet1);
            Console.WriteLine(bal1);

            //steedcoin.AddBlock(new Block(1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 50"));
            //steedcoin.AddBlock(new Block(2, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 200"));
            Transaction tx1 = new Transaction(wallet1, wallet2, 10);
            tx1.SignTransaction(key1);
            steedcoin.addPendingTransaction(tx1);
            Console.WriteLine("Stating the miner for real.");
            steedcoin.MinePendingTransactions(wallet2);
            Console.WriteLine("\nBalance of wallet1 is: " + steedcoin.GetBalanceInWallet(wallet1).ToString());
            Console.WriteLine("\nBalance of wallet2 is: " + steedcoin.GetBalanceInWallet(wallet2).ToString());


            string blockJson = JsonConvert.SerializeObject(steedcoin, Formatting.Indented);
            Console.WriteLine(blockJson);

            if (steedcoin.IsValidChain()) 
            {
                Console.WriteLine("Valid");
            }
            else
            {
                Console.WriteLine("INVALID BLOCKCHAIN");
            }
            Console.WriteLine(blockJson);
        }
    }

    

   
}

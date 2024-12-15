using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugBot.Objects;
using Discord.Rest;
using static System.Windows.Forms.Design.AxImporter;

namespace BugBot.Managers.Database
{
    internal static class Database_Handler
    {

        private static Dictionary<string, Dictionary<string, Card>> cardMap = new Dictionary<string, Dictionary<string, Card>>();//name of the card is the key, value is another dictionary with all cards with that name
        //second dictionary has the rarity as the key, e.g C, R1, R2, U, and the card as the value. 

        private static Dictionary<string, Card> referenceMap = new Dictionary<string, Card>(); //allows cards to be searched by reference

        public static void addCards(List<Card> cardList)
        {
            RegexOptions options = RegexOptions.RightToLeft;
            string pattern = "_(C|U\\d*|R\\d*)$";
            foreach (Card card in cardList)
            {
                card.CleanName();
                //Console.WriteLine($"Adding card {card.getName()}, with reference {card.getReference()}.");
                referenceMap.Add(card.getReference(), card);
                
                string rarityCode = Regex.Match(card.getReference(), pattern, options, TimeSpan.FromMilliseconds(50)).Groups[1].Captures[0].Value;
                if (!cardMap.ContainsKey(card.getCleanName()))
                { 
                    Dictionary<string, Card> newDict = new Dictionary<string, Card>();
                    newDict.Add(rarityCode, card);

                    cardMap.Add(card.getCleanName(), newDict);             
                    Trie.Insert(card.getCleanName());
                }
                else if (!cardMap[card.getCleanName()].ContainsKey(rarityCode))
                {
                    cardMap[card.getCleanName()].Add(rarityCode, card);
                }
            }
        }

        public static void addCard(Card card)
        {
            RegexOptions options = RegexOptions.RightToLeft;
            string pattern = "_(C|U\\d*|R\\d)$";
            string rarityCode = Regex.Match(card.getReference(), pattern, options, TimeSpan.FromMilliseconds(50)).Groups[1].Captures[0].Value;
            card.CleanName();
            if (!cardMap.ContainsKey(card.getCleanName()))
            {
                Dictionary<string, Card> newDict = new Dictionary<string, Card>();
                newDict.Add(rarityCode, card) ;
                cardMap.Add(card.getCleanName(), newDict);
                Trie.Insert(card.getCleanName());
            }
            else
            {
                cardMap[card.getCleanName()].Add(rarityCode, card);
            }
        }

        public static Card getCard(string name, string rarityCode) //C for common, R1 for the in faction rare, R2 for out of faction
        {
            if (cardMap.ContainsKey(name) )
            {
                if (cardMap[name].ContainsKey(rarityCode))
                {
                    return cardMap[name][rarityCode];
                }
                
            }
            return null;
        }

        public static int getCardCount()
        {
            return cardMap.Count;
        }

        public static Card getCardByReference(string reference)
        {
            reference = reference.ToUpper();
            if (referenceMap.ContainsKey(reference))
            {
                return referenceMap[reference];
            }
            //Console.WriteLine("Card with reference: " + reference + " Not found");
            return null;
        }
    }
}

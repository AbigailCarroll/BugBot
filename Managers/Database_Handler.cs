using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BugBot.Objects;
using Discord.Rest;

namespace BugBot.Managers
{
    internal static class Database_Handler
    {

        private static Dictionary<string, List<Card>> cardMap = new Dictionary<string, List<Card>>();//name of the card is the key, value is a vector of all cards with that name (i.e the normal, rare and colour shifted rare versions)

        public static void addCards(List<Card> cardList)
        {
            foreach (Card card in cardList)
            {
                card.CleanName();
                if (!cardMap.ContainsKey(card.getName()))
                {

                    List<Card> newList = new List<Card>();
                    newList.Add(card);
                    cardMap.Add(card.getName(), newList);
                    
                }
                else
                {
                    cardMap[card.getName()].Add(card);
                }
            }
        }

        public static void addCard(Card card)
        {
            if (!cardMap.ContainsKey(card.getName()))
            {
                List<Card> newList = new List<Card>();
                newList.Add(card);
                cardMap.Add(card.getName(), newList);
                
            }
            else
            {
                cardMap[card.getName()].Add(card);
            }
        }

        public static Card getCard(string name, int version) //0 = common, 1 = rare, 2 = colour shifted rare.
        {
            if (cardMap.ContainsKey(name))
            {
                return cardMap[name][version];
            }
            else
            {
                Console.WriteLine($"Error fetching card {name}, with version {version} from cardMap, returned placeholder instead");
                //testing
                
                Console.WriteLine($"cardMap count: {cardMap.Count}");
                List<string> keyList = new List<string>(cardMap.Keys);

                foreach (string key in keyList)
                {
                    Console.WriteLine(key);
                }

                //testing
                return cardMap["manaorb"][0];

            }
        }

        public static int getCardCount()
        {
            return cardMap.Count;
        }
    }
}

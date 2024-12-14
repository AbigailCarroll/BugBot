using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugBot.Objects
{
    internal class Card_DataBase 
    {
        public Card_DataBase()
        {
            cardMap = new Dictionary<string, List<Card>>();
        }

        public Card_DataBase(List<Card> cardList)
        {
            cardMap = new Dictionary<string, List<Card>>();
            foreach (Card card in cardList)
            {
                if (!cardMap.ContainsKey(card.getCleanName()))
                {
                    List<Card> newList = new List<Card>();
                    newList.Add(card);
                    cardMap.Add(card.getCleanName(), newList);
                    Console.WriteLine($"added {card.getCleanName()}, with reference {card.getReference()} to database");
                }
                else
                {
                    cardMap[card.getCleanName()].Add(card);
                }  
            }

            for (int i = 0; i < cardMap["Kelon Elemental"].Count(); i++)
            {
                Console.WriteLine(cardMap["Kelon Elemental"][i].getReference());
            }
        }
        private Dictionary<string, List<Card>> cardMap;  //name of the card is the key, value is a vector of all cards with that name (i.e the normal, rare and colour shifted rare versions)
    }
}

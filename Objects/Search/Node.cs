using Discord.Audio.Streams;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugBot.Managers;


namespace BugBot.Objects.Search
{
    
    internal class Node
    {
        private Node[] children = new Node[26];
        private bool teminal;
        public const string CARD_NOT_FOUND = "";

        public Node()
        {
            for (int i = 0; i < children.Length; i++)
            {
                this.children[i] = null;
            }
            this.teminal = false;
        }

        public void Insert(string cardName)
        {
            cardName = cardName.ToLower();
            cardName = Handle_Input.Clean_Input(cardName);
            Insert_(cardName);
        }

        private void Insert_(string cardName)
        {
            
            Console.WriteLine($"inserting {cardName}");

            int reference = cardName[0] - 'a';
            if (children[reference] == null)
            {
                children[reference] = new Node();
            }
            cardName = cardName.Substring(1);
            if(cardName.Length > 0)
            {
                children[reference].Insert_(cardName);
            }
            
            else
            {
                children[reference].teminal = true;
            }
            

        }

        private string AutoComplete(string output) //when the cardName value has been exhausted will attempt to autocomplete the card name by traversing the trie
            //will only return a positive result if the autocomplete is unambiguous, so there is only one path to traverse
            //could make this more efficient my minimising the trie but i cba right now.
        {
            Console.WriteLine("Attempting autocomplete of string " + output);
            int childFound = -1;
            for (int i = 0; i < this.children.Length; i++)
            {
                if (this.children[i] != null)
                {
                    if(childFound != -1)
                    {
                        return CARD_NOT_FOUND; //card not found/intended card is ambiguous
                    }
                    childFound = i;
                }
            }
            if (this.teminal == true)
            {
                return output;
            }
            output += Convert.ToChar(childFound +'a');
            return children[childFound].AutoComplete(output);
        }

        private string Find_(string cardName, string output) //output builds the card name that can then be used to get it's details through the dictionary 
        {
            int reference = cardName[0] - 'a';
            output += cardName[0];
            cardName = cardName.Substring(1);
            if (reference < 0 || reference > 26)
            {
                throw new Exception($"reference was outside bounds at {reference}");
            }
            if (children[reference] == null)
            {
                return CARD_NOT_FOUND; //card not found.
            }
            if(cardName.Length > 0)
            {
                return children[reference].Find_(cardName, output);
            }
            else if (children[reference].teminal == true)
            {
                return output;
            }
            else //children[reference].teminal == false
            {
                return children[reference].AutoComplete(output);
            }
        }

        public string Find(string cardName)
        {
            cardName = cardName.ToLower();
            cardName = Handle_Input.Clean_Input(cardName);

            return Find_(cardName, "");
        }

        
    }
}

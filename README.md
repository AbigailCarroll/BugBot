A discord bot to fetch data about Altered TCG cards

The bot is capable of fetching cards based on either name or reference. To trigger the bot surround the name of the card you want in greater than and less than signs. For example, if I wanted the card Kelon Elemental, I would type ```<<Kelon Elemental>>```. Or I could instead use the card's reference, for example ```<<ALT_CORE_B_AX_04_R2>>``` would return the Muna version of Kelon Elemental.

The bot is not case sensitive, and when searching for a card with it's name, the bot will try to complete the card for you if there is only one card that matches the characters provided.
for example: ```<<Kelon Ele>>``` will return card data for Kelon Elemental, but ```<<Kelon>>``` could referr to Kelon Burst, Kelon Cylinder etc, and so will return a Card Not Found error.

After the card name you can add tokens within the LT/GT signs to specify the specific version of the card you want, tokens are separated from the card name with "|". If you are searching for a card by it's reference you do not need to add tokens

these tokens are: 

"C" for the common version, though this is the default so shouldn't be necessary.

"R" / "R1" for the rare version.

"R2" / "OOF" / "OOC" / "CS" for the out of faction/colour shifted rare version of the card.

e.g: ```<<Kelon Elemental|R1>>``` will provide the rare version of a card, ```<<Kelon Elemental|OOC>>``` and ```<<Kelon Elemental|R2>>``` will both provide the out of faction version (Muna in this case)

These tokens are also not case sensitive. Supplying an invalid token will return an Invalid Token error.

The bot also takes tokens to specify the language of the results, different languages are not currently supported so they will have no effect on the output, but will not return an error.

Languages supported by Altered are: "FR", "DE", "ES", "IT", and "EN", this bot's default is EN

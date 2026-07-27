<p align="center"><img src="https://i.ibb.co/4mrqB2T/mtgl-logo.png" alt="mtgl-logo" border="0"></p>

# MTG-Librarian
MTG Librarian is a Magic: the Gathering card library and collection tracker, inspired by *Magic Assistant*.

## Features
1. Full library of Magic cards, beyond booster pack sets and promos
2. Easy drag-and-drop interface
3. Up-to-date card and pricing data from Scryfall
4. Card search and filtering
5. Simulator
6. Collection price histories
7. Deck editor
8. Deck import/export (Magic Online and Arena formats)
9. CSV collection import/export. Supported import formats include MTGGoldfish, Magic Online, EchoMTG, MTG Collection Builder, Archidekt, Deckbox, Deckstats, Topdecked, UrzaGatherer, ManaBox, Tappedout, Dragon Shield, Moxfield, and MTG Studio.

## Getting Started
On first run, the application will update its list of available sets and begin downloading set icons. To add cards to the main collection, first search using the Catalog form. You may drag-and-drop cards from the list view, double-click, or add them by right-clicking. New collections are created by right-clicking a group in the Collections form. Double-clicking a collection will open it. Cards can be moved between collections using drag-and-drop. To edit card details, double-click the field you wish to edit. Card quantities can also be increased/decreased by selecting the card and using the =/- keys. It is possible to edit multiple cards at the same time by Shift-selecting or Ctrl-selecting and, while still holding the Shift or Ctrl keys, double-clicking the field you wish to edit. You may combine and split cards using the right-click menu.

Commanders can be selected using the deck editor by right-clicking. Decks can be playtested using the simulator. To do so, open or activate the deck you wish to playtest and select Simulator from the Deck main menu item.

## Importing an Existing Collection
Collections can be imported using the Magic Online DEK format or any supported CSV format. Click Import collection from the File menu and select the file you wish to import. Select the platform of the collection. For most formats, you may import into an existing collection or create a new one. Click the Import button to begin. Depending on the size of the collection, the process may take several minutes as the importer queries Scryfall.

## Automatic Price Updates
The application will automatically update price information after application start, but not more than once per day. Update progress is displayed in the Tasks form. To manually update card prices, select the cards you wish to update in the Collection View form and hit Enter. Prices are also automatically entered when a new card is added to the local catalog, such as when adding a card to a collection.

## Collection Price Histories
Collection snapshots are saved when exiting the application (which saves a snapshot for each collection in the database) or when clicking Price history from the Collection menu (which saves a snapshot for the active collection). The price history chart displays price data points for the collection based on snapshots. Zooming is possible using the mouse wheel, as is panning by clicking and dragging. Cost, price, and card count are displayed.

*Special thanks to the **Scryfall Developers** and to the developer of **Magic Assistant***

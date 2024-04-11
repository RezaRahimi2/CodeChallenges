## Assignment:
Create a simple card game in Unity that has 4 players all playing against each 
other.
One player will be controlled by the user and the other 3 will be controlled by 
the computer. 
A dealer will be randomly assigned every new game and the dealer will start 
dealing cards in a counter clockwise fashion where each player will be dealt 13 
cards.
At of the start of every round, each player throws one card on the ground. After 
each of the 4 players throw a card, the player with highest card on the ground 
(based on the the suit and card number) wins a point, and a new round starts.
A game ends when all 13 rounds are played, and the player with highest amount of 
points wins the game.
The computer controlled players should not have complicated logic, attempting to 
play the best card in hand should be enough.
Suits Order is from low to high:
clubs, diamonds, hearts, spades
Card Numbers from low to high:
two, three, four, five, six, seven, eight, nine, ten, jack, queen, king, ace

### Gameplay
[![Video Title](http://i3.ytimg.com/vi/Gr0BmHi-Bwk/hqdefault.jpg)](https://youtu.be/Gr0BmHi-Bwk)
---------------------------------------------------------------------------------------------------------------
### Theme Customization
[![Video Title](http://i3.ytimg.com/vi/XklXKGjQ5Wc/hqdefault.jpg)](https://youtu.be/XklXKGjQ5Wc)

--------------------------------------------------------------------------------------------------------------
### Documentation: 

### GameController
This class is the main controller of the game. It manages the game state, including the players, the deck of cards, and the game rounds. It also handles the game logic, such as dealing cards, starting rounds, and determining the winner of the game. The GameController uses the Singleton pattern, ensuring that there is only one instance of the GameController in the game.
### GameViewManager
This class is responsible for managing the game's visual elements. It keeps track of the views associated with each player and handles the visual aspects of the game, such as dealing cards and displaying the winner of a round. It subscribes to various events to update the game view accordingly.

### EventManager
This class implements the Observer pattern. It maintains a dictionary of events and provides methods to subscribe to, broadcast, and clean up events. This allows different parts of the game to communicate with each other without being tightly coupled.

### IPlayer and AIPlayer And HumanPlayer
These classes represent the players in the game. The IPlayer interface defines the common behaviors of a player, and the AIPlayer class implements these behaviors for an AI player.
ArcLayout: This class is a MonoBehaviour that arranges its children in an arc. It's used to display the cards in a player's hand in a visually pleasing way.

### CardView
This class represents the visual aspect of a card. It updates the card's appearance based on the card model and handles user interactions with the card.

### DealerView
This class represents the dealer in the game. It handles the visual aspects of dealing cards to the players.
TableView: This class represents the table where the game is played. It manages the visual aspects of the table, such as displaying the cards played in a round and collecting the cards won by a player.

### Events
These events are used by the EventManager to facilitate communication between different parts of the game.

The design of these classes follows the Model-View-Controller (MVC) pattern. The GameController acts as the model, managing the game's state and logic. The GameViewManager, CardView, DealerView, and TableView classes act as the view, displaying the game's state to the user. The EventManager acts as the controller, handling user input and updating the model and view accordingly.
The design also follows the Component-Based pattern. Each class is a component that handles a specific aspect of the game. This makes the code modular and reusable, as each component can be developed and tested independently. It also allows for flexibility in game design, as components can be added, removed, or modified without affecting other components.
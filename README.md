## Assignment:
Your task is to create a Unity scene that encompasses several key aspects of Unity development. You will be
responsible for building a game scene with a user interface, simple animations, interactions, and
integrating Unity's Addressables system.
Expected Output
Design a single Unity scene that combines the following features:
● A simple UI with a Roll button to roll a virtual die and a Reset button to
return the chip to its initial position.
● A Ludo board game layout with a single game piece (chip) on one of the
starting positions.
● When the Roll button is clicked, display a simple animation of the die
rolling, fetch a random number from an online service and show the final
number obtained from the die roll.
● When the chip is tapped, move the chip to the appropriate position based
on the last die roll.
● Integrate Unity's Addressables system into the scene. You can load the
images of the die and chip.


### Gameplay
[![Video Title](http://i3.ytimg.com/vi/_b4yToZ-Nqo/hqdefault.jpg)](https://youtu.be/_b4yToZ-Nqo)
---------------------------------------------------------------------------------------------------------------
### Autoplay for test and debug
[![Video Title](http://i3.ytimg.com/vi/-E1k4_JFnT8/hqdefault.jpg)](https://youtu.be/-E1k4_JFnT8)
---------------------------------------------------------------------------------------------------------------
### Unity Editor Tool For Create Board Paths
[![Video Title](http://i3.ytimg.com/vi/A8o_x_XcjaY/hqdefault.jpg)](https://youtu.be/A8o_x_XcjaY)
--------------------------------------------------------------------------------------------------------------
### Documentation: 

The Ludo game classes utilize two key design patterns: the Model-View-Controller (MVC) and the Observer pattern. These patterns are fundamental to structuring the game's architecture and ensuring efficient communication between different components of the game. The MVC and Observer patterns are integral to the design of the Ludo game classes. The MVC pattern helps separate concerns and makes the code more maintainable, while the Observer pattern allows efficient communication between different game components without them needing to be tightly coupled. These patterns are fundamental to creating a scalable and robust game architecture.

### GameController
The GameController class is a central component of the game's architecture, managing the game state and orchestrating the game flow. Its design and implementation are crucial to the game's performance and player experience. if we want turn the game into multiplayer we able to run this class in a Headless Server or .NET Core server.
#### Class Structure
The GameController class is a singleton, ensuring that only one instance of the class exists during runtime. This design pattern is commonly used for managing game states, as it provides global access to the game controller from any part of the codebase.
#### Fields
The GameController class contains several fields:
DiceManager: This field manages the dice rolling mechanism in the game.
Board: This field represents the game board.
Players: This field is an array of Player objects, representing the players in the game.
#### Methods
The GameController class has several methods that handle different aspects of the game:
StartGame(): This method initializes the game, setting up the board and players.
StartRound(): This method starts a new round, allowing players to roll the dice and make their moves.
DetermineGameWinner(): This method checks if a player has won the game.
#### Asynchronous Programming
use of the UniTask library for handling asynchronous tasks. This allows the game to perform long-running operations, such as waiting for a player's turn or a dice roll, without blocking the main thread and freezing the game.

### GameModel 
The GameModel class is a crucial part of the game's architecture. It serves as the primary data model for the game, storing the game's state and providing methods to manipulate this state.
#### Class Structure
The GameModel class is a central component of the game's architecture, managing the game's state and providing methods to manipulate this state. Its design and implementation are crucial to the game's functionality and player experience. By adjusting the PlayerNumber, NumberOfChips, and PlayerBotSetter fields, you can customize the game's setup to suit your needs.
#### Fields
The GameModel class is a central component of the game's architecture, managing the game's state and providing methods to manipulate this state. Its design and implementation are crucial to the game's functionality and player experience. By adjusting the PlayerNumber, NumberOfChips, and PlayerBotSetter fields, you can customize the game's setup to suit your needs.
PlayerNumber: This field represents the number of players in the game. It can be changed to adjust the number of players.
NumberOfChips: This field represents the number of chips each player has. It can be adjusted to change the number of chips each player starts with.
PlayerBotSetter: This is a list of boolean values that determine whether each player is an AI or a human player. By changing the values in this list, you can control which players are AI.
#### Methods
SetPlayerNumber(int number): This method sets the number of players in the game.
SetNumberOfChips(int number): This method sets the number of chips each player has.
SetPlayerBotSetter(List<bool> botSetter): This method sets which players are AI. The botSetter parameter is a list of boolean values, where true indicates an AI player and false indicates a human player.

### GameViewManager
The GameViewManager class is a key component of the game's architecture. It serves as the central hub for managing the visual representation of the game's state and orchestrating interactions between different visual elements.
#### Fields
BoardView m_boardView: This field represents the visual representation of the game board.
HighlightManager m_highlightManager: This field manages the highlighting of game elements.
List<PlayerView> m_playerViews: This field is a list of PlayerView objects, representing the visual representation of the players in the game.
List<PlayerPositionWithColor> playersPositionParent: This field is a list of PlayerPositionWithColor objects, representing the positions and colors of the players.
PlayerView m_currentPlayerView: This field represents the current player in the game.
#### Methods
GetPlayerByIndex(int index): This method returns the PlayerView object at the specified index in the m_playerViews list.
OnStartGame(OnStartGameEvent onStartGame): This method initializes the game view, setting up the board view and player views.
OnSetPlayerTurn(OnSetPlayerTurnEvent onSetPlayerTurnEvent): This method handles the event of setting a player's turn.
#### Event Subscription
The GameViewManager class subscribes to several events using the EventManager class. These events include OnStartGameEvent, OnSetPlayerTurnEvent, OnPlayerSetPlayableChipsEvent, and OnChipTapEvent. When these events are triggered, the corresponding methods in the GameViewManager class are called to update the game view.

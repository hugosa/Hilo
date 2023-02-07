# Hi-LO from Gaming1 by Hugo Sá

The game implementation presented in this challenge takes a KISS approach, with a simplied attempt at production level code.

The solution has a structured approach following the Clean Architecture design principles using CQS.

Though the requirements are clear for a one player game, the multiplayer concept is open for interpretation, so this implementation was made under a certain group of assumptions.

Though the goal is to be close to production level code, this is merely and exercise and it's definetly not production read.

#### Assumptions

    1. Multiplayer may need to scale beyond local play, over the internet.
    2. Players may want to join an open game with their friends, over the internet.
    3. As the game gets more traction, the structure of the application should be ready to handle higher loads and scaling accordingly (e.g., horizontal).

## Structure

The UI in this implementation is a console application.

The communication between the presentation and the application layers is abstracted by the CQS pattern, enabling other types of presentation layer implementation to be easily plugged in (e.g., a web application).

Not all actions implement in the at the application layer level are used, as is an example the
```
    GetOpenGamesQuery
```
which is available in the application layer under the assumption 2, no enable future use cases.

The domain object representing the game is stateless, under the assumptions 1, 2 and 3. The game context is expected to be managed by the presentation layers, using GameId and PlayerId for context representation in each request.

For the sake of simplicity, as the UI is a local console application, the persistence, abstrated using the Repository pattern, is implement in memory. A different implementation of the IRepository should be enough to move the game to the web.

Also, for the sake of simplicity, only a couple of unit tests were added to ensure that the game behaves as expected. The tests should cover the majority of the criticals paths in the Presentation and Domain layers, but they do not cover the solution exhaustively.

Finally, project Hilo.DemoPlay displays and algorithm to beat the game. The algorithm uses the application layer directly, also displaying that any other type of presentation can easily be plugged in.
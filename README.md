# match3-test

![Gameplay footage](https://user-images.githubusercontent.com/1725075/89704704-b6fc5500-d92c-11ea-8c02-6aee59d17d8a.gif)

This is a complete game for an admission test. Feel free to use and support.

**Game concept**

There are gems of different colors on a grid. Players have to move these gems up down left right to form a row or column of same colored 3 or more gems. When the player match gems of same kind, they gets destroyed and player get points for it. 
Everytime the player scores, the gems need to be moved down and freeing up place for new gems that will be automaticly generated on top.

**Rules**

- Game must be coded in C# using Unity 2019.2 or later (Official Releases: Do not use Alpha/Beta builds!)
- After every change in the board the game must evaluate if there is possible moves and shuffle gems if needed.
- Every round must last 2 minutes and have a points goal that will bee increased after the conclusion of each round.
- Game must have sound and make use of the sprites of this repository. 
- The game must be implemented in Portrait orientation, and the game board and UI must works in differenct resolutions and aspect ratios.
- Delivery: the project must be uploaded into a github repository

**Run**

- Install Unity version 2019.4 official release

**Features**

- Scriptable Objects based architecture
- Simple physics mechanics
- TextMesh fonts assets
- Some tooling scripts for better development workflow
- Android build support (tested on a Android 10)
# match3-test

![Gameplay footage](https://user-images.githubusercontent.com/1725075/89704704-b6fc5500-d92c-11ea-8c02-6aee59d17d8a.gif)

This is a complete game for an admission test. You can play the game by building it youself in Unity.

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

- Install Unity version 2019.4.3f1 official release
- Import and run project on Unity

**Features**

- Scriptable Objects event-based architecture
- Simple physics mechanics
- TextMesh fonts assets
- Some tooling scripts for better development workflow
- Android build support (tested on a Android 10)

***A note regards ScriptableObject based architecture***

When using Singletons as global references to objects (like a Game Manager or an Audio Manager), our prefabs can't directly reference it in the inspector. Also, we can easily run into the problem of Singletons initializing each other, turning it into a inter-dependant mess.

To avoid these problems, we can use ScriptableObjects for the "global access" and keep the code modular. It can be also used for a lot of other very useful purposes. [Check out this talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) in Unite Austin 2017 conference, which explains it in great details.
# Minesweeper Game
The well-known minesweeper game, implemented in C# and using Windows Forms, in Visual Studio.

## Features
The game has three different levels: `Easy`, `Intermediate` and `Hard` in which the number of squares and the number of mines vary.
It also carries a study of the statistics of the previous games, whose data are stored in a file (`statistics.txt`).

## How to launch
1. Clone this repository
```bash
$> git clone https://github.com/davidmohedanovazquez/Minesweeper.git
```
2. Open the project in Visual Studio (clicking directly on `MinesweeperGame.sln`).

> **_NOTE:_** Before executing the program it is important to change the path of the `statistics.txt` file to the path of the computer from which it is executed, both in line 12 of the `Minesweeper.cs` file and in line 14 of the `Statistics.cs` file.

Run and enjoy the game.

## How to play
The game controls are exactly the same as in the original game:
- Left click on a square to open it and discover its contents.
- :bomb: Right click on a square to mark it with a flag as Bomb.
- :grey_question: Second right click on a box to mark it as doubt with a question mark.
- Third right click on a box to return to its initial state.

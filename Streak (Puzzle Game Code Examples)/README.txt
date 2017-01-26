For simpler readability within Sublime or Notepad++, treat the script as if C#.

Brief - The entire main game logic. Creates block patterns, manage/manipulate them, score accordingly.

Table of Contents:
 - LoadInitialization (Line 99) - Process that is called from a different script that initializes the game based on different difficulties
 - Create2x2 (Line 174) - Populates an array with the 2x2 blocks for the game, passing future block info to the HUD
 - DetermineScoreLine (Line 350) - Sets a seed for the ScoreLine iterative pathfinding
 - ScoreLine (Line 403) - Contains the entire iterative pathfinding process, determining if a path exists
# cisc-496-game-project-p3-game-buds
cisc-496-game-project-p3-game-buds created by GitHub Classroom
----------------------Known Issues---------------------
------------------------Overview-----------------------
Run Man
Group P3: Game Buds (Aaron Safer-Rosenthal, Alice Kim, Bryan Chafee, Sarah Pak)
Introduction
An amorphous blob-man runs endlessly towards the horizon. No one knows why. All we do know is there are those that seek to stop him. This ‘Run Man’ can scale his size and weight up and down to change his physical properties. The larger he is the harder it is to stop him, but the slower he moves. Getting smaller does the opposite. Varied in size and weight, other amorphous blob-creatures chase him in an attempt to bring an end to his running. They can stick to Run Man’s body adding their weight and friction slowing him down. Unfortunately for their target they can also identify objects and traps they can interact with or stear the Run Man into.
Game Setting
The game takes place in a level-based format, with each level being a flat track, possibly featuring hills/ramps, that the run man runs across, towards the horizon. The player views the level from a third-person perspective, above the run man and focused on the middle (horizontally) of the platform. We intend for the player to be able to zoom their view in and out and possibly tie this to the size scaling mechanic. The platform will not take up the entire screen and on the left and right side there will be empty space. In addition, each level will feature various traps scattered throughout and visible to the player. Similarly, the blob-creatures will be dispersed throughout the level, but with the ability to move throughout it to try and harass the player.


Game Style
Simplistic and colourful. The player is a rounded blob person with two featureless legs, and is bubblegum pink. Every creature is styled as featureless blobs that slide on the ground, randomized bright colours from high saturation colour choices. The environment will be mostly featureless aside from traps and interactables.

Colour Palettes
Main Character:
	[] - A3278F, or player selected


AI Blobs:
	[] - E77237, or random
	
Ground Colours:
	[] - 69CA30
[] - some brown


Core Game Mechanics
Player
Automatically runs forward until the end of the level is reached
The player has 4 inputs. 
A and D to move left and right respectively
Scroll wheel up a down to scale the size/mass of the player object to a minimum or maximum
When the player’s size/mass is lower, they move faster (vice-versa for bigger size/mass)
Loses when velocity reaches 0
Agent
Object identification
Relates objects to actions
Attach to the player and add physical properties to the player
Wins when the player velocity reaches 0
Desired emergent behaviours; Herding of player, co-ordinated entrapment of player
Environment
Destructible walls
Moveable objects
End of level trigger
Traps
Slow trap
Pressure plate bombs (certain mass will trigger)
Pitfalls
Speed booster
Novel Game Design 
The interactions between the game and the agents will be the novel design aspect. The agents will learn how to interact with the environment, player, and objects in the environment to win the game by reducing the player’s velocity to 0. Our goal is to train the agent to behave dynamically depending on how many other agents it can detect, what objects it and the player can interact with, and what condition the player is currently in. The player will be experiencing a pre-emptively acting foe with more complicated behaviour and tactical patterns than a behaviour tree.
Intended Platform and Development Environment
This game is a single-player game, played on a PC/Mac computer using the keyboard. The player can control the character to move left or right using the “a” and “d” characters and “w” and “s” to scale up and down. The player can also use the mouse to control the character by moving the mouse left and right, as well as using the scroll wheel to scale up and down.

For the development we will be using Unity with the WebGL build option, so that we can build for web deployment.
Interesting Technological Approach
This game will implement ML Agent to train adversarial agents that will seek to use their environment and physical bodies to stop the player from reaching the end of a level. When the player’s velocity reaches 0 they lose. The player must balance swapping between their maximum and minimum scale to outrun and resist the physical interactions made between the AIs and the player. 


# CIS497-Procedural-Platformer
DMD Senior Design Project

Evelyn Bailey

Beta Progress:

-Implemented a user interface so the player can choose which difficulty and constraints they want to use. These parameters adjust the procedural generation of the level and the level completion rules.

-Implemented difficulty adjustment. Easy corresponds to shorter levels, less spawning of obstacles, and more regular placement of obstacles. Medium corresponds to longer levels, higher frequency of obstacles, and the introduction of rhythms with obstacles randomly placed. Hard corresponds to the longest levels with high randomness.

-Added a slope sprite to introduce a more organic level layout. Implemented physics for sloped sections.

-Implemented no damage constraint. The player essentially only has one life throughout the level.

-Implemented time constraint. The player must beat the level within the given time. The max time is proportional to the length of the level.

-Implemented don't look left constraint. The player must beat the level without ever moving backwards. The level generation ensures the player does not have to backtrack to complete the level.

-Implemented defeat enemies constraint. The player must defeat all enemies before completing the level. If there are still enemies present, the player can not beat the level even if they reach the goal sprite. The level generation increases the frequency that enemies are chosen and ensures that at least one enemy is present when this constraint is active.

-Implemented all spikes constraint. The ground sprites are replaced with spikes (except for rest areas). The level generation spawns enemies and restricts their movement patterns so that they are non-overlapping and allow the player to hop from one enemy to another to avoid spikes and jump over chasms.

-Implemented no jump constraint. The level generation disables free roaming enemies since players can not jump to defend themselves and increases the frequency of stompers and moving platforms instead. It also ensures that if a post-cliff ledge is higher than the pre-cliff edge, an enemy will spawn at a lower elevation to allow the player to hop off the enemy to reach the other side. Gaps where the pre- and post-cliff sections are the same height are avoided by reducing the elevation of the post-cliff area. Still needs to adjust spike spawning when all spikes constraint is active so that players do not have to jump to avoid spikes that are not preceded by a cliff.

-Started stars constraint. The level branches off into three different sections. Must define behavior for reaching different areas and for how to place stars throughout the level.


Alpha Progress:

-Built a rhythm generator that creates 6 rhythm blocks by randomly generating distribution types, density, and length. These rhythm blocks are represented as arrays of 1s and 0s, where 1s correspond to actions the player must take.

-Generated action arrays that correspond to the 1s in rhythm blocks. These store two values that represent the type of obstacle and its duration.

-Translated rhythm and action arrays into 2d sprites in the scene. 1s in the rhythm block are represented as enemies, moving platforms, or spikes. 0s are represented as the ground, which changes elevation based on a built-in state machine that determines the probability of switching from one state to another.

-Implemented physics engine and collision detection system

-Implemented basic enemy behavior (move back and forth along a flat, level stretch of ground). Restricted placement of enemies so that they would not appear at the start of the level or on stretches level ground that were too small.

-Implemented movement behavior of the moving platforms and stompers.

-Implemented lives system, respawning after falling off a cliff, and restarting the game after the player reaches the end of the level or if they lose all their lives

-Adjusted rhythm generation parameters to ensure actions weren't too close together so that they player could not navigate the level. Placed rest areas of flat ground between rhythm blocks.
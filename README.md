Defend
==============

2D Game made with Unity 5 using C#, done in my spare time after work.

- Use the mouse to aim and fire to defend your base
- Don't forget to watch your ammo
- Don't get hit by the meteors or it's game over
 
### Play here: https://github.com/Teh-Lemon/Defend-Game/releases/tag/1.0

![alt text](Screenshot.png "")
 
Progress
------------
All the core features are complete so far except for music.  
Done:
- Spawning meteors, size and spawn positions are random.
- Meteor waves. The chance of larger waves increases as the game progresses.
- UI - Menu buttons, in-game UI, custom mouse cursor.
- Player turret with a working animated shield and limited fire-able ammo. Camera shakes when hit.
- Game states: Can start, end with score and restart the game.
- Options and pause menu
- AI friendly turrets
  - Turrets can pick and fire at meteors
  - Turrets can die, dying animation
- Sound effects
- Power ups
  - Shield
    - Activate fade in animation
  - Turret
    - Can spawn the AI turrets with spawning animation
  - Big Bullet
    - Visual effect to show it's on, flashing to show it's almost over
	- Camera shake to show recoil
  - Ammo refill
    - Particle effect to draw attention to ammo bar
- Android port

Possible future updates:
- Different meteor types.
- Environmental changes

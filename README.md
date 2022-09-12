
# Star Wars: The Bounty Hunter

![Image 1)](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_1.png)

**This repository does not contain all of the files and directories 
of the whole project.
Only source code game scripts and playable executables for Mac and Windows
operating systems are included (note Windows 11 may not fully work yet).**

## Introduction

2D shooter pixel platformer game made in Unity.

Game's short fictional story is based on life of Boba Fett character inspired by Star Wars Omnibus: Boba Fett by John Wagner.

The goal was to create game in Unity Engine as my school project.
The first version of the game was created when I was studying Informatics at Middle School of Information Technology in Kysucké Nové mesto (SPS IT KNM).
I participated in Stredoškolská odborná činnosť (SOČ), category Informatics.
That year I placed third in the school round (see [SOC SPS IT KNM 2020](http://www.spsknm.sk/ssknm/sk/node/923)).

I went through all the phases of game development 
(level design, UI design, game mechanics, programming game logic (C#), 
graphics and animations (Photoshop), music and sounds (Audacity), 
testing, debugging, and more...)

Afterwards I decided to completely remake the game from the ground up.
I reworked all of the game scripts, I made the code more cleaner, readable, effective 
and I followed common C# conventions. I also reworked and added more 
UI, graphics, sounds, animations, ...

I also added more features such as SQL lite database for storing player data, 
more menus such as admin menu to manage player data, player profiles, login screen 
and other things...

So far I was able to rework and make playable only the first level.
Two more levels are planned. Also character voice lines, parallax background, 
advanced enemy logic using A* pathfinding algorithm and more things are on TO-DO list.

---

**This game is side project made solely for demonstration, education and self-development purposes. \
It is not intended to generate any monetary profit.**

---

## Screenshots form the game

![Image 2](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_2.png)\
![Image 3](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_3.png)\
![Image 4](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_4.png)\
![Image 5](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_5.png)\
![Image 6](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_6.png)\
![Image 7](https://raw.githubusercontent.com/oliver-svrcek/Star_Wars_The_Bounty_Hunter_/main/Screenshots_and_videos/Screenshot_7.png)

## Controls

- **Left arrow** - move left
- **Right arrow** - move right
- **Up arrow** - jump
- **Spacebar** - use jetpack
- **V key** - use flamethrower
- **C key** - use blaster
- **ESC key** - pause

## Game mechanics

- **Top bar** - Health
- **Bottom bar** - Jetpack fuel
- **Left bar** - Blaster heat
- **Right bar** - Flamethrower fuel

Use jetpack to fly thorugh the air, reach platforms and evade enemies.
Jetpack depletes it's fuel when used. It starts to recharge automatically after a short delay.

Use blaster to eliminate your enemies.
Blaster shoots burst of three bullets. It heats up when being fired. 
To use it the most effectively wait a short moment after you fire each burst to let it cool down, 
otherwise the blaster might overheat and you will not be able to use it until it cools down completely.

Use flamethrower in sticky situations or when you want to deal high damage to your enemies.
Flamethrower can be used only when you are on the ground and you will not be able to move until the whole salvo is burned out.
It also blocks incoming bullets from enemies but beware because it's recharge time is quite long.

### Upgrade menu

To access upgrade menu pause the game and click on "Upgrades" button.\
There you can upgrade characters abilities and gear (Armor, Blaster, Jetpack, Flamethrower).

You can upgrade various stats such as recharge time, usage time, damage, depletion reduction, character's health points and more... 

### Admin menu

Admin menu can be used to manage players.\
You are able to add players, delete players or view player data.

## Installation

To play the game go to releases and download ZIP archive for your operating system.\
No installation is reqired, launch the game directly from executable file.

## Packages and plugins

[sqlite-unity-plugin](https://github.com/rizasif/sqlite-unity-plugin)\
[A* Pathfinding Project for Unity](https://arongranberg.com/astar/)

Manaspace font\
Star Jedi Rounded font

# Author
Created by  Oliver Svrček

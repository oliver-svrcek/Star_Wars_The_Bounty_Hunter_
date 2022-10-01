
# Star Wars: The Bounty Hunter

![Image_1](https://user-images.githubusercontent.com/75705745/193413709-b942dc3d-a322-4132-9ac9-cd86b84efc06.png)

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

![Screenshot_2](https://user-images.githubusercontent.com/75705745/193413736-44fa3d22-1679-4a43-9e63-541c971355b4.png)
![Screenshot_3](https://user-images.githubusercontent.com/75705745/193413740-20a982e6-4cb1-41e0-a505-0b08da82a17a.png)
![Screenshot_4](https://user-images.githubusercontent.com/75705745/193413742-39d52910-f7aa-4ecf-841d-fa6ab9a93e4a.png)
![Screenshot_5](https://user-images.githubusercontent.com/75705745/193413744-c5ee760b-a14b-4877-a9cf-27b17dff2294.png)
![Screenshot_6](https://user-images.githubusercontent.com/75705745/193413746-815cc22e-b00f-4f05-9973-5e468b595511.png)
![Screenshot_7](https://user-images.githubusercontent.com/75705745/193413748-4981991a-3208-4cf4-a700-53310f53cdcf.png)

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
No installation is reqired, unzip the archive and launch the game directly from executable file.

## Packages and plugins

[sqlite-unity-plugin](https://github.com/rizasif/sqlite-unity-plugin)\
[A* Pathfinding Project for Unity](https://arongranberg.com/astar/)

Manaspace font\
Star Jedi Rounded font

# Author
Created by  Oliver Svrček

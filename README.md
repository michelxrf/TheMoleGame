# Toco, The Miner

## About

This game was developed as the final project for the CS50 Introduction to Game Developmet course.

My name is Michel Redmer da Fonseca. This game was fully made by me, except for most of the sound, winch was taken from freesound.org.

The project is available for free at https://michelxrf.itch.io/toco-the-miner

## Intro

You play Toco, a mole. He is a Miner, or rather a mining engineer. He's greedy and is willing to go to dig treasures in caves infested with giant spiders. Will he make it back alive? Will he turn a profit? Or just become spider food?

## How to play

WASD keys to move around
MOUSE to look around
LMB to attack
E to interact with boxes

## Features

### Proc gen levels

I think the main feature of my game is the level generation, I've put quite a lot of effort into it. Since I've began this project I was sure I wanted to make a dungeon crawler with proc gen levels, so this guided all else in the game. At each level start a new layout is generated based on it's level number, and based on this level it'll generate larger levels with different treasure chances, different monsters spawner chances, higher monster populations, higher monster spawn rates, higher monster levels and so on.

The main reference I had for this feature was the Dreadhalls project. I've started with the idea of a big square filled with blocks, breakable ones. Then it carves rooms, similar to Dreadhalls but instead of corridors it starts with a point, winch will be the center and carves a rectangle around it and them places a monster spawner at the center. This is all made at an array and not actual carving is done. Only after the map's array is generated the script proceeds to instance the blocks.

### Coin collection

I wanted the player to be able to collect some for of reward, coins was the obvious choice. My first idea was to make just different values for each type, just like Zelda games. But ended up creating 3 independent currencies, I wanted to give more meaning to the treasures, so its higher rarity meant to be more interesting.

### Shop

It felt useless to have a reward that wasn't useful for nothing. So I've added the items shop at the beggining. The shop actually took more effort than it was worth it, I had to add all the player stats, keep track of them throughout the game and reset them at the end of each run. Without any powerups to buy, all that wouldn't be needed.

### Spiders

At first I wanted to add many different types of enemies. I've started with something that I thought would be easy to model and animate in Blender, but it was a lot harder than what I expected so I decided to cut off all other monsters and just make some variations to the spider's model. This allowed me to reutilze all the animations at least.

Their AI was one my favorite parts of this project, I really liked working with creature's beahviour. In the end I've made a spider that is too smart for the game, it has four modes, and the player will only ever see 2 or 3. It spawns in a idle mode, just walking aimlessly through the map. Then it uses raycasting to detect the player, once it detects the player it'll enter a stalk mode, following the player around but keeping a safe distance. Once in a while it'll enter the hunt mode and move straight at the player for a hit. If it gets hit or hits the player it'll go back to stalking, unless it is at a low health state. In that case it'll turn around and run for it's life.

Also, if it loses sight of the player for a long enough time it'll forget about it and just go back to idling. If it loses visual sight of the player a timer will start to make it go idle and it'll run to the player last known location. This was very intersting to see before I turned off the lights. Maybe I should make a random chance for the levels to be dark or illuminated.

### Save system

Another thing I'm happy with is the save system. All the main game data is stored at a static class called GameData so its very straight forward to make changes all across the game easily. And those changes can be saved and loaded from a file very easily too. At first I've made this very simple system that would simply save all the GameData class to a binary file when a SaveGame() method was called and the reverse was done when a LoadGame() was called. This way the game can be saved automatically at certain moments. It's an autosave.

Later I had to upgrade this system to check for failures while saving or loading the file. This was necessary because when made changes to the GameData class, the save file would break and had to be manually deleted. I ended up solving it with some try-catch logic so it would automatically delete a corrupted save file and create new blank one.

### Dark Ambience

At first this wasn't part of the plan but as the game took form, I started contemplating the idea of a dark eerie dungeon/mine. I've gave it a shot and once I've turned of the lights I liked the result, and so it stayed like that.

### Non dark levels

I had this idea will writing this readme, this was going to be in the "future features" but decided to add it already. This would allow the player to see the spiders idling around and stuff. This is just to shine a light at them and allow them to be appreciated. This could be a simple light above the level that will get darker as the level grows.

### Online leaderboard

I've stumbled on a website a few days back, Lootlocker, they provide some tools and services for game devs and they say they have a tool to make easy online leaderboards for free. Of course the free version is limited, but it'll be enough for this game.
The scores are calculated like this: silver = 1 point; gold = 2 points; emerald = 3 points;
I was already with my finished version, ready to be submited when I decided to implement this. I was going to leave it for a future version, but decided to give it a try. It was super easy to set up. I'm very happy with the state of this game now.

## Main Design Challenges

### Coding was alright

The game design and coding was actually not a problem, as I was figuring out how to do each of the things I wanted to do it was quite straight forward. That doesn't mean it was not filled with mistakes, I had to do a lot of bug hunting. I implemented one thing at the time and was playtesting a lot. So I never got overwhelmed with bugs, they only came a few at a time.

With the expection of the level generator. As this code got more and more complex I had rethink how things were done:
- First it only generated a rectangular shaped map.
- Later it filled it breakable blocks. 
- Then I needed to position the player starting point and map exit, and they couldn't be in the same position, I had to first place the exit randomly and them calculate a valid point to put the player, this point had to be at least at a certain distance from the exit. But to place the player I had to generate rooms.
- And then I had to add the treasure blocks at random locations, also respecting the rooms, the exit and the starting points. Them I had the spawners that couldn't spawn a spider so close to the player starting position that it would kill it too fast. And to finish I had to also calculate the monsters population, type distribution and resourse rarity, winch was actually the easiest part.

### Art

I've made every single graphical asset in the game, and that was real challenge and time sink. I barely knew anything about 3d modeling. At the beggining when all I had was cubes and a cilinder a gray box map I thought 3d modeling was easy. But when I started modeling my first monster, the spider, I had a reallity shock! Animating was a pain. It took me a while to make a decent spider animation, and then I couldn't find a way to import it to Unity.

It took me weeks just to figure out how to animate something and import it into Unity correctly.

### UI

Unity's has a very good Navmesh system, but it doesn't have much information about it. At first it seemed like a very straight forward thing, I could just ignore most details and it worked out of the box. That until it started to not work, then I had to start sacvenging information in forums, and the information wasn't easy to find. Many of the details like base offset of the model and making agents avoid other agents was mostly a trial and error process.
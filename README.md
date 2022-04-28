# Toco, The Miner

## About

This game was developed in Unity as the final project for the CS50 Introduction to Game Developmet course.

My name is Michel Redmer da Fonseca. This game was fully made by me, except for: 
- for most of the sound, winch was taken from freesound.org and it's CC0 license;
- and Lootlocker's leaderboard, I've used their system and most of what's implemented is just their tutorial applied to my use case;

This project is available for free at https://michelxrf.itch.io/toco-the-miner

## Intro

You play Toco, a mole. He is a Miner, or rather a mining engineer. He's greedy and is willing to go to dig treasures in caves infested with giant spiders. Will he make it back alive? Will he turn a profit? Or just become spider food?

## How to play

WASD keys to move around
MOUSE to look around
LMB to attack
E to interact with boxes

## Features

### Proc gen levels

I think the main feature of my game is the level generation, I've put quite a lot of effort into it. Since I've began this project I was sure I wanted to make a dungeon crawler with proc gen levels, so this guided all else in the game. At each level start a new layout is generated based on it's level number. Based on the level it'll generate larger levels with different treasure chances, different monsters spawner chances, higher monster populations, higher monster spawn rates, harder monsters and a darker ambient.

The main reference I had for this feature was the Dreadhalls project. The map starts with a big square filled with blocks, breakable ones. Then it carves rooms, similar to Dreadhalls but instead of corridors it starts with a point, winch will be the center and carves a rectangle of random size around and them places a monster spawner at the center. This is all made at an array first and not actual carving is done. Only after the map's array is generated the script proceeds to instance the blocks. Then it picks a spot at random to place the map's exit, and based on the exit it chooses another point to place the player, it carves this location to ensure the player won't be dropped inside a wall block.

Also when when the map is getting filled with the blocks in the beginning of the map generation it'll have a random chance to actually be a treasure block instead of dirt block.

### Coin collection

I wanted the player to be able to collect some sort of reward, coins was the obvious choice. My first idea was to make just different values for each type, just like Zelda games used the Ruppees. But ended up creating 3 independent currencies, I wanted to give more meaning to the treasures, so its higher rarity meant to be more interesting choices.

### Shop

It felt useless to have a reward that wasn't useful for nothing. So I've added the items shop at the beginning. The shop actually took more effort than it was worth it, I had to add all the player stats, keep track of them throughout the game and reset them at the end of each run. Without any powerups to buy, all that wouldn't be needed.

### Spiders

At first I wanted to add many different types of enemies. I've started with something that I thought would be easy to model and animate in Blender, but it was a lot harder than what I expected so I decided to cut off all other monsters and just make some variations to the spider's model. This allowed me to reutilize all the animations at least.

Their AI was one my favorite parts of this project, I really liked working with creature's behavior. In the end I've made a spider that is too smart for the game, it has four modes, and the player would only ever see 2 or 3 because of the darkness of the map, so I've decided to reduce the darkness, more on that in its own topic. 

The spiders spawn in a idle mode, just walking aimlessly through the map. Then it uses raycasting to detect the player, if it detects the player it'll enter a stalk mode, following the player around but keeping a safe distance. Once in a while it'll enter the hunt mode and move straight at the player for a hit, using an IEnumerator with a timer checking the spider's aggression stat against a random value to check when the spider should attempt an attack. If it gets hit or hits the player it'll go back to stalking, unless it is at a low health state. In that case it'll turn around and run for it's life no matter what, and will only attack if the player gets too close.

Also, if it loses sight of the player for a long enough time it'll forget about it and just go back to idling. If it loses visual sight of the player a timer will start to make it go idle and it'll run to the player last known location. This was very interesting to see before I turned off the lights, winch led me to make the game a little bit more illuminated at the initial levels.

### Dark Ambience

At first this wasn't part of the plan but as the game took form, I started contemplating the idea of a dark eerie dungeon/mine. I've gave it a shot and once I've turned of the lights I liked the result, and so it stayed like that.

At first it was just pitch black in every map, but it limited the player's capacity to see the spider's behavior, and I've so much effort into those spiders I couldn't let that go to waste.

Even though I did that mostly out of vanity I think it actually added to the game by letting the player avoid the spiders on the initial levels, decreasing the difficulty a bit.

### Save system

Another thing I'm happy with is the save system. All the main game data is stored at a static class called GameData so its very straight forward to make changes all across the game easily. And those changes can be saved and loaded from a file very easily too. At first I've made this very simple system that would simply save all the GameData class to a binary file when a SaveGame() method was called and the reverse was done when a LoadGame() was called. This way the game can be saved automatically at certain moments. It's an autosave system.

Later I had to upgrade this system to check for failures while saving or loading the file. This was necessary because when made changes to the GameData class, the save file would break and had to be manually deleted. I ended up solving it with some try-catch logic so it would automatically delete a corrupted save file and create new blank one.

### Online leaderboard

I've stumbled on a website a few days back, Lootlocker, they provide some tools and services for game devs and they say they have a tool to make easy online leaderboards for free. Of course the free version is limited, but it'll be enough for this game. I loved the idea and added to the game.

I was looking for a way to give the player a purpose. I've sent the game to a few friends for testing and the most common issue was them asking "what's the objective? Is there a boss or final objective?". I was considering adding a boss fight at a level and add a huge payoff, but it felt artificial. Luckily I've stumbled on Lootlocker while browsing some game jams at Itch.io. It was perfect for the game: I would need to put a ton of effort into designing a boss fight. While the competitiveness matched well with the game style. 

## Main Design Challenges

### Coding was alright

The game design and coding was actually not a problem, as I was figuring out how to do each of the things I wanted to do it was quite straight forward. That doesn't mean it was not filled with mistakes, I had to do a lot of bug hunting. I implemented one thing at the time and was playtesting a lot. So I never got overwhelmed with bugs, they only came a few at a time.

With the exception of the level generator. As this code got more and more complex I had rethink how things were done:
- First it only generated a rectangular shaped map.
- Later it filled it breakable blocks. 
- Then I needed to position the player starting point and map exit, and they couldn't be in the same position, I had to first place the exit randomly and them calculate a valid point to put the player, this point had to be at least at a certain distance from the exit. But to place the player I had to generate rooms.
- And then I had to add the treasure blocks at random locations, also respecting the rooms, the exit and the starting points. Them I had the spawners that couldn't spawn a spider so close to the player starting position that it would kill it too fast. And to finish I had to also calculate the monsters population, type distribution and resource rarity, winch was actually the easiest part.

### Art

I've made every single graphical asset in the game, and that was real challenge and time sink. I barely knew anything about 3d modeling. At the beginning when all I had was cubes and a cylinder a gray box map I thought 3d modeling was easy. But when I started modeling my first monster, the spider, I had a reality shock! Animating was a pain. It took me a while to make a decent spider animation, and then I couldn't find a way to import it to Unity.

It took me weeks just to figure out how to animate something and import it into Unity correctly.

And that led me to cut off all other monsters I had planned for the game. At first there was 4 monsters and 2 NPCs. The NPCs were meant to be a trader and a hauler. The trader is now just a shop screen and the hauler is now the storage boxes.

### Nav Mesh

Unity's has a very good Navmesh system, but it doesn't have much information about it. At first it seemed like a very straight forward thing, I could just ignore most details and it worked out of the box. That until it started to not work, then I had to start scavenging information in forums, and the information wasn't easy to find. 

Sometimes the spiders would walk on top of each other, or ignore one another, or simply not work at all. Many of the details like base offset of the model and making agents avoid other agents was mostly a trial and error process. I had to play around with it until I figured it out.

### UI

Making the menus and HUD work well on different screen sizes was a real challenge, every time I tested in a different size something would look off: a panel that would overstretch, a text that would be partly off screen. So much so that I don't think I've solved all the issues with the UI. This is a pain because when someone sees a menu broken it'll make the whole game look broken.

Godot has much better UI design tools, things like self adjusting menu slots and texts limited by margins seemed so basic, it's hard to believe Unity doesn't have this. I probably just missed though.

## Wrapping up

This course was awesome. Thank you Colton Ogden and all the CS50 crew for this, you guys rock! It was a great experience to learn Unity and all those game dev concepts.
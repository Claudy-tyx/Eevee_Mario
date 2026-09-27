# Eevee's Lost Ribbon

Eevee has lost her ribbon somewhere in the forest. The player explores three connected maps, defeats Pokémon enemies, collects upgrades and Gold Nuggets, improves Eevee's abilities, and eventually searches the dark forest for the missing ribbon.

## Controls

| Key | Action |
|---|---|
| A / D or Arrow Keys | Move left / right |
| Space | Jump |
| Left Shift | Dash |
| J | Tackle |
| K | Swift |
| Mouse | Interact with menus and upgrade choices |

## Gameplay

The game consists of three connected forest maps.

**Map 1** introduces the player to the basic forest, enemies and mechanics. After defeating **23 enemies**, the portal to Map 2 is unlocked.

**Map 2** contains a different forest layout and continues the player's progression. After defeating **43 enemies in total**, Map 3 is unlocked.

**Map 3** is a dark magical forest. Visibility is reduced and Eevee and her Swift projectiles provide light. Eevee's lost ribbon can be found somewhere within this map.

The player's progress is maintained while travelling between maps, including health, maximum health, Gold Nuggets, kills, upgrades and unlocked areas.

## Combat

Eevee has two attacks:

- **Tackle (J):** A close-range attack which can damage enemies and break boxes.
- **Swift (K):** A ranged projectile attack that can be improved through upgrades.

Enemies include different Pokémon with different movement patterns, including ground and flying enemies.

Eevee can take damage from enemies. When all hearts are lost, Eevee faints and the Game Over screen is displayed.

## Upgrade System

Upgrade crystals periodically appear as enemies are defeated.

Touching a crystal pauses the game and presents two randomly selected upgrades. The player chooses one and confirms the selection.

Available upgrades include:

- Increased movement speed
- Improved healing
- Additional jumps
- Increased Swift damage
- Directional Swift aiming
- Homing Swift
- Increased Swift speed
- Increased Swift duration
- Reduced dash cooldown

Some upgrades contain multiple tiers, with later upgrades requiring their earlier versions.

## Health and Collectibles

Eevee starts with **3 hearts**.

**Healing Flowers** restore lost health. Their effectiveness can also be increased through upgrades.

**Breakable Boxes** require multiple Tackle hits to destroy and drop **Gold Nuggets**.

Gold Nuggets can be given to **Sylveon** in exchange for additional maximum hearts.

Enemies, flowers and breakable boxes respawn so that the player can continue exploring and progressing through the game.

---

# Lab Checkoffs

## Lab 1 – Game Over, Statistics and Restart

- Eevee plays the faint animation and sound effect.
- Player controls are disabled.
- Eevee's movement is stopped.
- Gameplay is frozen.
- A Game Over overlay fades onto the screen.
- The player's defeated enemy count is displayed.
- A Restart button is provided.
- Restarting reloads the level and restores Eevee's health.

---

## Lab 2 – Interactable Objects and Sound Effects

### Breakable Box
Eevee can use **Tackle** against boxes.
- Reacts when hit by Tackle.
- Requires 3 hits before breaking.
- Temporarily changes appearance when hit.
- Plays hit/break sound effects.
- Drops a Gold Nugget when destroyed.
- Respawns 30s after being destroyed.

### Gold Nugget
The Gold Nugget can be collected by Eevee.
- Spawned from destroyed boxes.
- Detected when Eevee touches it.
- Added to the player's persistent Gold Nugget count.
- Displayed through the HUD.

### Healing Flower
Healing Flowers are another interactable object.
- Detect Eevee entering the trigger.
- Restore lost health.
- Produce visual feedback.
- Healing amount can be modified by upgrades.
- Disappear after being collected.
- Respawns after use.

### Flower Trigger
Flower Patches will create petal bursts and sound when Eevee walks on them.
- Detect Eevee entering the trigger.
- Plays a petal burst under Eevee's feet.


---

## Lab 3 – Input, Events and Game State Management
The game uses Unity's Input System for player input, including:

- Keyboard movement
- Jumping
- Tackle
- Swift
- Dash
- Mouse input for menus and upgrade selection

A central `GameManager` maintains the state of the current run, including:
- Gold Nuggets
- Total enemy kills
- Per-map enemy kills
- Maximum/current health
- Map unlocks
- Crystals collected
- Player upgrades
- Run time
- Death count

Events are also used so that systems can react when important values change.
Examples include:
- Gold changes
- Kill count changes
- Maximum health changes
- Map 2 unlocking
- Map 3 unlocking

---

## Lab 4 – Game Polish, Multiple Scenes, Persistence and Powerups

The game contains visual and audio feedback for player actions, including:
- Walking, jumping, attacking, hurt and faint animations
- Landing particles
- Landing sound
- Jump sound
- Tackle sound
- Swift sound
- Hurt and faint sounds
- Enemy hurt/death feedback
- Breakable box feedback
- Healing effects
- Crystal effects
- Map-unlock dialogue
- Game Over fade
- Background music
- Map 3 lighting

### Multiple Scenes
The game contains multiple connected scenes:
- Start Screen
- Map 1
- Map 2
- Map 3

Portals allow Eevee to travel between the three gameplay maps.

### Persistent Data
Gameplay data persists while travelling between scenes.

This includes:
- Current health
- Maximum health
- Gold Nuggets
- Enemy kills
- Map unlocks
- Crystal progress
- Player upgrades
- Run statistics
For example, if Eevee enters a portal at 5/8 HP, Eevee remains at 5/8 HP in the next map.

### Interactable Powerups
Crystals act as the main powerup system.

When Eevee touches a crystal:
1. Gameplay pauses.
2. Two upgrade choices appear.
3. The player selects an upgrade.
4. The player confirms the choice.
5. The upgrade immediately modifies Eevee's abilities.
6. The obtained upgrade persists between maps.

### Polished Restart
When Eevee dies:

- Gameplay stops.
- The Game Over screen appears.
- The player can restart.
- Eevee's health is restored.
- The level is reloaded correctly.
- Existing run progression is retained.

---

## Lab 5 – ScriptableObject Architecture and Pluggable FSM

### Pending

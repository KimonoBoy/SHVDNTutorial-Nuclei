Sorry for the lack of updates over the past couple of days. Been busy, will get back to the mod within a day or two.

# SHVDNTutorial-Nuclei
A successor to **NucleiLite** - this will build upon everything we learned here [NucleiLite - Wiki](https://github.com/KimonoBoy/SHVDNTutorial-NucleiLite/wiki)  

The idea of this **mod** is to **create** a **well-build**, **well-documented** overall **mod menu**, to **help** people interested in **developing mods** for **Grand Theft Auto V**

See the [Wiki](https://github.com/KimonoBoy/SHVDN-Tutorial/wiki)

## Current Mod Progress
* Added HotKeys service that handles saving and loading the different hotkeys in the hotkeys.ini
* Hotkeys can be either a key, a control or both. Controls are nice, because they depend on the game's keybindings, this ensures that e.g. pressing the sprint key on any keyboard or controller will trigger the same feature.
* Hotkeys can also have modifiers such as Shift and Control. 
* Hotkeys can be changed directly through the .ini file, but will be updated to be changeable through the menu.

-- The hotkeys should be mapped, for now you kind of have to know which hotkeys belongs to what group, we want to avoid this.  

-- When the menu is updated to allow for hotkeys changes, each menu item have an associated hotkey list where the user can change the hotkey for that specific feature by pressing F1 when the item is selected. 

## NOTE
I've decided that the Wiki will be finished once the mod is in a more complete state. 

Sorry for any inconveniences.

## Planned Features
Below is a list of features currently planned, more will come as the mods develops.

Features marked with an Asterix * are features that are functional but not yet complete.
* **Player**
- [x] Skin Changer Menu* - Change your Character's Model, Add your favorites to your favorites menu, change appearance and save the models.
- [ ] Stats Menu
- [x] Fix Player - Restores Health and Armor
- [x] Invincible - Makes the player invincible
- [x] Adjust Wanted Level - Change your wanted level
- [x] Lock Wanted Level - When enabled, the wanted level set will stay at that level, leave it at 0 to be never wanted.
- [x] Add Cash - $10.000 to $1.000.000
- [x] Set Cash by Input
- [x] Super Jump - Jump as high as a building
- [x] Infinite Stamina - sprint or swim forever
- [x] Infinite Breath - Bro, that's amazing lung capacity
- [x] Infinite Special Ability - Never run out of special ability power
- [x] Super Speed* - Switch between different super speeds with different super powers
- [x] Invisible - Well... No one can see you, not even you.
- [x] One Punch Man - Hit your foes with immense force, killing them instantly and sending them flying.
* **Vehicle**
- [x] Vehicle Spawner Menu  
	- [x] Spawn Any Vehicle in Game
	- [x] Add your favorite vehicles to a favorites menu.
	- [x] Save your vehicles* with all of their modifications.
- [x] Vehicle Weapons Menu* - Choose between a bunch of different Weapons, from shotguns, to lasers, to rockets and adjust the firerate and attachmentpoints
- [x] Vehicle Mods Menu* - Change all your vehicles modifications
- [ ] Vehicle Handling Menu
- [x] Repair Vehicle - Repair everything on your vehicle.
- [x] Indestructible - Your vehicle cannot be damaged.
- [x] Flip Vehicle - That annoying situation where your vehicle is upside-down
- [x] Speed Boost* - Apply a speed boost to your vehicle
- [ ] Emergency Break
- [x] Seat Belt - Even at the most intense crashes, your character stays inside the vehicle.
- [x] Never Fall off Bike - Stay on your bike at all costs.
- [x] Lock All Doors
- [ ] Vehicle Jump
- [x] Drive under water - Your engine won't die under water.
- [ ] Drive on water
- [ ] No Collision - Traffic heading straight towards you? You don't really care.
- [ ] Flying Vehicles

* **Weapons**
- [ ] Weapon Components Menu
- [x] Give all Weapons - Gives all Weapons
- [x] Infinite Ammo - Never run out of ammunition
- [x] No Reload - Combine with Infinite ammunition to wreak havoc
- [x] Explosive Bullets - Bullet impacts explode
- [x] Fire Bullets - Light sh*t on fire.
- [x] Levitation Gun - Entities float when you shoot them
- [x] Teleport Gun - Teleport to wherever you shoot
- [x] Gravity Gun - Lift aimed entities and move them around with your mouse. Release them again with a force depending on your mouse Movement speed
- [x] Black Hole Gun* - Create a black hole at a desired position and watch as the whole world around you is pulled towards it.
- [ ] Vehicle Gun Menu - Shoot vehicles.
- [ ] Portal Gun - Was a fun game, wasn't it?
- [ ] Seagull Gun - Everything you shoot turns into a Seagull
- [ ] Grappling Gun
- [ ] Ultra Rapid Fire
- [ ] Guided Bullets - Lock on to all targets on screen and shoot guided bullets at them.

Below features will be written later
* **Pedestrians**
* **Props**
* **World**

Save and load + auto-save and load also works! But this will be reworked later. 

## Hotkeys
At the moment hotkeys are hardcoded. This will be configurable in settings later. For now:

* **Menu** 
  * **F5** - **Open** latest **Menu** if none exists in memory, open **Main Menu**  
  * **Space** - **Add** or **Remove** vehicles to/from favorites.  
  * **Enter** - **Activates** a feature. In **Vehicle Weapons Menu** it will mark a **weapon** to use.  
  * **CTRL + SHIFT + S** - **Save** the current states.
  * **CTRL + SHIFT + L** - **Load** the saved states.  
  
* **Speed - Shift**  
  * **On Foot** - **Super Run**  
  * **In Vehicle** - **Speed Boost**  
  
* **Vehicle Weapons**  
  * **T** - **Shoot** the different vehicle weapons.  
  
* **Teleport to Waypoint**  
  * **CTRL + T** - **Teleport** to the desired location. If in a vehicle, teleport the vehicle.  
  
* **Gravity Gun**  
  * **HOLD J** - **Aim** at a **target** hold down the **J key** and **pick** up the target with your gravity gun. Moving the mouse around quickly and releasing the key again throws the entity in that direction.

## Credits
**Alexander Blade** - **ScriptHookV**  
**Crosire** - **ScriptHookVDotNet**   
**Lemon** - **LemonUI**  
**Guad** - **NativeUI** which **LemonUI** is based on.  
**JohnFromGWN** - For **feedback** and **suggestions**  

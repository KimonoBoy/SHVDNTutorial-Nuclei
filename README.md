# SHVDNTutorial-Nuclei
A successor to **NucleiLite** - this will build upon everything we learned here [NucleiLite - Wiki](https://github.com/KimonoBoy/SHVDNTutorial-NucleiLite/wiki)  

The idea of this **mod** is to **create** a **well-build**, **well-documented** overall **mod menu**, to **help** people interested in **developing mods** for **Grand Theft Auto V**

See the [Wiki](https://github.com/KimonoBoy/SHVDN-Tutorial/wiki)

## Current Mod Progress
Finally the Modifications Menu is ready to be finished.

* Finishing the Modifications Menu - or at least adding all the features, we might change how each item is represented later.

* Once the above is complete, we'll update the save & load functionality of our services, including the stored CustomVehicles.

## NOTE
I'm in an optimization Process with the mod itself. Don't worry every step up until this point will be covered, but the mod itself needs to go through a huge Refactor process first.
All these updates and changes atm is for my own personal test-stage, rethinking how the different layers should interact to make the most sense, and single-ton instances are a challenge to test, scale
and maintain - unfortunately deriving from the Script class doesn't allow for dependency injection, and state-management between different layers can be a tedious and cumbersome subject.

However, when I begin updating the wiki again, you should be able to catch up with all the changes made this far, but I'd rather make it right first than teach you something that is subject to change.

Both the **Wiki** and the **Mod** itself is a work in progress and it's in a very early stage.

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

## Credits
**Alexander Blade** - **ScriptHookV**  
**Crosire** - **ScriptHookVDotNet**   
**Lemon** - **LemonUI**  
**Guad** - **NativeUI** which **LemonUI** is based on.  
**JohnFromGWN** - For **feedback** and **suggestions**  

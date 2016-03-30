# Stardew Valley Farm Automation Mods
## Introduction
The purpose of these mods is to save you time doing tedious task without being completely unfair/cheating. Whether something is an unfair advantage or not is of course subjective, which is why you can disable various features. The goal is to make the game more enjoyable for you by removing aspects you find tedious or unfun.

### Disclaimer
These Mods are still in early development and might have bugs or unintended behaviour. Make sure you backup your safefiles before using them!
If you have problems, please give feedback over at the [Forums](http://community.playstarbound.com/threads/alpha-farmautomation-mods-for-smapi-0-39-2.111931)

### Requirements
You'll need SMAPI v0.39.0 or higher for these mods to work, see their [GitHub Page](https://github.com/ClxS/SMAPI) for more information

### Download
You can download the compiled mods under [latest releases](https://github.com/oranisagu/SDV-FarmAutomation/releases/latest).

## The Mods
The following mods are currently part of this project:

### ItemCollector
This is the main mod of this project. it's purpose is to collect items from your barns, coops and machines (in various locations) and put them in nearby chests.
If the chest contains appropriate raw materials (for example for Kegs it would be Fruit or Wheat or Hops), it will also immediately refill the machine with the first matching item in the chest. if, for example you have a keg farm somewhere, just put enough fruit into the chest and it should automatically create wine for you. This alleviates the tiresome running around refilling your machines.

*So how does it find the chests?* Simply put: if a machine touches a chest, it will use it. If not, but that machine touches another machine and that one touches a chest, it will use it. As long as there's another machine connected will keep looking. You can create as large rows (or fields, it works in all 4 directions) of machines as you want. 
In addition, it's now possible to use pathways like wood floor or cobblestone paths as well by adding the appropriate number into your configuration file (the default configuration file contains a list of the various paths and their numbers and only enables the wood floor by default).

You will want to shut off flooring tiles until you've setup your farm appropriately - otherwise it's possible everything connects to everything and uses the first available chest (don't worry if it's full, it won't deposit the items then, but it might take items for further processing you didn't want to process).

#### Installation
the installation is simple, download the zipfile and extract it into your mods folder (either %AppData%\StardewValley\Mods\ or the games installation directory). check the configuration file and read it carefully first, to avoid unexpected behaviour.

#### Configuration (Important!)
read the configuration file first and disable any machines or floorings you might not want. if you're unsure, don't run the mod without making a backup until it's in a more stable state.

### BarnDoorAutomation
This mod simply opens and closes the barn doors at configurable times. there is already another mod doing this so unless you have problems with that one, this probably won't help you much.

#### Installation
the installation is simple, download the zipfile and extract it into your mods folder (either %AppData%\StardewValley\Mods\ or the games installation directory) and start SMAPI.
there are some configuration options, though you can probably leave those at their defaults.

## Feedback
Feedback is very appreciated as there are so many situations I can't account for. If you have problems, consider sending me your savefile so I can look into it.

There's also my [Forum Post](http://community.playstarbound.com/threads/alpha-farmautomation-mods-for-smapi-0-39-2.111931), where you can leave suggestions or bug reports.

## Version History
#### [0.1.2](https://github.com/oranisagu/SDV-FarmAutomation/releases/tag/v0.1.2)
* Collection of Void Eggs
* Opening/Closing times for Barn/Coop doors were inverted
* animals get petted even if no chest is present in the building

#### [0.1.1](https://github.com/oranisagu/SDV-FarmAutomation/releases/tag/v0.1.1)
the same as 0.1.0-alpha, only updated assembly versions and added the version to the zip files.

#### [0.1.0-alpha](https://github.com/oranisagu/SDV-FarmAutomation/releases/tag/v0.1.0-alpha)
* floor tiles can now be used as connectors
* animals in coops are getting petted as well
* improved caching and performance
 
#### before GitHub
the first release was only posted on the community forums and offered basic functionality.

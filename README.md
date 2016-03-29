# Stardew Valley Farm Automation Mods

## Disclaimer
These Mods are still in early development and might have bugs or unintended behaviour. Make sure you backup your safefiles before using them!
If you have problems, please give feedback over at the [Forums](http://community.playstarbound.com/threads/alpha-farmautomation-mods-for-smapi-0-39-2.111931)

## Download
If you want to try the mods, head over to the [Forum Post](http://community.playstarbound.com/threads/alpha-farmautomation-mods-for-smapi-0-39-2.111931), for now I'll upload the mods there as zip files until I figure out how to handle this better with GitHub.

## About the Mods
The following mods are currently part of this project:

### ItemCollector
This is the main mod of this project. it's purpose is to collect items from your barns, coops and machines (in various locations) and put them in nearby chests.
if the chest contains appropriate raw materials, it will also immediately refill the machine. if, for example you have a keg farm somewhere, just put enough fruit into the chest
and it should automatically create wine for you. this alleviates the tiresome running around refilling your machines.

*So how does it find the chests?* Simply put: if a machine touches a chest, it will use it. if it touches another machine and that one touches a chest, it will use it.
and so on. you can create as large rows (or fields, it works in all 4 directions) of machines as you want. in addition, it's now possible to use pathways like 
boardwalks or cobblestone paths as well by adding the appropriate number into your configuration file (the default configuration file contains a list of the various paths and their numbers).

so you might want to shut off flooring tiles until you've setup your farm appropriately - otherwise it's possible everything connects to everything and uses the first available chest
(don't worry if it's full, it won't deposit the items then, but it might take items for further processing you didn't want to process).

#### Installation
the installation is simple, download the zipfile and extract it into your mods folder from Stardew Valley and start SMAPI.

#### Configuration (Important!)
read the configuration file first and disable any machines or floorings you might not want. if you're unsure, don't run the mod without making a backup until it's in a more stable state.

### BarnDoorAutomation
This mod simply opens and closes the barn doors at configurable times. there is already another mod doing this so unless you have problems with that one, this probably won't help you much.

#### Installation
copy the mod into your Stardew Valley Mods folder and start SMAPI.
there are some configuration options, though you can probably leave those at their defaults.
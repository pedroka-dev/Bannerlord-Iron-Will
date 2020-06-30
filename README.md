# But what is the "Iron Will" project?
  Iron Will is a collection of mods for the game Mount & Blade 2: Bannerlord that adds many utilities to aid the player and his party to survive even in the harshest, rock botton environments. The main objetive is add immersive actions to balance the game difficulty curve without decreasing the challenge. This modpack might be useful with any mods that makes the game harder and more punishing. 
  
# Current Mod List:
  - [Iron Will - Wound Experience](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797)
  - Iron Will - Kleptomania  (comming soon)
  - Iron Will - Battlefield Scavenging (comming soon)
  - Iron Will - Hunting Instinct (comming soon)
  - Iron Will - Shady Business (comming soon)

---

# Iron Will - Wound Experience

![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/Wound%20Experience%20Thumbnail.jpg?raw=true)

The best way to get stronger is by struggle. **The mod Wound Experience** adds experience points to any troop which survives getting wounded. With inspirations from Kenshi, this true "survival of the fittest" experience makes your troop battle hardened by just the simple fact of not dying after a possible fatal blow. 

This decreases the grind for upgrading low tier troops and actually rewards the player for letting his weak troopos risk their lives by fighting in battle. It also indirectly buffs Medicine skill, by making it kinda useful to upgrade troops. 

The mod works not only for the player, but for any mobile NPC parties too! That's right, even for the poor and dirty Looters! It only show as a message for the player character, his companions and owned troops by default.

# Download and Links

  - [Iron Will - Wound Experience Nexus Mods page](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797)
  - [Iron Will - Wound Experience TaleWorlds Forums page](https://forums.taleworlds.com/index.php?threads/iron-will-wound-experience.426533/)
  - [Iron Will - Wound Experience ModDB page](https://www.moddb.com/mods/iron-will-wound-experience)
  - [Iron Will Project GitHub](https://github.com/pedro-ca/bannerlord_iron_will)

# Features 
  - Adds Athletics Experience to **any** Hero wounded in battle (80xp by default. Almost 1/6 of the Athletics XP for level 25) 
  - Adds Generic Experience to **any** Troop wounded in battle (80xp by default. Almost 1/3 of de XP to upgrade a Aserai Recruit)
  - Player Character, Companions and Owned troops received XP appears in the ingame console.
  - Works globally to all troops in game, both in battle and campaing map.
  - Configurable Experience value for Heroes and  Generic Troops at "xxWoundXPSettings.xml"
  - NLogger for debugging reasons at "WoundExperienceLog.txt"
  - Only valid for **real** battles, so you can't cheese it by getting wounded in tournaments and arenas heh

  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/heroe%20athletic%20exp%20example.JPG?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/generic%20troop%20exp%20example.JPG?raw=true) 
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/owned%20troops%20in%20the%20console%20example.png?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/Configurable%20xp%20example.JPG?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/debug%20on%20example.JPG?raw=trueG)
  

# To-do
  - Implementation of optional configuration menu with [mildeww's ModLib](https://www.nexusmods.com/mountandblade2bannerlord/mods/592)
  - Toggleable linear escale Experience
  
  
# Compatibility
Currently working with e1.4.0, e1.4.1 and e.1.4.2 versions. Should be compatible with most mods.

If you have any issue, please adress it at the [Bugs tab](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797?tab=bugs) in the Nexus mod page.

# Changelog
v1.1.0:
- Changed the default experience value for both troops and heroes, from 20 to 80. 
- Added NLogger to debug and find problems.
- Information manager messages are now yellow, to increased visibility. 
- Added initial sucess message on campaing start to inform the user that the module is working.

v1.2.0:
  - Allows customization of values with xxWoundXPSettings.xml, located at the mod folder
  - All player owned troops that receives xp appears in the Console and Nlogger
  - WoundExperienceLog.txt NLogger is now at the mod folder and some log are more informative
  - The Information manager now shows more clearly any mod error/exception

# But what is the "Iron Will" project?
  Iron Will is a collection of mods for the game Mount & Blade 2: Bannerlord that adds many utilities to aid the player and his party to survive even in the harshest, rock botton environments. The main objetive is add immersive actions to balance the game difficulty curve without decreasing the challenge. This modpack might be useful with any mods that makes the game harder and more punishing. 
  
# Current Mod List:
  - [Iron Will - Wound Experience](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797)
  - [Iron Will - Kleptomania](https://www.nexusmods.com/mountandblade2bannerlord/mods/1997)
  - Iron Will - Battlefield Scavenging (comming soon)
  - Iron Will - Hunting Instinct (comming soon)

---

# Iron Will - Wound Experience

![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/Wound%20Experience%20Thumbnail.jpg?raw=true)

The best way to get stronger is by struggle. **The mod Wound Experience** adds experience points to any troop which survives getting wounded. With inspirations from Kenshi, this true "survival of the fittest" experience makes your troop battle hardened by just the simple fact of not dying after a possible fatal blow. 

This decreases the grind for upgrading low tier troops and actually rewards the player for letting his weak troopos risk their lives by fighting in battle. It also indirectly buffs Medicine skill, by making it kinda useful to upgrade troops. 

The mod works not only for the player, but for any mobile NPC parties too! That's right, even for the poor and dirty Looters! It only show as a message for the player character, his companions and owned troops by default.

This mod is compatible with the amazing [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612) by [Aragasas](https://www.nexusmods.com/mountandblade2bannerlord/users/2355906). This is a SOFT dependency and thus entirely optional but highly recommended. 

# Download and Links

  - [Iron Will - Wound Experience Nexus Mods page](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797)
  - [Iron Will - Wound Experience TaleWorlds Forums page](https://forums.taleworlds.com/index.php?threads/iron-will-wound-experience.426533/)
  - [Iron Will - Wound Experience ModDB page](https://www.moddb.com/mods/iron-will-wound-experience)

# Features 
  - Adds configurable Athletics Experience to **any** Hero wounded in battle (40xp by default) .
    - Hero XP scales with Learning Rate. Current: xpValue  * learningRateBonus. (can be turned off)
  - Adds configurable Generic Experience to **any** Troop wounded in battle (40xp by default).
    - Generic Troop XP scales with troop tier. Current: xpValue * (troop.Tier +1). (can be turned off)
  - Player Character, Companions and Owned troops received XP appears in the ingame console. (can be turned off)
  - Works globally to all troops in game, both in battle and campaing map.
  - Optional in-game configurable values with Mod Configuration Menu (MCM). This is a soft dependency.
  - Configurable Experience value for Heroes and  Generic Troops at "xxWoundXPSettings.xml". (note: MCM configuration takes priority and overrides this file)
  - NLogger for debugging reasons at "WoundExperienceLog.txt".
  - Only valid for **real** battles, so you can't cheese it by getting wounded in tournaments and arenas heh.
# Settings
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/heroe%20athletic%20exp%20example.JPG?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/generic%20troop%20exp%20example.JPG?raw=true) 
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/owned%20troops%20in%20the%20console%20example.png?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/Configurable%20xp%20example.JPG?raw=true)
  
  ![alt text](https://github.com/pedro-ca/bannerlord_iron_will/blob/master/WoundXP/Thumbnails/debug%20on%20example.JPG?raw=trueG)
 

# To-do
The mod is pretty much done :D

There might be some minor tweaks/bug fixes here and there in the future.

# Custom Configurations
Downloading [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612) by [Aragasas](https://www.nexusmods.com/mountandblade2bannerlord/users/2355906) is highly recommended for best quality of life by changing mod settings in game. This is a SOFT dependency and thus entirely optional by the player. MCM settings takes priority before the XML settings file. So if the player haves MCM, the code will load stored info from MCM and then override XML file on startup every time.

![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/WoundXP/Thumbnails/woundxp%20mcm%20screen.jpg)

However, if you choose to not install Mod Configuration Menu for some reason, you can change the desired values of "xxWoundXPSettings.xml
". In this case during startup, the settings loads from the XML at "\Mount & Blade II Bannerlord\Modules\xxWoundXp\xxWoundXPSettings.xml".
  
# Compatibility
Currently tested and working with e.1.4.2 and e1.4.3 version. Should be compatible with most mods.

If you have any issue, please adress it at the [Bugs tab](https://www.nexusmods.com/mountandblade2bannerlord/mods/1797?tab=bugs) in the Nexus mod page.

# Changelog
<details>
  <summary>Versions</summary>
  
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
  
  v1.3.0:
  - Changed default XP value for heroes and troops to 40
  - Hero XP scales with Learning Rate. Current: xpValue  * learningRateBonus
  - Generic Troop XP scales with troop tier. Current: xpValue * (troop.Tier +1)
  - Player can change to not show received XP of heroes and troops on the console (_Requested by NexusMod user dungeons0_)
  - Compatibility with game version Bannerlord Beta Branch e1.4.3
  
  v1.4.0
  - Minor code refactor
  - Added soft mod compatibility with Mod Configuration Menu v3(MCM)
  - If the player haves MCM, the code will now load stored info from MCM and then override XML file on startup .
  - If the player doesnt have MCM, the code will deserialize the settings from the XML like usual on startup.
  - Fixed some crashes for e1.5.2 and above
  
</details>

![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/WoundXP/Thumbnails/woundxp%20inspiration%20header.JPG)

*Kenshi, one of the major creative influences for this mod series.*

---
  
# Iron Will - Kleptomania

![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/Kleptomania%20Thumbnail.jpg)

Being a criminal in Bannerlord is now more viable and flavorfull, with **the mod Kleptomania**'s ability to steal trade goods and food from towns and villages. Although the interactions are limited to in-game menus, the goal is to create a Stealing mechanic with a feeling of risk vs reward similar from games like Skyrim, Kenshi, Fallout, Thief, and even Tabletop RPG.

After a simple Roguery skill check, you might be able to get an dynamic ammount of loot and leave without been seen by anyone. But BEWARE, if you get caught there will be consequences.

Keep in mind Stealing is different from the Native's Hostile Action "Force Peasants to give you supplies". The most important differences is that you dont have to fight the militia, chance to not get detected at all, can loot from towns, variable ammount of loot (sometimes more, sometimes less), amongst other things.

For now, only the player can interact with the steal menus.

This mod is compatible with the amazing [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612) by [Aragasas](https://www.nexusmods.com/mountandblade2bannerlord/users/2355906). This is a SOFT dependency and thus entirely optional but highly recommended. 

![alt text](https://staticdelivery.nexusmods.com/mods/3174/images/1997/1997-1596763421-1876725097.jpeg) 

# Download and Links
 - [Iron Will - Kleptomania Nexus Mods page](https://www.nexusmods.com/mountandblade2bannerlord/mods/1997)
 - [Iron Will - Kleptomania TaleWorlds Forums page](https://forums.taleworlds.com/index.php?threads/iron-will-kleptomania.428278/)
 - [Iron Will - Kleptomania ModDB page](https://www.moddb.com/mods/iron-will-kleptomania)

# Features
  - 3 new menus for stealing from Villages and Towns, which allows player to steal trading goods, food or animal.
  - Scalable Roguery XP on steal attempt, independent of the outcome. Current: 20 * learningRateBonus.
  - Can steal without being detected (Skill Check with modifiers)
  - If detected, decreases relationship with the settlement owner and settlement notables
  - If detected, applies criminal rating with faction.
  - If detected, player is confronted by civilian. The player can either bribe, persuade, call friendly gang leader support or give back. 
  - Dynammic minimun received ammount of goods (Skill Check with modifiers).
  - Max ammount of availiable goods scales with settlement prosperity / hearth.
  - On steal, decreases settlement prosperity / hearth, affects the settlement loot pool and increase detection for consecutive steals atempt of the same Faction.
  - Optional in-game configurable values with Mod Configuration Menu (MCM). This is a soft dependency.
  - Configurable Experience value for Heroes and  Generic Troops at "xxWoundXPSettings.xml". (note: MCM configuration takes priority and overrides this file)
  - NLogger for debugging reasons at "KleptomaniaLog.txt".
  
![alt text](https://staticdelivery.nexusmods.com/mods/3174/images/1997/1997-1597096891-131847187.jpeg) 
  
# Modifiers 
<details>
There are some conditions that can change the outcome of a steal attempt. To have the best outcome, you have to try to get as much bonuses and as little penalties as you can.

*Probability of detection (75% by default. Less = Good)*
  - Current Bonuses:
    - Roguery Skill (max bonus is 50%) = -(SkillLevel / 5)% 
    - Night time = -10%
  - Penalties: 
    - High Crime Rating = +20%
    - Recent steal attempt at same faction (Decays by -5% every day) = +(NumberOfAttempts * 10)%
    
 *Minimun ammount of Goods (30% by default. More = Good)*
   - Current Bonuses:
    - Roguery Skill (max bonus is 30%) = +(SkillLevel / 10)% 
 </details>
 
 ![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/steal%20from%20town%20thumbnail.jpg)
 ![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/steal%20from%20villages%20thumbnail.jpg)
 ![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/different%20results%20thumbnail.jpg)
 
# Results and Consequences
<details>
Remember: there is always a risk. Once the Modifiers have been calculated and you have waited long enough to find a steal opportunity at the settlement, two Random numbers from 1 to 100 will be created. Think about this like two d100, dices with 100 faces being thrown. These values will be used to calcule your steal result.

*For the detection check*, if the value are smaller than your "Detection Chance" modifier you get detected while stealling. If you get detected, you will receive some consequences like:
  - Criminal rating with faction (25 for towns and 20 for villages by default)
  - Decreases relationship with settlement notables (-15 by default, 2x if the player is from the same faction)
  - Decreases relationship with settlement owner (-15 by default, 2x if the player is from the same faction)
  - Only if from the same Faction, decreases relationship with Faction leader ( -15 x 2 by default)

*For the ammount of goods check*, each settlement has a "storage with goods inside", which has a dynamic number of goods based on the settlement health/prosperity. The random "dice" number will never go below "Minimun ammount of Goods" and will be a porcentage of the items inside the storage. No matter the outcome, this will take place on on loot:
  - Penalty of +10% detection chance for each consecutive steal atempt for the same Faction. Consecutive steal atempt decays by -5% every day.
  - Decreases settlement prosperity / hearth.
  - Subtract items from the settlement item pool.  (Be careful to not steal from settlement without any items, or you might get nothing.)
  - Roguery XP thats scales with Learning Rate. Current: 20 * learningRateBonus.
</details>

 ![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/steal%20caught%20screen.jpg)
 ![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/dynamic%20loot%20thumbnail.jpg)
 
# To-do
 - Pickpocketing 
 - Stealing from rich traders house
 - Bonus to detection and minimun goods from the party's best Bandit troops.
 - Stolen good fencing (aka not being able to sell to settlement it was stolen from).
 
 # Custom Configurations
Downloading [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612) by [Aragasas](https://www.nexusmods.com/mountandblade2bannerlord/users/2355906) is highly recommended for best quality of life by changing mod settings in game. This is a SOFT dependency and thus entirely optional by the player. MCM settings takes priority before the XML settings file. So if the player haves MCM, the code will load stored info from MCM and then override XML file on startup every time.

![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/kleptomania%20mcm%20screen.jpg)

However, if you choose to not install Mod Configuration Menu for some reason, you can change the desired values of "xxKleptomaniaSettings.xml
". In this case during startup, the settings loads from the XML at "\Mount & Blade II Bannerlord\Modules\xxKleptomania\xxKleptomaniaSettings.xml".


# Compatibility
Currently tested and working with e.1.4.3 version. 

If you have any issue, please adress it at the [Bugs tab](https://www.nexusmods.com/mountandblade2bannerlord/mods/1997?tab=bugs) in the Nexus mod page.

# Changelog
<details>
  <summary>Versions</summary>
  
v1.1.0:
 - Fixed a crash because of horses in a settlement (Reported by NexusMod user aerosmei1).
 - Fixed a crash when settlement doesnt have anymore supplies.
 - Player can now steal any kind of animal (horse, pig cattle, etc.)
 - Added Scalable Roguery XP on steal attempt, independent of the outcome. Current: 20 * learningRateBonus.
 - Adds 10% detection chance penalty for each consecutive steal atempt for Faction.
 - Consecutive steal atempt decays by 5% every day.
 - Added Debug messages when DebugInfo is turned on in config.
 
v1.1.1
 - Fixed a major crash when that happens when you reach severe crime rating while inside a town
 - Fixed a bug that gives the player Athletics XP on steal attempt instead of the expected Roguery XP
 - Decreaased default TownStealCrimeRating from 35 to 25. Makes stealling more viable.
 - Decreased default VillageStealCrimeRating from 30 to 20. Makes stealling more viable.
 - TownStealCrimeRating and VillageStealCrimeRating cant go above 60 (fixes crash)
 - Substantial code refactor and more try catches, to understand any future exceptions
 
v1.2.0
- MAJOR code refator to prevent future bugs and crashes
- Added encounter game menu when detected. The player can either bribe, persuade, call friendly gang leader support or give back. 
- Persuade and Bribe values are configurable (XML file or MCM)
- Added soft mod compatibility with Mod Configuration Menu v3(MCM)
- If the player haves MCM, the code will now load stored info from MCM and then override XML file on startup .
- If the player doesnt have MCM, the code will deserialize the settings from the XML like usual on startup.
- Fixed a bug that made Consecutive Steal Penalty not affect detection chance modifiers
- Fixed a bug that gave relationship penalty to Settlement's Gang Leaders Notables on detection. This was not supposed to happen.
- Removed TownStealCrimeRating and VillageStealCrimeRating max of 60
- Removed unecessary penalty for high crime rating
- No penalty to prosperity / hearth if there is no steal quantity

v1.2.1
- Recent Faction Steal Attempt Penalty is now stored in the current save file as it's supposed to
- Fixed a bug that showed the wrong value of earned Roguery XP by not taking accounts the calculations with Learning Rate
- Fixed a bug that made earned Roguery XP scale by Athletics Learning Rate instead of Roguery Learning Rate 
- User can configure to not show received roguery XP from steal attempts on the console (on MCM or XML settings)
- User can configure its  own value roguery XP from steal attempts. Always scales with learning rate. (on MCM or XML settings)


</details>

![alt text](https://raw.githubusercontent.com/pedro-ca/bannerlord_iron_will/master/Kleptomania/Thumbnails/kleptomania%20inspiration%20header.JPG)

*Kenshi, one of the major creative influences for this mod series.*


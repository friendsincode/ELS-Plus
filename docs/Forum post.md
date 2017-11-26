<div align=center>ELS FiveM</div>
<div align=center>

[![alt text](https://cdn.discordapp.com/attachments/294203997781360640/307530522681147392/FiveM-svg.svg)](http://fivem-scripts.net)
### More info can be found [here](https://github.com/ejb1123/ELS-FiveM)
</div>

---------
## LIGHTS  ARE NOT IMPLEMENTED AND DON'T WORK YET
----------

### Controls:

Team Text Chat Y key): horn control  
Select Unarmed Weapon 1 Key: Manual tone 1  
Select  Melee Weapon 2 Key: Manual tone 2  
Select Shotgun Weapon 3 Key: Manual tone 3  
Select Heavy Weapon 4 Key: Manual tone 4  
Select Special Weapon 5 Key: Dual Siren Toggle  
Chat All T key: Goes to next tone or plays tone 1  
Throw Grenade G Key: Toggles main siren  
Vehicle Horn E Key: Toggles vehicle's Emergency lights  

----------

[details=Features implemented and working]
- Horn and 4 manual tones.
- Main siren.
- Working siren sync.
- Custom control bindings
- ELS V file compatibility.
[/details]

[details=Features that still need to be finished]
- ESL panel.
- Secondary siren.
- Auxiliary siren.
- Support ELS V vehicles lighting system.
- More things that I cannot think of...
[/details]

[details=Pipe dreams]
- Allow custom sound to be used from the server.
[/details]


----------
The source code can be found on [GitHub](https://github.com/ejb1123/ELS-FiveM).

----------
### Change log
#### v0.0.4.0
- Use the Q key to manually sync the siren. 
- Passenger siren control

#### v0.0.3.2
- Added bug submission code but commented it out
- Fixed a bug related to lights not staying on for everyone
#### v0.0.3.1
- Changed License to LGPL v3.0
- Added better error reporting when encountering a XML file that is encoded with UTF-8 BOM.
- Refactored the Siren Control code to make it easier to add passenger siren control in the future
- Now capture XML issue and displays then on screen and sends the issue to the server
#### dev-0.0.3.0v
- Added dual siren controls.
- Added the ability to have custom xml files registered.
- Implemented more features from the VCF files.
- Added the ability to toggle the state of emergency lights
- Added a folder on the server that will store any bugs that occur.
- Now shows a notification if a bug occurs
#### dev-0.0.2.3v
* Added the ability to use only the lights with in game vehicles
* Fixed bugs related to how the Main Siren and Manual tones work together
#### dev-0.0.2.2v
* Made it so only the player of the character who is in sitting in the driver seat of a ELS enable vehicle can control the vehicles siren.
* Made it so only vehicles that have sirens have ELS.
 * this is done by checking the result of `ENTITY::GET_ENTITY_BONE_INDEX_BY_NAME(Els_vehicle,"siren1");`
Is true when the return value is not -1


#### dev-0.0.2.1v
* Fix a bug when spawning directly into a vehicle did not add ELS to the vehicle

----------
## Closed Polls

### How many siren tones do you want?
[poll type=number min=1 max=10 step=1]

[/poll]

### How Should the manual tones work?
[poll name=poll2 type=multiple min=1 max=3]
* Require a siren to be on. (ex. Main Siren G key)
* Should be independent, of the state of any siren
* Shoulc have a key to set if they should be allowed to act indipently of any siren state.
[/poll]

### Would you like to see passenger controlled sirens
[poll name=poll3 type=regular]
* Yes
* No
[/poll]

----------
### Downloads
[v0.0.4.0](https://github.com/ejb1123/ELS-FiveM/releases/)
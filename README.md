# ELS-Plus

See: https://github.com/friendsincode/ELS-Plus/issues to submit an issue or use our discord here: . It is important that if you test ELS Plus for FiveM that you submit issues so that we may work to resolve these. Here's some things to consider:

- ELS vehicles are fully supported.
- Full VCF parsing so you can totally customize vehicles.
- Custom cars will require VCF files to be created by following the [How to add custom Vehicles](#how-to-add-els-vehicles-to-els-fivem) section.
- You can spawn a vehicle using /elscar {model}
- You can obtain a list of the vehicles ELS has detected by typing /elslist and pressing F8 to view console.(Plans to make menu in progress :D)



### Default Controls

|Action Key|Default Key|Default Binding
|---|---|---|
| Vehicle Horn  | E key | Horn control|
| Select Unarmed Weapon | 1 Key | Wail tone |
| Select Melee Weapon | 2 Key | Yelp tone |
| Select Shotgun Weapon | 3 Key | Auxilary tone 1|
| Select Heavy Weapon | 4 Key | Auxilary tone 2|
| Select Special Weapon | 5 Key | Toggles Dual Siren Mode|
| Veh Cinema Cam | R Key|Goes to next tone or plays tone 1|
| Character Wheel | Left Alt Key |Toggles main siren|
| Radio Wheel | Q Key | Toggles vehicle's Light Stages using L Alt Modifier toggles backwards depending on Activation type |
| Select SMG | 7 Key | Toggles vehicle's Primary Patterns using L Alt Modifier toggles backwards |
| Select Auto Rifle | 8 Key | Toggles vehicle's Secondary Patterns using L Alt Modifier toggles backwards |
| Select Sniper Rifle | 9 Key | Toggles vehicle's Warning Patterns using L Alt Modifier toggles backwards |
| Vehicle Next Radio Track | . Key | Toggles vehicle's Takedown Lights (Extra 12) |
| Vehicle Next Radio Track | Alt + . Key | Toggles vehicle's Scene Lights (Extra 11) |
| Replay Show Hot Key | K Key | Toggles vehicle's secondary lights|
| Cinematic Slo Mo | L Key | Toggles vehicle's warning lights|
| Previous Weapon | [ Key | Toggles vehicle's Cruise Lights|
| Character Wheel + Previous Weapon | L Alt + [ | Toggles vehicles Arrowboard if equipped(Not fully working)|
| Duck + Pause | L Ctrl + P|Toggles UI Panel|
| Vehicle Previous Radio Track | - Key | Toggle Left Indicator |
| Vehicle Next Radio Track | = Key | Toggle Right Indicator |
| Cellphone Cancel | - Key | Toggle Hazards |
|---|---|---|
|Sniper Zoom in secondary|DPad Up|Toggle Auxilary Tone 1|
|Sniper Zoom out secondary|DPad Down|Toggle Main Siren|
|Detonate|DPad Left|Toggle Light Stages|
|Talk|DPad Right|Toggle Takedownlights (Extra 12)|
|Duck|L3(Press left stick)|Horn Control|
|Reload|B(XBox) or O/Circle (PS)|Wail Tone|
|Reload and Duck|L3(Press left stick) + B(XBox) or O/Circle (PS)|Yelp Tone|


### How to install
1. Copy the `els-plus` folder to `cfx-server\resources\`
2. Add `start els-plus` to `server.cfg`
3. Add `add_ace resource.RESOURCE_NAME command.add_ace allow` to `server.cfg`
   remember to replace `RESOURCE_NAME` with the name of this resource.
4. Modify settings via ini file how you see fit.
5. If you set AllowAdminOnly to `true` Add your user to the `group.admin`
   principal with the following `add_principal identifier.[license||steam]:id# group.admin`
   to allow you to use the elscar commmand to spawn a vehicle.
6. You can also add custom groups to els-plus to allow better control on who you wish to use the command.


### How to add ELS Vehicles to ELS-Plus
1. Create add-on/replace Vehicle with stream folder and relevant files.
2. In `__resource.lua` of that add-on/replace vehicle add the `VCF` xml file to the `files` list.
3. Make sure to validate the xml file to avoid issues with vehicle not showing up.
4. Add `is_els 'true'` to bottom of `__resource.lua`.
5. Restart Server
6. Profit

#### Important Notes

- When running the rcon command `restart els-plus` or `start els-plus`.

## Contribute
if you are a developer and  would like to contribute any help is welcome!
The contribution guide can be found [here](CONTRIBUTING.md).

### How to Build and Test

1. Add the enviroment variable `FXSERVERDATA` and set its value to the `resources` directory path.

2. `git clone https://github.com/friendsincode/ELS-Plus.git`

3. Open `ELS-Plus\src\elsplus.sln` in Visual Studio

4. Select `Release` and `Any CPU`  next to the Start button

5. In the menu bar under Build click on `Build Solution`

6. Copy all the files from `ELS-Plus\src\bin\Release` to `cfx-server\resources\els-plus`

7. Add `start els-plus` to `server.cfg`

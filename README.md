# disclaimer  
This project is still in development and should be expected to be buggy and not work in its current state  
# ELS-FiveM

### Default Controls

|Action Key|Default Key|Default Binding
|---|---|---|
|Team Text Chat | Y key | Horn control|
|Select Unarmed Weapon | 1 Key | Manual tone 1
|Select Melee Weapon | 2 Key | Manual tone 2|
|Select Shotgun Weapon | 3 Key | Manual tone 3|
|Select Heavy Weapon | 4 Key | Manual tone 4|
|Chat All| T Key|Goes to next tone or plays tone 1|
|Throw Grenade| G Key|Toggles main siren|

### How to Build and Test

1. `git clone https://github.com/ejb1123/ELS-FiveM.git`

2. Copy `{fivereborn}\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll` to `ELS-FiveM\libs`

3. Open `ELS-FiveM\src\ELS-for-FiveM.sln` in Visual Studio

4. Select `Release` and `Any CPU`  next to the Start button

5. In the menu bar under Build click on `Build Solution`

6. Copy all the files from `ELS-FiveM\src\bin\Release` to `cfx-server\resources\ELS-FiveM`

7. Add `ELS-FiveM` to `AutoStartResources` in `cfx-server\citmp-server.yml`

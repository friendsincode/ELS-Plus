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

### How to add custom Vehicles

1. Create a folder under resources with any name you want such as `cars`.
2. Create a file as UTF-8 without signature/BOM called `__resources.lua` in the folder you madin step 1.
3. Create a folder in the folder created in step 1 called `streams`.
4. For each vehicle you want to add create a folder with the name of the in game vehicle slot you want to replace such as `FBI2`
5. In the folder that was created in step 4 add the vehicle files. The name should be the same as folder name from step 4.
6. Create a folder called `ELS` in the folder that was created in step 4.
7. In the folder created in step 6 add the vehicles VCF file. The Name should be the name match the name of the vehicle files with a xml extension.
8. In the `__resource.lua` file add the code below
```
local function object_entry(data)
	files(data)
	ELSFMVCF(data)
end
object_entry('streams/CarName/ELS/CarName.xml')
```
9. Add the resource folder name to `AutoStartResources` in `citmp-server.yml` below `ELS-for-FiveM`
### How to Build and Test

1. `git clone https://github.com/ejb1123/ELS-FiveM.git`

2. Copy `{fivereborn}\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll` to `ELS-FiveM\libs`

3. Open `ELS-FiveM\src\ELS-for-FiveM.sln` in Visual Studio

4. Select `Release` and `Any CPU`  next to the Start button

5. In the menu bar under Build click on `Build Solution`

6. Copy all the files from `ELS-FiveM\src\bin\Release` to `cfx-server\resources\ELS-FiveM`

7. Add `ELS-FiveM` to `AutoStartResources` in `cfx-server\citmp-server.yml`

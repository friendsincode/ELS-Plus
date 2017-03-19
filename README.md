# disclaimer  
This project is still in development and should be expected to be buggy and not work in its current state  
# ELS-FiveM

### Defult Controls

|Action Key|Default Key|Default Binding
|---|---|---|
|Team Text Chat | (Y key) | horn control|
|Select Unarmed Weapon | 1 Key | Manual tone 1
|Select Melee Weapon | 2 Key | Manual tone 2|
|Select Shotgun Weapon | 3 Key | Manual tone 3|
|Select Heavy Weapon | 4 Key | Manual tone 4|


###How to Build and Test

`git clone https://github.com/ejb1123/ELS-FiveM.git`  
copy `{fivereborn}\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll` to `ELS-FiveM\libs`  
open `ELS-FiveM\src\ELS-for-FiveM.sln` with visual studio  
copy the files from `ELS-FiveM\src\bin\{Release Type}` to `cfx-server\resources\ELS-FiveM`  
add `ELS-FiveM` to `AutoStartResources` in `cfx-server\citmp-server.yml`

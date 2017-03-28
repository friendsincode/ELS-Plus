# DISCLAIMER  
This project is in early development stages. You should expect bugs and/or issues. To effectively combat these issues we ask that you submit issues to the Issues board in this repository. See: https://github.com/ejb1123/ELS-FiveM/issues to submit an issue. It is important that if you test ELS for FiveM that you submit issues so that we may work to resolve these. Here's some things to consider:

- ELS vehicles are not yet supported.
- Lights are not yet supported.
- Custom cars will require VCF files to be created by following the "How to add custom Vehicles" section below.

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
2. Create a file as UTF-8 without signature/BOM called `__resource.lua` in the folder you made in step 1.
3. Create a folder  called `streams` in the folder created in step 1.
4. For each vehicle you want to add create a folder with the name of the vehicle slot you want to replace such as `FBI2`
5. In the folder that was created in step 4 add the vehicle files.
6. Create a folder called `ELS` in the folder that was created in step 4.
7. In the folder created in step 6 add the repective vehicle's VCF file. The Name should be the name match the name of the vehicle files with a xml extension.
8. In the `__resource.lua` file add the code below
```
local function vcf_entry(data)
	files(data)
	ELSFMVCF(data)
end

vcf_entry('streams-(step-3)/CarName-(step-4)/ELS-(step 6)/CARNAME.xml (step 7)')
vcf_entry('streams-(step-3)/PoliceName-(step-4)/ELS-(step-6)/POLICENAME.xml (step 7)')
```
9. Add the resource folder name to `AutoStartResources` in `citmp-server.yml` below `ELS-for-FiveM`
#### Important Notes

- When running the rcon command `restart ELS-for-FiveM` or `start ELS-for-FiveM`.  
Make sure you restart any resources that have ELS vehicles.
- Make sure ELS-for-FiveM is located above all ELS enabled vehicle stream resources in the `AutoStartResources` section in the `citmp-server.yml` file.

### How to Build and Test

1. `git clone https://github.com/ejb1123/ELS-FiveM.git`

2. Copy `{fivereborn}\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll` to `ELS-FiveM\libs`

3. Open `ELS-FiveM\src\ELS-for-FiveM.sln` in Visual Studio

4. Select `Release` and `Any CPU`  next to the Start button

5. In the menu bar under Build click on `Build Solution`

6. Copy all the files from `ELS-FiveM\src\bin\Release` to `cfx-server\resources\ELS-FiveM`

7. Add `ELS-FiveM` to `AutoStartResources` in `cfx-server\citmp-server.yml`

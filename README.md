# DISCLAIMER  
This project is in early development stages. You should expect bugs and/or issues. To effectively combat these issues we ask that you submit issues to the Issues board in this repository. See: https://github.com/FiveM-Scripts/ELS-FiveM/issues to submit an issue. It is important that if you test ELS for FiveM that you submit issues so that we may work to resolve these. Here's some things to consider:

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
|Select Special Weapon | 5 Key | Toggles Dual Siren Mode|
|Chat All| T Key|Goes to next tone or plays tone 1|
|Throw Grenade| G Key|Toggles main siren|
|Vehicle Horn | E Key | Toggles vehicle's emergency lights|

### How to add custom Vehicles

1. Download the example resource for custom cars: https://github.com/FiveM-Scripts/ELS-FiveM/raw/master/src/Help/cars.zip
2. For each vehicle you want to add create a folder with the name of the vehicle slot you want to replace such as `FBI2`. This folder **must** be in all caps. No exceptions.
3. In the folder that was created in step 2 add the vehicle files directly to that folder.
4. Create a folder called `ELS` in the folder that was created in step 3, or, copy the ELS folder from the example and paste it in here. It won't matter at this stage of the project.
5. In the folder created in step 4 add the repective vehicle's VCF file. The Name should be the name match the name of the vehicle files (all caps, though) with an xml extension, like shown in the example resource.
6. In the `__resource.lua` file copied from the example resource, check out the `vcf_loader` function and modify it to include your cars. If you have a custom car in the `police` and `police2` slot, this is how it would look:

```
local function vcf_loader()
  local vcf_files = {
	-- We modified inside of this Lua table to create a list of strings containing our car names.
	'POLICE',
	'POLICE2'
  }
  for i = 1, #vcf_files do
	local car = vcf_files[i]:upper()
	files('stream/' .. car .. '/ELS/' .. car .. '.xml')
	ELSFMVCF('stream/' .. car .. '/ELS/' .. car .. '.xml')
  end
end
```

7. **Double-check everything is in the right directory tree!** If the directories are not created properly the vcf_loader() will NOT import the VCF files properly into ELS.

9. Add the resource folder name to `AutoStartResources` in `citmp-server.yml` below `ELS-for-FiveM`
#### Important Notes

- When running the rcon command `restart ELS-for-FiveM` or `start ELS-for-FiveM`.  
Make sure you restart any resources that have ELS vehicles.
- Make sure ELS-for-FiveM is located above all ELS enabled vehicle stream resources in the `AutoStartResources` section in the `citmp-server.yml` file.

### How to Build and Test

1. `git clone https://github.com/FiveM-Scripts/ELS-FiveM.git`

2. Open `ELS-FiveM\src\ELS-for-FiveM.sln` in Visual Studio

3. Select `Release` and `Any CPU`  next to the Start button

4. In the menu bar under Build click on `Build Solution`

5. Copy all the files from `ELS-FiveM\src\bin\Release` to `cfx-server\resources\ELS-FiveM`

6. Add `ELS-FiveM` to `AutoStartResources` in `cfx-server\citmp-server.yml`

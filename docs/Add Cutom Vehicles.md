# How to add custom Vehicles

1. Download the example resource for custom cars: [example cars](https://github.com/FiveM-Scripts/ELS-FiveM/raw/master/docs/Help/cars.zip)
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

9. Add the resource folder name to `AutoStartResources` in `citmp-server.yml` below `ELS-FiveM`
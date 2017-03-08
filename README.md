# disclaimer  
This project is still in development and should be expected to be buggy and not work in its current state  
# ELS-FiveM

##How to Build and Test
`git clone https://github.com/ejb1123/ELS-FiveM.git`  
copy `{fivereborn}\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll` to `ELS-FiveM\libs`  
open `ELS-FiveM\src\ELS-for-FiveM.sln` with visual studio
copy the files from 'ELS-FiveM\src\bin\{Release Type}' to 'cfx-server\resources\ELS-FiveM'
add 'ELS-FiveM' to 'AutoStartResources' in 'cfx-server\citmp-server.yml'

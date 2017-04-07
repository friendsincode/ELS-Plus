
local NUIEnabled = false

-- XML Processing Requirements
client_script 'System.Xml.net.dll'

-- ELS-FiveM Requirements
client_script 'SharpConfig.net.dll'
client_script 'ELS-for-FiveM.net.dll'

-- NUI Stuff
if NUIEnabled then

	client_script 'NUI/api.lua'

	ui_page('NUI/app/index.html')

	files({
		'NUI/app/index.html',
		'NUI/app/js/vue.js',
		'NUI/app/js/app.js'
	})

end


-- Server Events
server_script 'server.lua'

-- Object Entry
local function ini_entry(data)
	files(data)
	ELSFM(data)
end

local function vcf_entry(data)
	files(data)
	ELSFMVCF(data)
end

ini_entry('extra-files/ELS.ini')
vcf_entry('extra-files/POLICE.xml')
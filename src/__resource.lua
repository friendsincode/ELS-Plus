client_script 'ELS-for-FiveM.net.dll'
client_script 'INIFileParser.net.dll'
server_script 'server.lua'
local function object_entry(data)
	local file =''
	for w in data:gmatch(".*/(.*)") do file=w end
	files(data)
	ELSFM(data)
end

object_entry('extra-files/ELS.ini')
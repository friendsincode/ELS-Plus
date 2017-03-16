client_script 'System.Xml.net.dll'
client_script 'SharpConfig.net.dll'
client_script 'ELS-for-FiveM.net.dll'
server_script 'server.lua'
local function object_entry(data)
	files(data)
	ELSFM(data)
end

object_entry('extra-files/ELS.ini')
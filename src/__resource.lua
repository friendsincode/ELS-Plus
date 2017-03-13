client_script 'ELS-for-FiveM.net.dll'
server_script 'server.lua'
local function object_entry(data)
	--dependency 'object-loader'

	files(data)
	settings(data)
end

object_entry('settings.xml')
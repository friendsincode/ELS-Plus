--[[
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
]]
resource_manifest_version '05cfa83c-a124-4cfa-a768-c24a5811d8f9'
local NUIEnabled = false

-- XML Processing Requirements
client_script 'System.Xml.net.dll'

-- ELS-FiveM Requirements
client_script 'SharpConfig.net.dll'
client_script 'ELS-FiveM.net.dll'

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
server_script 'ELS-Server.net.dll'

files({
'extra-files/ELS.ini',
'extra-files/POLICE.xml'})

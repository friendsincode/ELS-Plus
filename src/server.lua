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
RegisterServerEvent("sirenStateChanged")
RegisterServerEvent("ELS")
RegisterServerEvent("ELS:FullSync")
RegisterServerEvent("ONDEBUG")

if clr.System.IO.Directory.Exists("resources/" .. GetInvokingResource() .. "/bugs") ==false then
	clr.System.IO.Directory.CreateDirectory("resources/" .. GetInvokingResource() .. "/bugs")
end

AddEventHandler("sirenStateChanged",function (vehnetId,netSoundid,propertyName,state)
	--print(netId .. propertyName .. state)
	TriggerClientEvent("sirenStateChanged",-1,vehnetId,netSoundid,propertyName,state)
	local players=GetPlayers()
	print(players)
	for x,y in ipairs(players) do
		print(x .. " "  .. y)
	end
end)

AddEventHandler("ELS",function(type,netId,state)
	print(type .. " " .. netId .. " "  .. state)
	TriggerClientEvent("ELS:SirenUpdated",-1,type,netId,state)
end)

local function PrintTable(table)
	if type(table) == 'table' then
		foreach v in table do
			print(v)
			if type(v) == 'table' then
				PrintTable(v)
			end
		end
	end
end

AddEventHandler("ELS:FullSync",function(DataType,DataDic,PlayerId)
	--print(type(DataType),type(DataDic),type(PlayerId))
	--PrintTable(DataType)
	--if DataType == 'Tones' then
		PrintTable(DataDic)
	
	TriggerClientEvent("ELS:NewFullSyncData",-1,DataType,DataDic,PlayerId)
end)


local function getAllSubDirs(directory)
    local tabletoreturn={}
    local dfiles = clr.System.IO.Directory.GetDirectories(directory,"*",clr.System.IO.SearchOption.AllDirectories)
    foreach v in dfiles do
	print(type(v))
        table.insert(tabletoreturn,v)
    end
	--print(typeOf(dfiles))
    return dfiles
end



AddEventHandler("ONDEBUG",function(error)
	 --[[PerformHttpRequest('http://127.0.0.1:8000/bugs/submit', function(err, text, headers)
        if text then
            print(json.decode(text))
        end
        if err then
            print("error " .. err)
        end
    end, 'POST', json.encode({version='0.0.3.2',project_id=3,details=error}), { ["Content-Type"] = 'application/json' })]]
	local file = clr.System.IO.FileInfo("resources/" .. GetInvokingResource() .. "/bugs/bug-" .. clr.System.DateTime.Now.ToFileTimeUtc().ToString() .. "." .. "bug")
	local tw = file.CreateText()
	local stc = json.encode({error})
	print(stc)
	tw.Write(stc)
	tw.Flush()
	tw.Close()
	--file.AppendText(error)
end)
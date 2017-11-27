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
RegisterServerEvent("ELS")
RegisterServerEvent("ELS:FullSync")
RegisterServerEvent("ONDEBUG")
RegisterServerEvent("ELS:FullSync:Request")
--[[
if clr.System.IO.Directory.Exists("resources/" .. GetInvokingResource() .. "/bugs") ==false then
	clr.System.IO.Directory.CreateDirectory("resources/" .. GetInvokingResource() .. "/bugs")
end]]

AddEventHandler("ELS",function(type,netId,PlayerId,state)
	--print(type .. " " .. netId .. " "  .. state)
	TriggerClientEvent("ELS:SirenUpdated",-1,type,netId,PlayerId,state)
end)

local function PrintTable(table)
	if type(table) == 'table' then
		for k,v in pairs(table) do
			if type(v) == 'table' then
				PrintTable(v)
			end
			if type(v) == 'string' then
				print(string.format("%-20s",k),v)
			end
			if type(v) == 'number' then
				print(string.format("%-20f",k),v)
			end
		end
	end
end

AddEventHandler("ELS:FullSync:Request",function(NetworkId)
	print(source," is requsting ELS sync data")
	TriggerClientEvent(0,"ELS:FullSync:Request",NetworkId)
end)
--TriggerClientEvent()

AddEventHandler("ELS:FullSync",function(DataType,DataDic,NetworkId)
	--print(type(DataType),type(DataDic),type(PlayerId))
	--PrintTable(DataType)
	--if DataType == 'Tones' then
	PrintTable(DataDic)
	
	TriggerClientEvent("ELS:NewFullSyncData",-1,DataType,DataDic,NetworkId)
end)

--[[
local function getAllSubDirs(directory)
    local tabletoreturn={}
    local dfiles = clr.System.IO.Directory.GetDirectories(directory,"*",clr.System.IO.SearchOption.AllDirectories)
    for v in dfiles do
	print(type(v))
        table.insert(tabletoreturn,v)
    end
	--print(typeOf(dfiles))
    return dfiles
end]]



AddEventHandler("ONDEBUG",function(error)
	 --[[PerformHttpRequest('http://127.0.0.1:8000/bugs/submit', function(err, text, headers)
        if text then
            print(json.decode(text))
        end
        if err then
            print("error " .. err)
        end
    end, 'POST', json.encode({version='0.0.3.2',project_id=3,details=error}), { ["Content-Type"] = 'application/json' })
	local file = clr.System.IO.FileInfo("resources/" .. GetInvokingResource() .. "/bugs/bug-" .. clr.System.DateTime.Now.ToFileTimeUtc().ToString() .. "." .. "bug")
	local tw = file.CreateText()
	local stc = json.encode({error})
	print(stc)
	tw.Write(stc)
	tw.Flush()
	tw.Close()
	--file.AppendText(error)]]
end)

RegisterServerEvent("sirenStateChanged")
RegisterServerEvent("ELS")
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

AddEventHandler("ELS",function(type,netId,siren,state)
	print(netId .. " " .. siren .. state)
	TriggerClientEvent("ELS:SirenUpdated",-1,netId,siren,state)
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
	--[[PerformHttpRequest('http://localhost:8080/bugsubmit', function(err, text, headers)
		if text then
			print(text)
		end
	end, 'POST', json.encode({ msg = error }), { ["Content-Type"] = 'application/json' }) ]]
	local mp = require 'system/MessagePack'
	local file = clr.System.IO.FileInfo("resources/" .. GetInvokingResource() .. "/bugs/bug-" .. clr.System.DateTime.Now.ToFileTimeUtc().ToString() .. "." .. "bug")
	local tw = file.CreateText()
	local stc = json.encode({error})
	print(stc)
	tw.Write(stc)
	tw.Flush()
	tw.Close()
	--file.AppendText(error)
end)
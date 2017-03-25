RegisterServerEvent("sirenStateChanged")
RegisterServerEvent("ELS")
RegisterServerEvent("error")

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

AddEventHandler("error",function(error)
	print(error)
end)

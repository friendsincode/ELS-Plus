RegisterServerEvent("sirenStateChanged")
RegisterServerEvent("ELS")
AddEventHandler("sirenStateChanged",function (vehnetId,netSoundid,propertyName,state)
	--print(netId .. propertyName .. state)
	TriggerClientEvent("sirenStateChanged",-1,vehnetId,netSoundid,propertyName,state)
	local players=GetPlayers()
	print(players)
	for x,y in ipairs(players) do
		print(x .. " "  .. y)
	end
end)
AddEventHandler("ELS",function(type,netId)
	TriggerClientEvent("ELS:SirenUpdated",-1,netId)
	end)
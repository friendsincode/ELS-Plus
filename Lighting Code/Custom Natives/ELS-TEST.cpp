#include "StdInc.h"
#include <GameInit.h>

#include <GlobalEvents.h>
#include <nutsnbolts.h>
#include <ScriptEngine.h>
#include <gameSkeleton.h>

#include <Hooking.h>


static hook::cdecl_stub<void*(int handle)> gettest1([]() {
	return hook::pattern("44 89 4C 24 20 48 83 EC  28 0F 29 74 24 10 0F 57 C0").count(1).get(0).get<void>();

});
static hook::cdecl_stub<void*(int handle)> gettest2([]() {
	return hook::pattern("F3 0F 11 4C 24 40 F3 0F 11 7C 24 38 0F 28 D3 48 89 44 24 30 44 0B C9").count(1).get(0).get<void>();

});

//static hook::cdecl_stub<void*(int handle)> gettest3([]() {
//	return  hook::pattern("48 8B 03 45 33 C0 B2 01 48 8B CB FF 90 ?? 05 00 00 48 8B 4B 20").count(1).get(0).get<void>();
//
//});
/*
it's seemingly (temporarily) replacing the call at (48 8B 03 45 33 C0 B2 01 48 8B CB FF 90 ?? 05 00 00 48 8B 4B 20 + 11) with a NOP
that specific CVehicle function indeed seems responsible for repairing, and that call is used in a number of functions (applying mods, extras, ...)
the best way to do this of course would be having an actual flag per vehicle that specifies if this call should be allowed or not, using a wrapper stub

*/

static InitFunction initfunc() {
	fx::ScriptEngine::RegisterNativeHandler("test1", [](fx::ScriptContext& context) {
		context.SetResult(gettest1(0));
	});
	fx::ScriptEngine::RegisterNativeHandler("test2", [](fx::ScriptContext& context) {
		context.SetResult(gettest2(0));
	});
	/*fx::ScriptEngine::RegisterNativeHandler("test3", [](fx::ScriptContext& context) {
		context.SetResult(gettest3(0));
	});*/
}


#include "StdInc.h"
#include <GameInit.h>

#include <GlobalEvents.h>
#include <nutsnbolts.h>
#include <ScriptEngine.h>
#include <gameSkeleton.h>

#include <Hooking.h>


static hook::cdecl_stub<uint32_t()> gettest1([]() {
	return hook::get_pattern("44 89 4C 24 20 48 83 EC  28 0F 29 74 24 10 0F 57 C0", 0);

});
static hook::cdecl_stub<uint32_t()> gettest2([]() {
	return hook::get_pattern("F3 0F 11 4C 24 40 F3 0F 11 7C 24 38 0F 28 D3 48 89 44 24 30 44 0B C9", 0);

});

static hook::cdecl_stub<uint32_t()> gettest3([]() {
	return  hook::get_pattern("48 8B 03 45 33 C0 B2 01 48 8B CB FF 90 ?? 05 00 00 48 8B 4B 20", 0);

});

static InitFunction initfunc() {
	fx::ScriptEngine::RegisterNativeHandler("test1", [](fx::ScriptContext& context) {
		context.SetResult(gettest1());
	});
	fx::ScriptEngine::RegisterNativeHandler("test2", [](fx::ScriptContext& context) {
		context.SetResult(gettest2());
	});
	fx::ScriptEngine::RegisterNativeHandler("test3", [](fx::ScriptContext& context) {
		context.SetResult(gettest3());
	});
}


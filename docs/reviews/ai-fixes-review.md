# Review: friendsincode/ELS-Plus branch `ai-fixes` (5 commits on top of master 52e52c4)

Reviewed 2026-10-14/16. Branch head at review time: 4759368.
Reviewed from a local clone: `git diff master..ai-fixes`, `python3 utils/check-build.py`, and the
two test projects (logs not included here).

## Verdict
Mergeable at its head. Real design improvement (server-issued ELS identity bound to the server
entity, validated + authorized + rate-limited state, no more `elsplus_data` convar, telemetry removed).
Two things to fix before merge: the intermediate commit 2556f80 breaks all sync by itself, and the
validator silently drops all state on any client/server field mismatch.

## What was run (on Linux, dotnet SDK 9.0.121)
| check | result |
|---|---|
| `python3 utils/check-build.py` (real client+server sources vs CitizenFX.Core.Client NuGet, net452, C# 7.3) | Client passed, Server passed |
| `tests/RegressionChecks` | 43 checks passed |
| `tests/StreamingChecks` (runs production VehicleManager against fakes) | 27 checks passed |

Both test csproj files target net10.0 (not installed); they were retargeted to net9.0 in a throwaway
copy to run. Nothing exercises GTA audio/rendering/real replication (README says so too).

## Per commit
1. **c4eaf61 Fix resource manifest and client build packaging** - correct. `Global.cs`→`global.cs`
   matches disk (Linux-safe); `FullSync\FullSync.cs` was missing from the compile list; the removed
   jquery manifest entry pointed at a nonexistent file; `/onesync` dependency added; prebuild.ps1
   version regex no longer rewrites every dotted number.
2. **04388a4 Fix optional lighting state and siren playback lifecycle** - fixes `data.WRN != null &&
   data.SEC.Count` bug; extras applied by key lookup (no KeyNotFound); `[Optional]` struct params
   became `T?`; Tone ctor now calls SetState after sound set registration (real ordering bug).
   Behaviour changes: removed guard that skipped playing tones for the local player's own vehicle
   (local tones now use PlaySoundFromEntity like remote); non-DLC sound set passed as null instead
   of "0". Both unverified in game. `SetData` now sets stage before patterns (was after) - no
   observed problem, eyeball `Stage.SetStage`.
3. **2556f80 Repair sync and UI initialization with authenticated vehicle updates** - server:
   `SyncPayload.TryParse` strict validator, `ControlledVehicle` (driver or seat 0) authorization,
   per-player rate limits, `_entities` id→server-entity map, identity resolve event, admin commands
   now ACE-restricted, update-check/GHMatti removed, `elsplus_data` convar removed. Client:
   VehicleManager rewritten (pending-state map, snapshot requests, reconcile every 250 ms instead
   of per-frame World.GetAllVehicles, makenetworked async/bool). **Broken on its own**: client
   serializes `LightFSData.GtaSirens` but the validator's exact field count omits it → every
   payload rejected until commit 5. Squash/reorder if bisecting matters.
4. **bb144fb Add portable compile checks and sync regression coverage** - check-build.py is honest
   (builds real sources). Regression suite covers validator edges, authorization, identity
   stability, rate limiting, tone lifecycle.
5. **4759368 Preserve ELS state across streaming and ownership handoffs** - adds GtaSirens to the
   validator, `entityRemoved` server handler, 500 ms server tick refreshing control, streaming
   suite, README notes.

## Concerns to act on
- **Strict field matching**: `SyncPayload.Fields` requires exact property count; any version skew or
  new client field silently rejects all state. Rejections are bare `return`s with no log. Add a
  debug log on rejection; consider accepting a superset.
- **Blocking preload**: `PreloadSyncData().GetAwaiter().GetResult()` in the server ctor is safe only
  while loaders are synchronous file scans; an `await Delay` inside later deadlocks the script.
- **OneSync is mandatory now** (`ControlledVehicle` uses server-side ped/vehicle data). README and
  manifest say so; confirm acceptable for users.
- **Test TFM**: lower tests to net8.0/net9.0 or multi-target so contributors can run them.

## Smaller observations
- Front passenger can control siren/lights (README test plan says intended).
- A parked, never-driven ELS car has no identity; observers ignore it until a driver resolves one.
- README cites "supplied GTA sources" for the streaming cross-check; reconsider that wording for a
  public README.
- `ELSVehicle` ctor now broadcasts only when the local player drives (previously always).
- `ELS:VehicleEntered` is now sent only back to the originating client with its own handle
  (previously broadcast a client-local handle to everyone - a bug).

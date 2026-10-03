### ELS-Plus

See: https://git.friendsincode.com/friendsincode-public/elsplus/issues to submit an issue or use our discord here: . It is important that if you test ELS Plus for FiveM that you submit issues so that we may work to resolve these. Here's some things to consider:

- ELS vehicles are fully supported.
- Full VCF parsing so you can totally customize vehicles.
- Custom cars will require VCF files to be created by following the [How to add custom Vehicles](#how-to-add-els-vehicles-to-els-fivem) section.
- You can spawn a vehicle using /elscar {model}
- You can obtain a list of the vehicles ELS has detected by typing /elslist and pressing F8 to view console.(Plans to make menu in progress :D)

### Default Controls

| Action Key                        | Default Key                                     | Default Binding                                                                                    |
| --------------------------------- | ----------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| Vehicle Horn                      | E key                                           | Horn control                                                                                       |
| Select Unarmed Weapon             | 1 Key                                           | Wail tone                                                                                          |
| Select Melee Weapon               | 2 Key                                           | Yelp tone                                                                                          |
| Select Shotgun Weapon             | 3 Key                                           | Auxilary tone 1                                                                                    |
| Select Heavy Weapon               | 4 Key                                           | Auxilary tone 2                                                                                    |
| Select Special Weapon             | 5 Key                                           | Toggles Dual Siren Mode                                                                            |
| Veh Cinema Cam                    | R Key                                           | Goes to next tone or plays tone 1                                                                  |
| Character Wheel                   | Left Alt Key                                    | Toggles main siren                                                                                 |
| Radio Wheel                       | Q Key                                           | Toggles vehicle's Light Stages using L Alt Modifier toggles backwards depending on Activation type |
| Select SMG                        | 7 Key                                           | Toggles vehicle's Primary Patterns using L Alt Modifier toggles backwards                          |
| Select Auto Rifle                 | 8 Key                                           | Toggles vehicle's Secondary Patterns using L Alt Modifier toggles backwards                        |
| Select Sniper Rifle               | 9 Key                                           | Toggles vehicle's Warning Patterns using L Alt Modifier toggles backwards                          |
| Vehicle Next Radio Track          | . Key                                           | Toggles vehicle's Takedown Lights (Extra 12)                                                       |
| Vehicle Next Radio Track          | Alt + . Key                                     | Toggles vehicle's Scene Lights (Extra 11)                                                          |
| Replay Show Hot Key               | K Key                                           | Toggles vehicle's secondary lights                                                                 |
| Cinematic Slo Mo                  | L Key                                           | Toggles vehicle's warning lights                                                                   |
| Previous Weapon                   | [ Key                                           | Toggles vehicle's Cruise Lights                                                                    |
| Character Wheel + Previous Weapon | L Alt + [                                       | Toggles vehicles Arrowboard if equipped(Not fully working)                                         |
| Duck + Pause                      | L Ctrl + P                                      | Toggles UI Panel                                                                                   |
| Vehicle Previous Radio Track      | - Key                                           | Toggle Left Indicator                                                                              |
| Vehicle Next Radio Track          | = Key                                           | Toggle Right Indicator                                                                             |
| Cellphone Cancel                  | - Key                                           | Toggle Hazards                                                                                     |
| ---                               | ---                                             | ---                                                                                                |
| Sniper Zoom in secondary          | DPad Up                                         | Toggle Auxilary Tone 1                                                                             |
| Sniper Zoom out secondary         | DPad Down                                       | Toggle Main Siren                                                                                  |
| Detonate                          | DPad Left                                       | Toggle Light Stages                                                                                |
| Talk                              | DPad Right                                      | Toggle Takedownlights (Extra 12)                                                                   |
| Duck                              | L3(Press left stick)                            | Horn Control                                                                                       |
| Reload                            | B(XBox) or O/Circle (PS)                        | Wail Tone                                                                                          |
| Reload and Duck                   | L3(Press left stick) + B(XBox) or O/Circle (PS) | Yelp Tone                                                                                          |

### How to install

1. Copy the `els-plus` folder to your server resource folder
2. Add `start els-plus` to `server.cfg`
3. Add `add_ace resource.NAME_OF_ELS_RESOURCE command.add_ace allow` to `server.cfg`
   remember to replace `NAME_OF_ELS_RESOURCE` with the name of this resource.
4. Modify settings via ini file how you see fit.
5. If you set AllowAdminOnly to `true` Add your user to the `group.admin`
   principal with the following `add_principal identifier.[license||steam]:id# group.admin`
   to allow you to use the elscar commmand to spawn a vehicle.
6. You can also add custom groups to els-plus to allow better control on who you wish to use the command.

### How to add ELS Vehicles to ELS-Plus

1. Create add-on/replace Vehicle with stream folder and relevant files.
2. In `__resource.lua` of that add-on/replace vehicle add the `VCF` xml file to the `files` list.
3. Make sure to validate the xml file to avoid issues with vehicle not showing up.
4. Add `is_els 'true'` to bottom of `__resource.lua`.
5. Restart Server
6. Profit

#### Important Notes

- When running the rcon command `restart els-plus` or `start els-plus`.

### Contribute

if you are a developer and would like to contribute any help is welcome!
The contribution guide can be found [here](CONTRIBUTING.md).

### How to Build and Test

1. Add the enviroment variable `FXSERVERDATA` to your computer's environment variables and set its value to the `resources` directory path.

2. `git clone https://git.friendsincode.com/friendsincode-public/elsplus.git`

3. Open `ELS-Plus\src\elsplus.sln` in Visual Studio

4. Select `Release` and `Any CPU` next to the Start button

5. In the menu bar under Build click on `Build Solution`

6. Copy all the files from `ELS-Plus\src\bin\Release` to your server resources folder as `els-plus`

7. Add `start els-plus` to `server.cfg`

### AI fixes branch: compatibility and verification

This branch requires **OneSync** for server-side checks of vehicle occupants and
entity lifetime. The manifest declares that requirement. Deploy the client and
server assemblies together and restart the resource; the sync payload now carries
both the stable `elsplus_id` decorator and the current `NetworkId`.

ELS identity is not derived from the network ID or the current owner. Clients
resolve streamed vehicles by the ELS decorator and retain pending state while an
entity is outside their scope. Network IDs are used only for server-side entity
lookup and authorization. Switching driver or network owner must preserve the
ELS decorator. A deleted/recreated vehicle should receive a new ELS identity.
The controlling driver asks the server to recover or allocate that identity and
waits for existing state before initializing an unfamiliar car. Visible vehicles
are reconciled every 250 ms; `ElsRegistrationDelay` controls only owner state
refreshes. Client scope loss releases local effects while retaining cached state;
the server's entity deletion event clears the identity and state on actual deletion.
Native GTA siren-light state is included in snapshots.

The administrative commands now require their respective ACE permissions. For
example, grant your administrator group:

```cfg
add_ace group.admin command.vcfrefresh allow
add_ace group.admin command.resync allow
add_ace group.admin command.clearcache allow
```

`ElsCarAdminOnly` continues to control `/elscar`. `/vcfrefresh` now reloads the
VCFs and custom patterns before sending them to connected clients.

Compile checks can run on macOS or Linux with Python 3.9+, the .NET SDK, NuGet
access, and the repository's existing referenced DLLs:

```sh
python3 utils/check-build.py
dotnet run --project tests/RegressionChecks
dotnet run --project tests/StreamingChecks
```

The compile helper reads the existing project source lists and creates temporary
.NET Framework 4.5.2 compilation projects. It skips the Windows PowerShell
versioning/deployment hooks and prints the location of its logs. Its output is
not a packaged resource. Regression checks require .NET 10 and execute the real
sync validation, server handlers, tone playback logic, and light serialization
with simulated CitizenFX calls. They do not verify GTA rendering, audio, or
network replication. The streaming suite executes the production vehicle manager
and list against simulated entity creation/removal, handle and network-ID reuse,
delayed decorators, driver handoffs, and state-before-clone delivery.

Streaming behavior was cross-checked against the supplied GTA sources
(`NetObjDynamicEntity.cpp`, `DynamicEntitySyncNodes.cpp`) and FiveM commit
`e34d12cd9a39cc223548a5be1ab09f60e9183051` (`CloneManager.cpp`,
`ServerGameState.cpp`, `EventHandlerDictionary.cs`). GTA replicates decorators
with entity state; FiveM distinguishes a client's scope loss from entity deletion.
These source checks establish the intended lifecycle, not an in-game test result.

Before deploying to a live server, test with two clients:

1. Fresh server start with an empty ELS cache: spawn an ELS car and check controls.
2. Vehicles with no arrowboard, no scene lights, extra-based scene lights, and
   secondary-only or warning-only lights: verify registration and all stages.
3. Driver and front passenger: check horn/siren start, stop, tone changes, and
   immediate exit behavior; rear passengers should not control these functions.
4. Swap drivers and network owners; stream the car out and back in on the other
   client. Confirm its ELS identity and light/siren state survive.
5. Join late with lights/siren already active; verify the initial snapshot.
6. Delete a vehicle, spawn replacements, and confirm old state does not attach to
   a new vehicle when network IDs are reused.
7. Restart ELS, run `/clearcache` and `/resync`, refresh VCFs, and switch/reload the
   UI. Confirm recovery, no stuck sounds, and the correct current UI state.
8. From a non-administrator, verify admin commands are denied. In a development
   harness, send malformed sync and updates for another player's vehicle; verify
   no state is relayed.

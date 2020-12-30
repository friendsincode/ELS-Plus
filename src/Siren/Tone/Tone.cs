using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;

namespace ELS.Siren
{
    public enum ToneType
    {
        Horn,
        SrnTon1,
        SrnTon2,
        SrnTon3,
        SrnTon4,
        SrnPnic
    }
    class Tone
    {

        private readonly string _file;
        private int soundId;
        private Entity _entity;
        private readonly ToneType _type;
        internal string Type;
        internal bool _state { private set; get; }
        internal bool AllowUse { get; set; }
        internal string DLCSoundSet { get; set; }
        internal Tone(string file, Entity entity, ToneType type, bool allow, string soundbank, string soundset, bool state = false)
        {
            _entity = entity;
            _file = file;
            _type = type;
            SetState(state);
            AllowUse = allow;
            switch (type)
            {
                case ToneType.SrnTon1:
                    Type = "WL";
                    break;
                case ToneType.SrnTon2:
                    Type = "YP";
                    break;
                case ToneType.SrnTon3:
                    Type = "A1";
                    break;
                case ToneType.SrnTon4:
                    Type = "A2";
                    break;
            }
            soundId = -1;
            if (!String.IsNullOrEmpty(soundbank) && !Global.RegisterdSoundBanks.Contains(soundbank))
            {
                Utils.DebugWriteLine($"Registering sound bank {soundset}");
                API.RequestScriptAudioBank(soundbank, false);
                Global.RegisterdSoundBanks.Add(soundbank);
            }
            if (!String.IsNullOrEmpty(soundset))
            {
                Utils.DebugWriteLine($"Adding soundset {soundset} for {_file}");
                DLCSoundSet = soundset;
            }
        }

        internal void SetState(bool state)
        {
            Utils.DebugWriteLine($"Setting state to {state} for {Type}");
            _state = state;
            if (_state && AllowUse)
            {
                Utils.DebugWriteLine($"Getting ready to start sound for {Type}");
                if (soundId == -1)
                {
                    //soundId = Audio.PlaySoundFromEntity(_entity, _file);
                    soundId = API.GetSoundId();
                    Utils.DebugWriteLine($"1. Sound id of {soundId} with networkid of {_entity.GetNetworkId()} with network sound id of {API.GetSoundIdFromNetworkId(_entity.GetNetworkId())}");
                    if (!Audio.HasSoundFinished(soundId)) return;
                    if (!String.IsNullOrEmpty(DLCSoundSet))
                    {
                        Utils.DebugWriteLine($"DLC enabled sound using file  {_file}");
                        API.PlaySoundFromEntity(soundId, _file, _entity.Handle, DLCSoundSet, false, 0);
                        //Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)Global.DLCSoundSet, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
                    }
                    else
                    {
                        Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
                        Utils.DebugWriteLine($"DLC disabled sound using file  {_file}");
                        //API.PlaySoundFromEntity(soundId, _file, _entity.Handle, "0", false, 0);
                    }
                    Utils.DebugWriteLine($"2. Sound id of {soundId} with networkid of {_entity.GetNetworkId()} with network sound id of {API.GetSoundIdFromNetworkId(_entity.GetNetworkId())}");
                    //API.PlaySoundFromEntity(soundId, _file, _entity.Handle, "0", false, 0);
                    Utils.DebugWriteLine($"Started sound with id of {soundId}");
                } 
                else
                {
                    if (!String.IsNullOrEmpty(DLCSoundSet))
                    {
                        Utils.DebugWriteLine($"DLC enabled sound using file  {_file}");
                        API.PlaySoundFromEntity(soundId, _file, _entity.Handle, DLCSoundSet, false, 0);
                    }
                    else
                    {
                        //Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
                        Utils.DebugWriteLine($"DLC disabled sound using file  {_file}");
                        API.PlaySoundFromEntity(soundId, _file, _entity.Handle, "0", false, 0);
                    }
                }
            }
            else
            {
                Utils.DebugWriteLine($"Stopping sound {soundId} and setting to -1 for {Type} with networkid of {_entity.GetNetworkId()} and network sound if of {API.GetNetworkIdFromSoundId(soundId)}");
                Audio.StopSound(soundId);
                Audio.ReleaseSound(soundId);
                Utils.DebugWriteLine($"Stopped and released sound with id of {soundId}");
                soundId = -1;
            }
        }

        internal void CleanUp()
        {

            Utils.DebugWriteLine("Tone deconstructor ran");
            Audio.StopSound(soundId);
            Audio.ReleaseSound(soundId);
        }
    }
}

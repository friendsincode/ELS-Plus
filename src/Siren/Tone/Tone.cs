using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using System;

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
        private int soundId = -1;
        private Vehicle _entity;
        private readonly ToneType _type;
        internal string Type;
        internal bool _state { private set; get; }
        internal bool AllowUse { get; set; }
        internal string DLCSoundSet { get; set; }
        internal int elsid { get; set; }
        internal Tone(string file, Vehicle entity, ToneType type, bool allow, string soundbank, string soundset, bool state = false)
        {
            _entity = entity;
            elsid = entity.GetElsId();
            _file = file;
            _type = type;
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
            if (!String.IsNullOrEmpty(soundbank) && !Global.RegisteredSoundBanks.Contains(soundbank))
            {
                Utils.DebugWriteLine($"Registering sound bank {soundset}");
                API.RequestScriptAudioBank(soundbank, false);
                Global.RegisteredSoundBanks.Add(soundbank);
            }
            if (!String.IsNullOrEmpty(soundset))
            {
                Utils.DebugWriteLine($"Adding soundset {soundset} for {_file}");
                DLCSoundSet = soundset;
            }
            SetState(state);
        }

        internal void SetState(bool state)
        {
            _state = state && AllowUse && _entity.Exists();
            if (!_state)
            {
                CleanUp();
                return;
            }
            if (soundId != -1 && !Audio.HasSoundFinished(soundId)) return;
            if (soundId != -1) Audio.ReleaseSound(soundId);
            soundId = API.GetSoundId();
            API.PlaySoundFromEntity(soundId, _file, _entity.Handle,
                String.IsNullOrEmpty(DLCSoundSet) ? null : DLCSoundSet, false, 0);
        }

        internal void CleanUp()
        {
            if (!_state && soundId != -1)
            {
                Audio.StopSound(soundId);
                Audio.ReleaseSound(soundId);
                soundId = -1;
            }
        }
    }
}

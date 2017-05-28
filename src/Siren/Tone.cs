using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.Siren
{
    public enum ToneType
    {
        Horn,
        SrnTon1,
        SrnTon2,
        SrnTon3,
        SrnTon4
    }
    class Tone
    {
        
        private readonly string _file;
        private int soundId = Function.Call<int>(Hash.GET_SOUND_ID);
        private Entity _entity;
        private readonly ToneType _type;
        internal bool _state { private set;  get; }
        internal Tone(string file, Entity entity,ToneType type,bool state =false)
        {
            _entity = entity;
            _file = file;
            _type = type;
            SetState(state);
        }

        internal void SetState( bool state)
        {
            _state = state;
            if (_state)
            {
                Debug.WriteLine("file:" + _file);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
            }
            else
            {
                Audio.StopSound(soundId);
            }
        }
        
        internal void CleanUp()
        {
            Audio.StopSound(soundId);
            Audio.ReleaseSound(soundId);
        }
    }
}

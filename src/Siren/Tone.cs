using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.Siren.Tones
{
    public enum ToneType
    {
        Horn,
        SrnTon1,
        SrnTon2,
        SrnTon3,
        SrnTon4
    }
    public class Tone
    {
        
        private readonly string _file;
        private int soundId = Function.Call<int>(Hash.GET_SOUND_ID);
        private Entity _entity;
        private readonly ToneType _type;
        public bool _state { private set;  get; }
        public Tone(string file, Entity entity,ToneType type,bool state =false)
        {
            _entity = entity;
            _file = file;
            _type = type;
            SetState(state);
        }

        public void SetState( bool state)
        {
            _state = state;
            if (_state)
            {
                Debug.WriteLine("file:" + _file);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
                SendMessage();
            }
            else
            {
                Audio.StopSound(soundId);
                SendMessage();
            }
        }
        public void SetRemoteState(bool state)
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
        private void SendMessage()
        {
            RemoteEventManager.SendEvent(RemoteEventManager.MessageTypes.SirenUpdate,(Vehicle) _entity, _type.ToString() , _state);
        }
        public void CleanUp()
        {
            Audio.StopSound(soundId);
            Audio.ReleaseSound(soundId);
        }
    }
}

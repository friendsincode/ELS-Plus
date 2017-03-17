using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.Siren.Tones
{
    public class Tone
    {
        
        private readonly string _file;
        private int soundId = Function.Call<int>(Hash.GET_SOUND_ID);
        private Entity _entity;
        public Tone(string file, Entity entity)
        {
            _entity = entity;
            _file = file;
        }

        public Tone(string file, Entity entity,bool state)
        {
            _entity = entity;
            _file = file;
            SetState(state);
        }

        public void SetState( bool state)
        {

            if (state)
            {
                Debug.WriteLine("file:" + _file);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
            }
            else
            {
                Audio.StopSound(soundId);
            }
        }
        public void CleanUp()
        {
            Audio.StopSound(soundId);
            Audio.ReleaseSound(soundId);
        }
    }
}

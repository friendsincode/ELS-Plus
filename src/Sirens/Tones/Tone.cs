using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.Sirens.Tones
{
    class Tone
    {
        private readonly string _file;
        private int soundId = Function.Call<int>(Hash.GET_SOUND_ID);
        private Entity _entity;

        public Tone(string file, Entity entity)
        {
            _entity = entity;
            _file = file;
        }

        public void SetState( bool state)
        {
            if (state)
            {
                Debug.WriteLine("file:" + _file);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, (InputArgument)_file, (InputArgument)_entity.Handle, (InputArgument)0, (InputArgument)0, (InputArgument)0);
                //int netSoundid = Function.Call<int>(Hash.GET_NETWORK_ID_FROM_SOUND_ID, (InputArgument)soundId);
                //var vehNetId = Function.Call<int>(Hash.NETWORK_GET_NETWORK_ID_FROM_ENTITY, (InputArgument)soundId);
                //Debug.WriteLine(vehNetId.ToString());
                //Function.Call(Hash.DECOR_REGISTER, _sirenTypes.ToString(), 2);
                //Function.Call<bool>(Hash.DECOR_SET_BOOL, _entity, soundId, true);
                //BaseScript.TriggerServerEvent("sirenStateChanged", vehNetId, netSoundid, File, true);
            }
            else
            {
                //int netSoundid = Function.Call<int>(Hash.GET_NETWORK_ID_FROM_SOUND_ID, (InputArgument)soundId);
                //var vehNetId = Function.Call<int>(Hash.NETWORK_GET_NETWORK_ID_FROM_ENTITY, (InputArgument)soundId);
                Audio.StopSound(soundId);
                //BaseScript.TriggerServerEvent("sirenStateChanged", vehNetId, netSoundid, File, false);

            }
        }
    }
}

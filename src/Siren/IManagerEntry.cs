using CitizenFX.Core;

namespace ELS.Siren
{
    internal interface IManagerEntry 
    {
        void CleanUP();
        void ticker();
        void SirenControlsRemote(string sirenString, bool state);
        Vehicle _vehicle { get; set; }
    }
}
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core.UI;
using ELS.configuration;

namespace ELS.Siren
{
    public delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    partial class Siren : IManagerEntry
    {
        private bool dual_siren;
        private configuration.ControlConfiguration.ELSControls keyBinding;
        public Vehicle _vehicle { get; set; }
        private MainSiren _mainSiren;
        private VCF.vcfroot _vcf;

        configuration.ControlConfiguration.ELSControls keybindings =
            new configuration.ControlConfiguration.ELSControls();

        Tones _tones;

        public Siren(Vehicle vehicle)
        {
            _vehicle = vehicle;
            foreach (VCF.vcfroot vcfroot in VCF.ELSVehicle)
            {
                if (vcfroot.FileName.ToUpper() == _vehicle.DisplayName.ToUpper())
                {
                    _vcf = vcfroot;
                }
            }
            if (_vcf == null)
            {
                Debug.WriteLine(
                    $"Their is no VCF file for this vehicle: {_vehicle.DisplayName} defaulting to POLICE.xml");
                foreach (var vcfroot in VCF.ELSVehicle)
                {
                    if (vcfroot.FileName.ToUpper() == "POLICE")
                    {
                        _vcf = vcfroot;
                    }
                }
            }
            if (_vcf == null)
            {
                Debug.WriteLine("failed");
                return;
            }

            _tones = new Tones
            {
                horn = new Tone(_vcf.SOUNDS.MainHorn.AudioString, _vehicle, ToneType.Horn),
                tone1 = new Tone(_vcf.SOUNDS.SrnTone1.AudioString, _vehicle, ToneType.SrnTon1),
                tone2 = new Tone(_vcf.SOUNDS.SrnTone2.AudioString, _vehicle, ToneType.SrnTon2),
                tone3 = new Tone(_vcf.SOUNDS.SrnTone3.AudioString, _vehicle, ToneType.SrnTon3),
                tone4 = new Tone(_vcf.SOUNDS.SrnTone4.AudioString, _vehicle, ToneType.SrnTon4)
            };
            dual_siren = false;
            _mainSiren = new MainSiren(_tones);

        }

        public void CleanUP()
        {
            _tones.horn.CleanUp();
            _tones.tone1.CleanUp();
            _tones.tone2.CleanUp();
            _tones.tone3.CleanUp();
            _tones.tone4.CleanUp();

        }

        public void ticker()
        {
            Game.DisableControlThisFrame(0, Control.VehicleHorn);
            if (Game.IsControlJustReleased(0, Control.VehicleHorn))
            {
                Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, _vehicle, true);
                _vehicle.IsSirenActive = !_vehicle.IsSirenActive;
            }

            AirHornControlsProccess();
            ManualTone1ControlsProccess();
            ManualTone2ControlsProccess();
            ManualTone3ControlsProccess();
            ManualTone4ControlsProccess();
            ManualSoundControlsProccess();
            MainSirenToggleControlsProccess();
            
        }
    }
}
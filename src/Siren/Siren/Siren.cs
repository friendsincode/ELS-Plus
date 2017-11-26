using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core.UI;
using ELS.configuration;

namespace ELS.Siren
{
    internal delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    partial class Siren : IManagerEntry
    {
        private bool dual_siren;
        public Vehicle _vehicle { get; set; }
        private MainSiren _mainSiren;
        private VCF.vcfroot _vcf;
        Tones _tones;
        public Siren(Vehicle vehicle)
        {
            _vehicle = vehicle;
            Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, _vehicle, true);

            _vcf = VCF.ELSVehicle.Find(item => item.Item2.FileName == _vehicle.DisplayName).Item2;
            if (_vcf == null)
            {
                throw new Exception($"Their is no VCF file for this vehicle: {_vehicle.DisplayName}");
            }
            _tones = new Tones
            {
                horn = new Tone(_vcf.SOUNDS.MainHorn.AudioString, _vehicle, ToneType.Horn),
                tone1 = new Tone(_vcf.SOUNDS.SrnTone1.AudioString, _vehicle, ToneType.SrnTon1),
                tone2 = new Tone(_vcf.SOUNDS.SrnTone2.AudioString, _vehicle, ToneType.SrnTon2),
                tone3 = new Tone(_vcf.SOUNDS.SrnTone3.AudioString, _vehicle, ToneType.SrnTon3),
                tone4 = new Tone(_vcf.SOUNDS.SrnTone4.AudioString, _vehicle, ToneType.SrnTon4),
                panicAlarm = new Tone(_vcf.SOUNDS.PanicMde.AudioString, _vehicle, ToneType.SrnPnic)
            };
            dual_siren = false;
            _mainSiren = new MainSiren(ref _tones);
            RequestFullSyncData();
        }

        public void CleanUP()
        {
            _tones.horn.CleanUp();
            _tones.tone1.CleanUp();
            _tones.tone2.CleanUp();
            _tones.tone3.CleanUp();
            _tones.tone4.CleanUp();
            _tones.panicAlarm.CleanUp();
        }
    }
}
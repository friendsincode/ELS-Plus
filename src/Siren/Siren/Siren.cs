using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core.UI;
using ELS.configuration;
using System.Collections;
using System.Collections.Generic;
using ELS.NUI;

namespace ELS.Siren
{
    internal delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    partial class Siren : IManagerEntry
    {
        private Vcfroot _vcf;
        private bool dual_siren;
        public Vehicle _vehicle { get; set; }
        private MainSiren _mainSiren;
        Tones _tones;
        public Siren(Vehicle vehicle,Vcfroot vcfroot,[Optional]IDictionary<string,object> data)
        {
            _vcf = vcfroot;
            _vehicle = vehicle;
            
            Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, _vehicle, true);
#if DEBUG
            CitizenFX.Core.Debug.WriteLine(_vehicle.DisplayName);
#endif

            _tones = new Tones
            {
                horn = new Tone(vcfroot.SOUNDS.MainHorn.AudioString, _vehicle, ToneType.Horn),
                tone1 = new Tone(vcfroot.SOUNDS.SrnTone1.AudioString, _vehicle, ToneType.SrnTon1),
                tone2 = new Tone(vcfroot.SOUNDS.SrnTone2.AudioString, _vehicle, ToneType.SrnTon2),
                tone3 = new Tone(vcfroot.SOUNDS.SrnTone3.AudioString, _vehicle, ToneType.SrnTon3),
                tone4 = new Tone(vcfroot.SOUNDS.SrnTone4.AudioString, _vehicle, ToneType.SrnTon4),
                panicAlarm = new Tone(vcfroot.SOUNDS.PanicMde.AudioString, _vehicle, ToneType.SrnPnic)
            };

            dual_siren = false;

            _mainSiren = new MainSiren(ref _tones);

            if (data != null) SetData(data);
            ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
            ElsUiPanel.SetUiDesc("--", "HRN");
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

        internal void SyncUi()
        {
            
        }
    }
}
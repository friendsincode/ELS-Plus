using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.Light;
using ELS.NUI;
using System.Runtime.InteropServices;


namespace ELS.Siren
{
    internal delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    partial class Siren : IManagerEntry
    {
        private Vcfroot _vcf;
        internal bool dual_siren;
        public Vehicle _vehicle { get; set; }
        public MainSiren _mainSiren;
        IPatterns _patternController;
        internal Tones _tones;
        public Siren(Vehicle vehicle, Vcfroot vcfroot,  IPatterns patt, [Optional] SirenFSData data)
        {
            _vcf = vcfroot;
            _vehicle = vehicle;
            _patternController = patt;
            Function.Call(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, _vehicle, true);
            Utils.DebugWriteLine(_vehicle.DisplayName);

            _tones = new Tones
            {
                horn = new Tone(vcfroot.SOUNDS.MainHorn.AudioString, _vehicle, ToneType.Horn, true, vcfroot.SOUNDS.MainHorn.SoundBank, vcfroot.SOUNDS.MainHorn.SoundSet),
                tone1 = new Tone(vcfroot.SOUNDS.SrnTone1.AudioString, _vehicle, ToneType.SrnTon1, vcfroot.SOUNDS.SrnTone1.AllowUse, vcfroot.SOUNDS.SrnTone1.SoundBank, vcfroot.SOUNDS.SrnTone1.SoundSet),
                tone2 = new Tone(vcfroot.SOUNDS.SrnTone2.AudioString, _vehicle, ToneType.SrnTon2, vcfroot.SOUNDS.SrnTone2.AllowUse, vcfroot.SOUNDS.SrnTone2.SoundBank, vcfroot.SOUNDS.SrnTone2.SoundSet),
                tone3 = new Tone(vcfroot.SOUNDS.SrnTone3.AudioString, _vehicle, ToneType.SrnTon3, vcfroot.SOUNDS.SrnTone3.AllowUse, vcfroot.SOUNDS.SrnTone3.SoundBank, vcfroot.SOUNDS.SrnTone3.SoundSet),
                tone4 = new Tone(vcfroot.SOUNDS.SrnTone4.AudioString, _vehicle, ToneType.SrnTon4, vcfroot.SOUNDS.SrnTone4.AllowUse, vcfroot.SOUNDS.SrnTone4.SoundBank, vcfroot.SOUNDS.SrnTone4.SoundSet),
                panicAlarm = new Tone(vcfroot.SOUNDS.PanicMde.AudioString, _vehicle, ToneType.SrnPnic, vcfroot.SOUNDS.PanicMde.AllowUse, vcfroot.SOUNDS.PanicMde.SoundBank, vcfroot.SOUNDS.PanicMde.SoundSet)
            };

            dual_siren = false;

            _mainSiren = new MainSiren(ref _tones);

            if (!data.Equals(null)) SetData(data);
            ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
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
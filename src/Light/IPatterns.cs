namespace ELS.Light
{
    internal interface IPatterns
    {
        int CurrentPrmPattern { get; set; }
        int CurrentSecPattern { get; set; }
        int CurrentWrnPattern { get; set; }
        int CurrentStage { get; }
    }
}

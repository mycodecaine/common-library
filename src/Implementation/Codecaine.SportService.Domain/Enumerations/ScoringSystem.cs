using System.Text.Json.Serialization;

namespace Codecaine.SportService.Domain.Enumerations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ScoringSystem
    {
        PointsBased,        // Earn points for actions (e.g., Basketball, Football)
        TimeBased,          // Fastest time wins (e.g., Sprinting, Swimming)
        SetBased,           // Win sets or games (e.g., Tennis, Volleyball)
        JudgingBased,       // Judges assign scores (e.g., Gymnastics, Diving)
        GoalOriented,       // Score goals (e.g., Soccer, Hockey)
        RoundsBased,        // Win rounds to win match (e.g., Boxing, MMA)
        AggregateBased,     // Total score over matches (e.g., Golf, Two-leg Football)
        EliminationBased    // Eliminate others to win (e.g., Martial Arts, eSports)
    }

}

using Codecaine.Common.Domain;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.ValueObjects
{
    public sealed class SportRule: ValueObject
    {
        public ScoringSystem ScoringSystem { get; }
        public int PlayerCount { get; }
        public int? Duration { get; }
        public int? MaxScore { get; }

        public SportRule() { }

        public SportRule(ScoringSystem scoringSystem, int playerCount, int? duration, int? maxScore)
        {
            ScoringSystem = scoringSystem;
            PlayerCount = playerCount;
            Duration = duration;
            MaxScore = maxScore;
        }

        public static Result<SportRule> Create(ScoringSystem scoringSystem, int playerCount, int? duration, int? maxScore)
        {
            if (playerCount < 1)
                return Result.Failure<SportRule>(new Common.Primitives.Errors.Error("",""));
            if (duration.HasValue && duration <= 0)
                return Result.Failure<SportRule>(new Common.Primitives.Errors.Error("DurationMustBeGreater", "Duration must be greater than 0.")); 
            if (maxScore.HasValue && maxScore <= 0)
                return Result.Failure<SportRule>(new Common.Primitives.Errors.Error("", "Max score must be greater than 0."));
            return Result.Success(new SportRule(scoringSystem, playerCount, duration, maxScore));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ScoringSystem;
            yield return PlayerCount;
            yield return Duration ?? 0;
            yield return MaxScore ?? 0;

        }
    }
}

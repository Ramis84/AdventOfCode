using MoreLinq;

namespace AdventOfCode2023.Logic.Assignments;

public static class Day5
{
    private static readonly string[] Lines = File.ReadAllLines("input_20231205.txt");

    public static string GetPart1Answer()
    {
        var problem = new Problem(Lines);
        var seeds = problem.GetSeedsAsSingleSeeds();
        var locations = problem.LookupLocationsFromSeeds(seeds);
        var minLocation = locations.Ranges.Select(x => x.From).Min();
        return minLocation.ToString();
    }

    public static string GetPart2Answer()
    {
        var problem = new Problem(Lines);
        var seeds = problem.GetSeedsAsRanges();
        var locations = problem.LookupLocationsFromSeeds(seeds);
        var minLocation = locations.Ranges.Select(x => x.From).Min();
        return minLocation.ToString();
    }

    private class Problem
    {
        private readonly RangeMappings _seedToSoil;
        private readonly RangeMappings _soilToFertilizer;
        private readonly RangeMappings _fertilizerToWater;
        private readonly RangeMappings _waterToLight;
        private readonly RangeMappings _lightToTemperature;
        private readonly RangeMappings _temperatureToHumidity;
        private readonly RangeMappings _humidityToLocation;

        private readonly long[] _seedRawValues;

        public Problem(string[] lines)
        {
            var lineGroups = lines
                .Segment(string.IsNullOrWhiteSpace)
                .Select(x => x.SkipWhile(string.IsNullOrWhiteSpace).ToArray())
                .ToArray();

            _seedRawValues = lineGroups[0][0].Split(' ')[1..].Select(long.Parse).ToArray();

            _seedToSoil = new RangeMappings(lineGroups[1]);
            _soilToFertilizer = new RangeMappings(lineGroups[2]);
            _fertilizerToWater = new RangeMappings(lineGroups[3]);
            _waterToLight = new RangeMappings(lineGroups[4]);
            _lightToTemperature = new RangeMappings(lineGroups[5]);
            _temperatureToHumidity = new RangeMappings(lineGroups[6]);
            _humidityToLocation = new RangeMappings(lineGroups[7]);
        }

        public RangeSet GetSeedsAsSingleSeeds()
        {
            var asRanges = _seedRawValues.Select(x => new RangeInclusive{From = x, To = x});
            return new RangeSet(asRanges);
        }

        public RangeSet GetSeedsAsRanges()
        {
            var seedRanges = new List<RangeInclusive>();
            for (int i = 0; i < _seedRawValues.Length; i += 2)
            {
                var rangeStart = _seedRawValues[i];
                var length = _seedRawValues[i+1];
                seedRanges.Add(new RangeInclusive
                {
                    From = rangeStart,
                    To = rangeStart + length - 1
                });
            }
            return new RangeSet(seedRanges);
        }

        public RangeSet LookupLocationsFromSeeds(RangeSet seeds)
        {
            var soils = _seedToSoil.Lookup(seeds);
            var fertilizers = _soilToFertilizer.Lookup(soils);
            var waters = _fertilizerToWater.Lookup(fertilizers);
            var lights = _waterToLight.Lookup(waters);
            var temperatures = _lightToTemperature.Lookup(lights);
            var humidities = _temperatureToHumidity.Lookup(temperatures);
            var locations = _humidityToLocation.Lookup(humidities);
            return locations;
        }
    }

    private class RangeMappings
    {
        private readonly List<RangeMapping> _mappings;

        public RangeMappings(string[] lines)
        {
            _mappings = lines[1..]
                .Select(x => new RangeMapping(x))
                .ToList();
        }

        public RangeSet Lookup(RangeSet sourceRanges)
        {
            var targetRanges = new List<RangeInclusive>();
            var remainingSourceRanges = sourceRanges.Ranges.ToList();
            foreach (var mapping in _mappings)
            {
                var leftoverSourceRanges = new List<RangeInclusive>();
                var sourceRangesToCheck = new Queue<RangeInclusive>(remainingSourceRanges);
                while (sourceRangesToCheck.Count > 0)
                {
                    var sourceRange = sourceRangesToCheck.Dequeue();
                    var sourceToTarget = mapping.Lookup(sourceRange);
                    if (sourceToTarget != null)
                    {
                        targetRanges.Add(sourceToTarget.Value.Target);
                        var remainingSources = sourceRange.Except(sourceToTarget.Value.Source);
                        foreach (var remainingSource in remainingSources)
                        {
                            sourceRangesToCheck.Enqueue(remainingSource);
                        }
                    }
                    else
                    {
                        leftoverSourceRanges.Add(sourceRange);
                    }
                }

                remainingSourceRanges = leftoverSourceRanges;
            }

            targetRanges.AddRange(remainingSourceRanges); // Default same
            return new RangeSet(targetRanges);
        }
    }

    private class RangeMapping
    {
        private readonly RangeInclusive _sourceRange;
        private readonly long _destinationRangeStart;

        public RangeMapping(string line)
        {
            var segments = line.Split(' ');
            _destinationRangeStart = long.Parse(segments[0]);

            var sourceRangeStart = long.Parse(segments[1]);
            var length = long.Parse(segments[2]);
            _sourceRange = new RangeInclusive
            {
                From = sourceRangeStart,
                To = sourceRangeStart + length - 1
            };
        }

        public (RangeInclusive Source, RangeInclusive Target)? Lookup(RangeInclusive other)
        {
            var overlap = _sourceRange.GetOverlap(other);
            if (overlap == null) return null;

            var offset = _destinationRangeStart - _sourceRange.From;
            return (
                Source: overlap.Value,
                Target: new RangeInclusive
                {
                    From = overlap.Value.From + offset,
                    To = overlap.Value.To + offset
                });
        }
    }

    private readonly struct RangeInclusive
    {
        public long From { get; init; }
        public long To { get; init; }

        public RangeInclusive? GetOverlap(RangeInclusive other)
        {
            var maxFrom = long.Max(From, other.From);
            var minTo = long.Min(To, other.To);

            return maxFrom <= minTo ? new RangeInclusive { From = maxFrom, To = minTo } : null;
        }

        public IEnumerable<RangeInclusive> Except(RangeInclusive removed)
        {
            if (removed.From > From && removed.From <= To)
            {
                yield return this with { To = removed.From - 1 };
            }

            if (removed.To >= From && removed.To < To)
            {
                yield return this with { From = removed.To+1 };
            }
        }
    }

    private readonly struct RangeSet(IEnumerable<RangeInclusive> ranges)
    {
        public RangeInclusive[] Ranges { get; } = ranges.ToArray();
    }
}
using AdventOfCode2024.Logic;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class AdventOfCode2024Tests
{
    [Test]
    public void Day1Part1Test()
    {
        Day1.GetPart1Answer().Should().Be("1222801");
    }

    [Test]
    public void Day1Part2Test()
    {
        Day1.GetPart2Answer().Should().Be("22545250");
    }
}
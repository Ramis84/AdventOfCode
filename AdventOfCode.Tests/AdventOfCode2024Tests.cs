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
    
    [Test]
    public void Day2Part1Test()
    {
        Day2.GetPart1Answer().Should().Be("524");
    }

    [Test]
    public void Day2Part2Test()
    {
        Day2.GetPart2Answer().Should().Be("569");
    }
    
    [Test]
    public void Day3Part1Test()
    {
        Day3.GetPart1Answer().Should().Be("166630675");
    }

    [Test]
    public void Day3Part2Test()
    {
        Day3.GetPart2Answer().Should().Be("93465710");
    }
    
    [Test]
    public void Day4Part1Test()
    {
        Day4.GetPart1Answer().Should().Be("2504");
    }

    [Test]
    public void Day4Part2Test()
    {
        Day4.GetPart2Answer().Should().Be("1923");
    }
}
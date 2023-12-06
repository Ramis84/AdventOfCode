using AdventOfCode2023.Logic;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class AdventOfCode2023Tests
{
    [Test]
    public void Day1Part1Test()
    {
        Day1.GetPart1Answer().Should().Be("55208");
    }

    [Test]
    public void Day1Part2Test()
    {
        Day1.GetPart2Answer().Should().Be("54578");
    }

    [Test]
    public void Day2Part1Test()
    {
        Day2.GetPart1Answer().Should().Be("2512");
    }

    [Test]
    public void Day2Part2Test()
    {
        Day2.GetPart2Answer().Should().Be("67335");
    }

    [Test]
    public void Day3Part1Test()
    {
        Day3.GetPart1Answer().Should().Be("521515");
    }

    [Test]
    public void Day3Part2Test()
    {
        Day3.GetPart2Answer().Should().Be("69527306");
    }

    [Test]
    public void Day4Part1Test()
    {
        Day4.GetPart1Answer().Should().Be("18519");
    }

    [Test]
    public void Day4Part2Test()
    {
        Day4.GetPart2Answer().Should().Be("11787590");
    }

    [Test]
    public void Day5Part1Test()
    {
        Day5.GetPart1Answer().Should().Be("1181555926");
    }

    [Test]
    public void Day5Part2Test()
    {
        Day5.GetPart2Answer().Should().Be("37806486");
    }

    [Test]
    public void Day6Part1Test()
    {
        Day6.GetPart1Answer().Should().Be("281600");
    }

    [Test]
    public void Day6Part2Test()
    {
        Day6.GetPart2Answer().Should().Be("33875953");
    }
}
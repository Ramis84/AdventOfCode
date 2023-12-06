using AdventOfCode2022.Logic;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class AdventOfCode2022Tests
{
    [Test]
    public void Day1Part1Test()
    {
        Day1.GetPart1Answer().Should().Be("70369");
    }

    [Test]
    public void Day1Part2Test()
    {
        Day1.GetPart2Answer().Should().Be("203002");
    }

    [Test]
    public void Day2Part1Test()
    {
        Day2.GetPart1Answer().Should().Be("12535");
    }

    [Test]
    public void Day2Part2Test()
    {
        Day2.GetPart2Answer().Should().Be("15457");
    }

    [Test]
    public void Day3Part1Test()
    {
        Day3.GetPart1Answer().Should().Be("8240");
    }

    [Test]
    public void Day3Part2Test()
    {
        Day3.GetPart2Answer().Should().Be("2587");
    }

    [Test]
    public void Day4Part1Test()
    {
        Day4.GetPart1Answer().Should().Be("507");
    }

    [Test]
    public void Day4Part2Test()
    {
        Day4.GetPart2Answer().Should().Be("897");
    }

    [Test]
    public void Day5Part1Test()
    {
        Day5.GetPart1Answer().Should().Be("LJSVLTWQM");
    }

    [Test]
    public void Day5Part2Test()
    {
        Day5.GetPart2Answer().Should().Be("BRQWDBBJM");
    }

    [Test]
    public void Day6Part1Test()
    {
        Day6.GetPart1Answer().Should().Be("1582");
    }

    [Test]
    public void Day6Part2Test()
    {
        Day6.GetPart2Answer().Should().Be("3588");
    }

    [Test]
    public void Day7Part1Test()
    {
        Day7.GetPart1Answer().Should().Be("1667443");
    }

    [Test]
    public void Day7Part2Test()
    {
        Day7.GetPart2Answer().Should().Be("8998590");
    }

    [Test]
    public void Day8Part1Test()
    {
        Day8.GetPart1Answer().Should().Be("1809");
    }

    [Test]
    public void Day8Part2Test()
    {
        Day8.GetPart2Answer().Should().Be("479400");
    }

    [Test]
    public void Day9Part1Test()
    {
        Day9.GetPart1Answer().Should().Be("5902");
    }

    [Test]
    public void Day9Part2Test()
    {
        Day9.GetPart2Answer().Should().Be("2445");
    }

    [Test]
    public void Day10Part1Test()
    {
        Day10.GetPart1Answer().Should().Be("15880");
    }

    [Test]
    public void Day10Part2Test()
    {
        var expected = """
                       ###..#.....##..####.#..#..##..####..##..
                       #..#.#....#..#.#....#.#..#..#....#.#..#.
                       #..#.#....#....###..##...#..#...#..#....
                       ###..#....#.##.#....#.#..####..#...#.##.
                       #....#....#..#.#....#.#..#..#.#....#..#.
                       #....####..###.#....#..#.#..#.####..###.
                       """;
        Day10.GetPart2Answer().Should().Be(expected);
    }

    [Test]
    public void Day11Part1Test()
    {
        Day11.GetPart1Answer().Should().Be("58056");
    }

    [Test]
    public void Day11Part2Test()
    {
        Day11.GetPart2Answer().Should().Be("15048718170");
    }

    [Test]
    public void Day12Part1Test()
    {
        Day12.GetPart1Answer().Should().Be("462");
    }

    [Test]
    public void Day12Part2Test()
    {
        Day12.GetPart2Answer().Should().Be("451");
    }

    [Test]
    public void Day13Part1Test()
    {
        Day13.GetPart1Answer().Should().Be("5852");
    }

    [Test]
    public void Day13Part2Test()
    {
        Day13.GetPart2Answer().Should().Be("24190");
    }
}
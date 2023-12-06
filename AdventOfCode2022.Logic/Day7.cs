using MoreLinq;

namespace AdventOfCode2022.Logic;

public static class Day7
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221207.txt");
    private static readonly State FileSystem = InitializeState(Lines);

    public static string GetPart1Answer()
    {
        var descendantDirectoriesWithSize = FileSystem.Root.GetDescendantDirectories().Select(x => new
        {
            Directory = x,
            Size = x.GetTotalSize()
        });
        var directoriesBelowLimitSize = descendantDirectoriesWithSize
            .Where(x => x.Size <= 100000);
        var sizeSum = directoriesBelowLimitSize.Select(x => x.Size).Sum();

        return sizeSum.ToString();
    }

    public static string GetPart2Answer()
    {
        var totalSpace = 70000000;
        var requiredFreeSpace = 30000000;
        var maximumUsedSpace = totalSpace - requiredFreeSpace;
        var currentUsedSpace = FileSystem.Root.GetTotalSize();
        var minimumRemoveSize = currentUsedSpace - maximumUsedSpace;
        var descendantDirectories = FileSystem.Root.GetDescendantDirectories();
        var minimumSizeDirectoryAboveLimit = descendantDirectories
            .Select(x => new
            {
                Directory = x,
                TotalSize = x.GetTotalSize()
            })
            .Where(x => x.TotalSize >= minimumRemoveSize)
            .MinBy(x => x.TotalSize);
        return minimumSizeDirectoryAboveLimit.TotalSize.ToString();
    }

    private static State InitializeState(string[] lines)
    {
        var commandsAndResults = lines
            .Segment(line => line.StartsWith('$'))
            .Select(commandLines => new CommandAndResults(commandLines.ToArray()));
        var state = new State();

        foreach (var commandsAndResult in commandsAndResults)
        {
            state.Execute(commandsAndResult);
        }

        return state;
    }

    private class State
    {
        public ElfDirectory Root { get; } = new() {Name = "/"};
        public ElfDirectory? CurrentDirectory { get; private set; } = null;

        public void Execute(CommandAndResults commandAndResults)
        {
            if (commandAndResults.Command == "$ cd /")
            {
                CurrentDirectory = Root;
            }
            else if (commandAndResults.Command == "$ ls")
            {
                CurrentDirectory.AddFilesAndDirectories(commandAndResults.Results);
            }
            else if (commandAndResults.Command.StartsWith("$ cd "))
            {
                var directoryName = commandAndResults.Command[5..];
                CurrentDirectory = CurrentDirectory.GetDirectory(directoryName);
            }
        }
    }

    public class CommandAndResults
    {
        public string Command { get; set; }
        public string[] Results { get; set; }

        public CommandAndResults(string[] lines)
        {
            Command = lines[0];
            Results = lines[1..];
        }
    }

    private class ElfDirectory
    {
        public string Name { get; init; }
        public ElfDirectory? Parent { get; init; }

        public List<ElfDirectory> SubDirectories { get; } = new();
        public List<ElfFile> Files { get; } = new();

        public int GetTotalSize()
        {
            return Files.Select(x => x.Size)
                .Concat(SubDirectories.Select(x => x.GetTotalSize()))
                .Sum();
        }

        public IEnumerable<ElfDirectory> GetDescendantDirectories()
        {
            return SubDirectories.Concat(SubDirectories.SelectMany(x => x.GetDescendantDirectories()));
        }

        public void AddFilesAndDirectories(string[] entries)
        {
            foreach (var entry in entries)
            {
                var segments = entry.Split(' ');
                var name = segments[1];
                if (segments[0] == "dir")
                {
                    SubDirectories.Add(new ElfDirectory { Name = name, Parent = this });
                }
                else
                {
                    var size = int.Parse(segments[0]);
                    Files.Add(new ElfFile { Name = name, Size = size });
                }
            }
        }

        public ElfDirectory? GetDirectory(string directoryName)
        {
            return directoryName == ".." 
                ? Parent 
                : SubDirectories.FirstOrDefault(x => x.Name == directoryName);
        }
    }

    private class ElfFile
    {
        public string Name { get; init; }
        public int Size { get; init; }
    }
}
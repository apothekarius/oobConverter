using System.Text.RegularExpressions;
using OobConverter.Enums;
using OobConverter.Models;

const string patternElementary = "^[AKDGRBCPakdgrbcp]\\.\\s";
const string patternComposite = "^([AKDGRBCPakdgrbcp]|AG|ag)\\s";
const string patternCompositeWithNationality = "^[A-Za-z-]*\\s([AKDGRBCPakdgrbcp]|AG)\\s";
const string patternElementaryWithNationality = "^[A-Za-z-]*\\s[AKDGRBCPakdgrbcp]\\.\\s";
const string patternBegin = "Begin";
const string patternEnd = "End";

var lineCounter = 0;
var allFiles = Directory.EnumerateFiles(".", "*.oob");
var allRawResults = new Dictionary<string, RawUnitData>();


foreach (var file in allFiles) {
    try {
        Console.WriteLine($"Begin processing of {file}...");
        using var reader = new StreamReader(file);
        lineCounter = 0;
        var line = FindFirstLineOfUnits(reader);

        if (string.IsNullOrEmpty(line)) {
            Console.WriteLine($"{file} is empty or contains no valid unit data");
            continue;
        }

        var root = new RawUnitData {
            UnitData = "root",
            Subordinates = new List<RawUnitData>()
        };
        var current = root;
        do {
            ProcessLine(line.Trim(), reader, ref current);
            line = NextLine(reader);
        } while (!reader.EndOfStream);

        allRawResults.Add(file, root);
        Console.WriteLine($"Processing of {file} successful");
        Console.WriteLine();
    }
    catch (InvalidDataException e) {
        Console.Error.WriteLine(e.Message);
        Console.Error.WriteLine($"Processing of file {file} aborted");
    }
}

foreach (var pair in allRawResults) {
    var file = pair.Key.Substring(2, pair.Key.Length - 2 - 3);
    var raw = pair.Value;
    Console.WriteLine($"Creating output for {file}");

    var rootConverted = ConvertToCompositeUnit(raw, null);

    Console.WriteLine($"Created {file}.csv");
}

Console.WriteLine("All done.");

CompositeUnit ConvertToCompositeUnit(RawUnitData data, CompositeUnit? parent) {
    var splitDataString = SplitDataString(data.UnitData, !data.HasNationality);

    var result = new CompositeUnit();

    if (!data.IsRoot) {
        result.ParentUnit = parent;
        result.Nationality = data.HasNationality ? splitDataString[0] : parent?.Nationality;
        result.FormationType = Enum.Parse<FormationType>(splitDataString[1].ToUpper());

        result.Name = string.Join(' ', splitDataString.Skip(4)).Trim();
    }
    else {
        result.Name = data.UnitData;
    }


    foreach (var sub in data.Subordinates?.Where(x => x.IsComposite) ?? Array.Empty<RawUnitData>()) {
        result.SubordinateComposites ??= new List<CompositeUnit>();
        result.SubordinateComposites.Add(ConvertToCompositeUnit(sub, result));
    }

    foreach (var sub in data.Subordinates?.Where(x => x.IsElementary) ?? Array.Empty<RawUnitData>()) {
        result.SubordinateElementaries ??= new List<ElementaryUnit>();
        result.SubordinateElementaries.Add(ConvertToElementaryUnit(sub, result));
    }

    return result;
}

ElementaryUnit ConvertToElementaryUnit(RawUnitData data, CompositeUnit parent) {
    var splitDataString = SplitDataString(data.UnitData, !data.HasNationality);
    var result = new ElementaryUnit();
    result.ParentUnit = parent;
    result.Nationality = data.HasNationality ? splitDataString[0] : parent.Nationality;
    result.FormationType = Enum.Parse<FormationType>(splitDataString[1].Remove(splitDataString[1].Length - 1).ToUpper());
    result.UnitType = Enum.Parse<UnitType>(splitDataString[3]);
    result.MovementType = Enum.Parse<MovementType>(splitDataString[4]);
    result.Amount = int.Parse(splitDataString[5]);
    result.Quality = (UnitQuality) int.Parse(splitDataString[6]);
    result.HardAttack = int.Parse(splitDataString[7]);
    result.HardRange = int.Parse(splitDataString[8]);
    result.SoftAttack = int.Parse(splitDataString[10]);
    result.SoftRange = int.Parse(splitDataString[11]);
    result.AntiAirAttack = int.Parse(splitDataString[13]);
    result.AntiAirRange = int.Parse(splitDataString[14]);
    result.Defense = int.Parse(splitDataString[16]);
    result.Assault = int.Parse(splitDataString[17]);
    result.Speed = int.Parse(splitDataString[18]);
    result.Keywords = (Keywords) int.Parse(splitDataString[19]);

    var descAndName = string.Join(' ', splitDataString.Skip(20)).Split(',');
    result.Name = descAndName[0].Trim();
    result.Description = descAndName[1].Trim();
    return result;
}

List<string> SplitDataString(string data, bool prependDummy) {
    var splitDataString = data.Split(' ').ToList();
    if (prependDummy) splitDataString.Insert(0, "DUMMY");

    return splitDataString;
}

string FindFirstLineOfUnits(StreamReader reader) {
    var firstLine = string.Empty;
    while (!reader.EndOfStream) {
        firstLine = NextLine(reader);
        if (!string.IsNullOrEmpty(firstLine) && !Regex.IsMatch(firstLine, "^[0-9]"))
            break;
    }

    return firstLine;
}

void ProcessLine(string line, StreamReader reader, ref RawUnitData currentParent) {
    var dummy = line switch {
        _ when Regex.IsMatch(line, patternCompositeWithNationality) => ProcessLineComposite(line, reader, ref currentParent, true),
        _ when Regex.IsMatch(line, patternComposite) => ProcessLineComposite(line, reader, ref currentParent, false),
        _ when Regex.IsMatch(line, patternElementaryWithNationality) => ProcessLineElementary(line, currentParent, true),
        _ when Regex.IsMatch(line, patternElementary) => ProcessLineElementary(line, currentParent, false),
        _ when Regex.IsMatch(line, patternBegin) => throw new InvalidDataException($"Mismatched Begin statement after composite unit declaration in line {lineCounter}"),
        _ when Regex.IsMatch(line, patternEnd) => ProcessEnd(ref currentParent),
        _ => throw new InvalidDataException($"Malformed content in line {lineCounter}")
    };
}

string NextLine(StreamReader reader) {
    lineCounter++;
    return reader.ReadLine() ?? string.Empty;
}

string ProcessLineComposite(string line, StreamReader reader, ref RawUnitData currentParent, bool hasNationality) {
    if (!Regex.IsMatch(NextLine(reader), patternBegin)) throw new InvalidDataException($"Missing Begin statement after composite unit declaration in line {lineCounter}");

    var newUnit = new RawUnitData {
        Parent = currentParent,
        HasNationality = hasNationality,
        Subordinates = new List<RawUnitData>(),
        UnitData = line
    };

    currentParent.Subordinates.Add(newUnit);

    currentParent = newUnit;

    return hasNationality ? "NationComposite" : "Composite";
}

string ProcessLineElementary(string line, RawUnitData currentParent, bool hasNationality) {
    currentParent.Subordinates.Add(new RawUnitData {
        Parent = currentParent,
        HasNationality = hasNationality,
        UnitData = line
    });
    return hasNationality ? "NationElementary" : "Elementary";
}

string ProcessEnd(ref RawUnitData currentParent) {
    currentParent = currentParent.Parent;

    return "EndStatement";
}
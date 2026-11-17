namespace OOP_lr2.Models;

public class ResourceRecord
{
    private readonly Dictionary<string, string> _attributes = new();

    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    public void Set(string name, string value)
    {
        _attributes[name] = value;
    }

    public string Get(string name)
    {
        return _attributes.TryGetValue(name, out var value) ? value : string.Empty;
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, _attributes.Select(pair => $"{pair.Key}: {pair.Value}"));
    }
}

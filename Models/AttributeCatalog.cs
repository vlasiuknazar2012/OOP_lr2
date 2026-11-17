namespace OOP_lr2.Models;

public class AttributeCatalog
{
    private readonly Dictionary<string, SortedSet<string>> _values = new();

    public IEnumerable<string> AttributeNames => _values.Keys;

    public void Add(string attribute, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (!_values.TryGetValue(attribute, out var set))
        {
            set = new SortedSet<string>();
            _values[attribute] = set;
        }

        set.Add(value);
    }

    public void AddRecord(ResourceRecord record)
    {
        foreach (var pair in record.Attributes)
        {
            Add(pair.Key, pair.Value);
        }
    }

    public IReadOnlyList<string> GetValues(string attribute)
    {
        return _values.TryGetValue(attribute, out var set) ? set.ToList() : new List<string>();
    }
}

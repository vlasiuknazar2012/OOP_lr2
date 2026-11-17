namespace OOP_lr2.Models;

public class SearchCriteria
{
    private readonly Dictionary<string, string> _conditions = new();

    public bool IsEmpty => _conditions.Count == 0;

    public IReadOnlyDictionary<string, string> Conditions => _conditions;

    public void Set(string attribute, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _conditions.Remove(attribute);
        }
        else
        {
            _conditions[attribute] = value;
        }
    }

    public void Clear()
    {
        _conditions.Clear();
    }

    public bool Matches(ResourceRecord record)
    {
        return _conditions.All(condition =>
            record.Attributes.TryGetValue(condition.Key, out var value) && value == condition.Value);
    }
}

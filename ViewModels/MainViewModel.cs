using System.Text;
using OOP_lr2.Models;
using OOP_lr2.Services;
using OOP_lr2.Strategies;

namespace OOP_lr2.ViewModels;

public class MainViewModel
{
    private const string XslResourceName = "resources.xsl";

    private readonly XmlSearchContext _context = new();
    private readonly HtmlTransformer _transformer = new(XslResourceName);
    private readonly Dictionary<string, IXmlAnalysisStrategy> _strategies;
    private readonly SearchCriteria _criteria = new();
    private AttributeCatalog _catalog = new();

    public MainViewModel()
    {
        _strategies = new IXmlAnalysisStrategy[]
        {
            new SaxAnalysisStrategy(),
            new DomAnalysisStrategy(),
            new LinqToXmlAnalysisStrategy()
        }.ToDictionary(strategy => strategy.Name);

        _context.SetStrategy(_strategies.Values.First());
    }

    public string XmlPath { get; private set; } = string.Empty;

    public string ResultText { get; private set; } = string.Empty;

    public IEnumerable<string> AttributeNames => _catalog.AttributeNames;

    public IEnumerable<string> StrategyNames => _strategies.Keys;

    public void SelectFile(string xmlPath)
    {
        var catalog = _context.LoadAttributeValues(xmlPath);
        XmlPath = xmlPath;
        _catalog = catalog;
        _criteria.Clear();
        ResultText = string.Empty;
    }

    public void SelectStrategy(string name)
    {
        _context.SetStrategy(_strategies[name]);
    }

    public IReadOnlyList<string> GetValues(string attribute)
    {
        return _catalog.GetValues(attribute);
    }

    public void SetCriterion(string attribute, string value)
    {
        _criteria.Set(attribute, value);
    }

    public void Search()
    {
        EnsureFileSelected();
        if (_criteria.IsEmpty)
        {
            throw new InvalidOperationException("Оберіть значення хоча б одного атрибута для пошуку.");
        }

        var found = _context.Search(XmlPath, _criteria);
        if (found.Count == 0)
        {
            ResultText = "За заданими параметрами нічого не знайдено.";
            return;
        }

        var builder = new StringBuilder();
        builder.AppendLine($"Знайдено ресурсів: {found.Count}");
        for (var i = 0; i < found.Count; i++)
        {
            builder.AppendLine();
            builder.AppendLine($"Ресурс {i + 1}");
            builder.AppendLine(found[i].ToString());
        }

        ResultText = builder.ToString();
    }

    public string TransformToHtml()
    {
        EnsureFileSelected();
        return _transformer.Transform(XmlPath);
    }

    public void Clear()
    {
        _criteria.Clear();
        ResultText = string.Empty;
    }

    private void EnsureFileSelected()
    {
        if (string.IsNullOrEmpty(XmlPath))
        {
            throw new InvalidOperationException("Спочатку оберіть XML-файл.");
        }
    }
}

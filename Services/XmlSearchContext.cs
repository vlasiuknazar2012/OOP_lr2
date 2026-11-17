using OOP_lr2.Models;
using OOP_lr2.Strategies;

namespace OOP_lr2.Services;

public class XmlSearchContext
{
    private IXmlAnalysisStrategy? _strategy;

    public void SetStrategy(IXmlAnalysisStrategy strategy)
    {
        _strategy = strategy;
    }

    public AttributeCatalog LoadAttributeValues(string xmlPath)
    {
        return CurrentStrategy().LoadAttributeValues(xmlPath);
    }

    public List<ResourceRecord> Search(string xmlPath, SearchCriteria criteria)
    {
        return CurrentStrategy().Search(xmlPath, criteria);
    }

    private IXmlAnalysisStrategy CurrentStrategy()
    {
        return _strategy ?? throw new InvalidOperationException("Спосіб аналізу документа не обрано.");
    }
}

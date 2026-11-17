using OOP_lr2.Models;

namespace OOP_lr2.Strategies;

public interface IXmlAnalysisStrategy
{
    string Name { get; }

    AttributeCatalog LoadAttributeValues(string xmlPath);

    List<ResourceRecord> Search(string xmlPath, SearchCriteria criteria);
}

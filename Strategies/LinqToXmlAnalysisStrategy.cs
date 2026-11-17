using System.Xml.Linq;
using OOP_lr2.Models;

namespace OOP_lr2.Strategies;

public class LinqToXmlAnalysisStrategy : IXmlAnalysisStrategy
{
    public string Name => "LINQ to XML";

    public AttributeCatalog LoadAttributeValues(string xmlPath)
    {
        var document = XDocument.Load(xmlPath);
        var catalog = new AttributeCatalog();
        foreach (var record in document.Descendants(XmlNames.ResourceElement).Select(ToRecord))
        {
            catalog.AddRecord(record);
        }

        return catalog;
    }

    public List<ResourceRecord> Search(string xmlPath, SearchCriteria criteria)
    {
        var document = XDocument.Load(xmlPath);
        var predicate = BuildPredicate(criteria);
        return document.Descendants(XmlNames.ResourceElement)
            .Where(predicate)
            .Select(ToRecord)
            .ToList();
    }

    private static Func<XElement, bool> BuildPredicate(SearchCriteria criteria)
    {
        var conditions = criteria.Conditions.ToList();
        return element => conditions.All(condition =>
            element.Attribute(condition.Key)?.Value == condition.Value ||
            element.Element(XmlNames.AuthorElement)?.Attribute(condition.Key)?.Value == condition.Value);
    }

    private static ResourceRecord ToRecord(XElement element)
    {
        var record = new ResourceRecord();
        CopyAttributes(element, record);
        var author = element.Element(XmlNames.AuthorElement);
        if (author != null)
        {
            CopyAttributes(author, record);
        }

        return record;
    }

    private static void CopyAttributes(XElement element, ResourceRecord record)
    {
        foreach (var attribute in element.Attributes())
        {
            record.Set(attribute.Name.LocalName, attribute.Value);
        }
    }
}

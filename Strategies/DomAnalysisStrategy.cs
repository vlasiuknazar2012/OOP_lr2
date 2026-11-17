using System.Text;
using System.Xml;
using OOP_lr2.Models;

namespace OOP_lr2.Strategies;

public class DomAnalysisStrategy : IXmlAnalysisStrategy
{
    public string Name => "DOM API";

    public AttributeCatalog LoadAttributeValues(string xmlPath)
    {
        var document = Load(xmlPath);
        var catalog = new AttributeCatalog();
        foreach (var record in SelectRecords(document, $"//{XmlNames.ResourceElement}"))
        {
            catalog.AddRecord(record);
        }

        return catalog;
    }

    public List<ResourceRecord> Search(string xmlPath, SearchCriteria criteria)
    {
        var document = Load(xmlPath);
        return SelectRecords(document, BuildXPath(criteria));
    }

    private static string BuildXPath(SearchCriteria criteria)
    {
        var builder = new StringBuilder($"//{XmlNames.ResourceElement}");
        foreach (var condition in criteria.Conditions)
        {
            var literal = ToXPathLiteral(condition.Value);
            builder.Append($"[@{condition.Key}={literal} or {XmlNames.AuthorElement}/@{condition.Key}={literal}]");
        }

        return builder.ToString();
    }

    private static string ToXPathLiteral(string value)
    {
        if (!value.Contains('\''))
        {
            return $"'{value}'";
        }

        if (!value.Contains('"'))
        {
            return $"\"{value}\"";
        }

        return "concat('" + string.Join("', \"'\", '", value.Split('\'')) + "')";
    }

    private static XmlDocument Load(string xmlPath)
    {
        var document = new XmlDocument();
        document.Load(xmlPath);
        return document;
    }

    private static List<ResourceRecord> SelectRecords(XmlDocument document, string xpath)
    {
        var records = new List<ResourceRecord>();
        var nodes = document.SelectNodes(xpath);
        if (nodes == null)
        {
            return records;
        }

        foreach (XmlNode node in nodes)
        {
            records.Add(ToRecord(node));
        }

        return records;
    }

    private static ResourceRecord ToRecord(XmlNode node)
    {
        var record = new ResourceRecord();
        CopyAttributes(node, record);
        var author = node.SelectSingleNode(XmlNames.AuthorElement);
        if (author != null)
        {
            CopyAttributes(author, record);
        }

        return record;
    }

    private static void CopyAttributes(XmlNode node, ResourceRecord record)
    {
        if (node.Attributes == null)
        {
            return;
        }

        foreach (XmlAttribute attribute in node.Attributes)
        {
            record.Set(attribute.Name, attribute.Value);
        }
    }
}

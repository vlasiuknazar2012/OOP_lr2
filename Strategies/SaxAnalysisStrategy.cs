using System.Xml;
using OOP_lr2.Models;

namespace OOP_lr2.Strategies;

public class SaxAnalysisStrategy : IXmlAnalysisStrategy
{
    public string Name => "SAX API";

    public AttributeCatalog LoadAttributeValues(string xmlPath)
    {
        var catalog = new AttributeCatalog();
        foreach (var record in ReadResources(xmlPath))
        {
            catalog.AddRecord(record);
        }

        return catalog;
    }

    public List<ResourceRecord> Search(string xmlPath, SearchCriteria criteria)
    {
        return ReadResources(xmlPath).Where(criteria.Matches).ToList();
    }

    private static IEnumerable<ResourceRecord> ReadResources(string xmlPath)
    {
        var settings = new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true };
        using var reader = XmlReader.Create(xmlPath, settings);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == XmlNames.ResourceElement)
            {
                yield return ReadResource(reader);
            }
        }
    }

    private static ResourceRecord ReadResource(XmlReader reader)
    {
        var record = new ResourceRecord();
        CopyAttributes(reader, record);
        if (reader.IsEmptyElement)
        {
            return record;
        }

        var depth = reader.Depth;
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Depth == depth)
            {
                break;
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == XmlNames.AuthorElement)
            {
                CopyAttributes(reader, record);
            }
        }

        return record;
    }

    private static void CopyAttributes(XmlReader reader, ResourceRecord record)
    {
        if (!reader.HasAttributes)
        {
            return;
        }

        while (reader.MoveToNextAttribute())
        {
            record.Set(reader.Name, reader.Value);
        }

        reader.MoveToElement();
    }
}

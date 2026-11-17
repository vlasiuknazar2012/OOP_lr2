using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace OOP_lr2.Services;

public class HtmlTransformer
{
    private readonly string _xslResourceName;

    public HtmlTransformer(string xslResourceName)
    {
        _xslResourceName = xslResourceName;
    }

    public string Transform(string xmlPath)
    {
        using var xslStream = typeof(HtmlTransformer).Assembly.GetManifestResourceStream(_xslResourceName)
            ?? throw new FileNotFoundException($"XSL-документ '{_xslResourceName}' не знайдено в ресурсах програми.");
        using var xslReader = XmlReader.Create(xslStream);

        var transform = new XslCompiledTransform();
        transform.Load(xslReader);

        var settings = transform.OutputSettings.Clone();
        settings.Encoding = new UTF8Encoding(false);

        using var output = new MemoryStream();
        using (var xmlReader = XmlReader.Create(xmlPath))
        using (var writer = XmlWriter.Create(output, settings))
        {
            transform.Transform(xmlReader, writer);
        }

        return Encoding.UTF8.GetString(output.ToArray());
    }
}

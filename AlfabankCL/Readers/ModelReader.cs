using System.Xml;

namespace ReadersAndWriters.Readers;

public static class ModelReader
{
    public static Article[] Read()
    {
        var result = new List<Article>();
        var xDoc = new XmlDocument();
        xDoc.Load(Constants.DataPath);
        XmlElement xRoot = xDoc.DocumentElement ?? throw new Exception("root element is null");

        foreach (XmlElement xnode in xRoot)
        {
            var article = new Article
            {
                Title = xnode[Constants.Title]?.InnerText ?? string.Empty,
                Link = xnode[Constants.Link]?.InnerText ?? string.Empty,
                Description = xnode[Constants.Description]?.InnerText ?? string.Empty,
                Category = xnode[Constants.Category]?.InnerText ?? string.Empty,
                PubDate = xnode[Constants.PubDate]?.InnerText ?? string.Empty
            };
            result.Add(article);
        }

        return result.ToArray();
    }

}

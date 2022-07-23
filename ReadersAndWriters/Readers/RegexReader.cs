using System.Text.RegularExpressions;
using System.Xml;

namespace ReadersAndWriters.Readers;

public static class RegexReader
{
    public static Article[] Read()
    {
        var xDoc = new XmlDocument();
        xDoc.Load(Constants.DataPath);
        string xml = xDoc.InnerXml;
        var result = new List<Article>();
        var itemPattern = string.Format("(?:<{0}.*?>)(.*?)(?:<\\/{0}>)", "item");
        var itemRegex = new Regex(itemPattern);
        MatchCollection itemMatches = itemRegex.Matches(xml);
        foreach (Match itemMatch in itemMatches)
        {
            string itemXmlValue = itemMatch.Groups[1].Captures[0].Value;
            Article article = BuildArticle(itemXmlValue);
            result.Add(article);
        }
        return result.ToArray();
    }

    private static Article BuildArticle(string itemXmlValue)
    {
        var article = new Article();
        var tags = new List<string>
            {
                Constants.Title,
                Constants.Description,
                Constants.Link,
                Constants.Category,
                Constants.PubDate,
            };
        foreach (string tagName in tags)
        {
            var tagPattern = string.Format("(?:<{0}.*?>)(.*?)(?:<\\/{0}>)", tagName);
            var tagRegex = new Regex(tagPattern);
            Match tagMatch = tagRegex.Match(itemXmlValue);
            if (tagMatch.Success)
            {
                string tagInnerText = tagMatch.Groups[1].Captures[0].Value;
                switch (tagName)
                {
                    case Constants.Title: article.Title = tagInnerText; break;
                    case Constants.Category: article.Category = tagInnerText; break;
                    case Constants.Description: article.Description = tagInnerText; break;
                    case Constants.PubDate: article.PubDate = tagInnerText; break;
                    case Constants.Link: article.Link = tagInnerText; break;
                }
            }
        }
        return article;
    }
}

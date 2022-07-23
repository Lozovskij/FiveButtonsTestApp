using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ReadersAndWriters.Writers;

public static class WordWriter
{
    public static void Write(Article[] articles, string path)
    {
        using WordprocessingDocument package = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);
        package.AddMainDocumentPart();
        if (package.MainDocumentPart is null) throw new Exception();
        var docBody = new Body();

        foreach (Article article in articles)
        {
            PushArticle(docBody, article);
        }

        var document = new Document();
        document.Append(docBody);

        // Save changes to the main document part.
        package.MainDocumentPart.Document = document;
        package.MainDocumentPart.Document.Save();
    }

    private static void PushArticle(Body docBody, Article article)
    {
        var titleText = new Text(article.Title);
        var linkText = new Text(article.Link);
        var descriptionText = new Text(article.Description);
        var categoryText = new Text(article.Category);
        var PubDateText = new Text(article.PubDate);

        var r = new Run();
        var p = new Paragraph();
        r.Append(categoryText);
        p.Append(r);
        docBody.Append(p);

        r = new Run();
        p = new Paragraph();
        r.Append(PubDateText);
        p.Append(r);
        docBody.Append(p);

        p = new Paragraph();
        var rp = new RunProperties();
        rp.Append(new Bold());
        rp.Append(new FontSize { Val = new StringValue("30") });
        rp.Append(new RunFonts { Ascii = "Arial" });
        r = new Run();
        r.PrependChild(rp);
        r.Append(titleText);
        p.Append(r);
        docBody.Append(p);

        r = new Run();
        p = new Paragraph();
        r.Append(descriptionText);
        p.Append(r);
        docBody.Append(p);

        r = new Run();
        p = new Paragraph();
        r.Append(linkText);
        p.Append(r);
        docBody.Append(p);

        docBody.Append(new Paragraph());
    }
}

namespace ReadersAndWriters.Writers;

public static class TxtWriter
{
    public static async Task WriteAsync(Article[] articles, string path)
    {
        await File.WriteAllLinesAsync(path, articles.Select(a => $"{a.PubDate}\n{a.Title}\n{a.Description}\n{a.Category}\n{a.Link}\n\n"));
    }
}
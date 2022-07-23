using System.ComponentModel;

namespace ReadersAndWriters;

public class Article
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }

    [DisplayName("Date of publication")]
    public string PubDate { get; set; }
    public override string ToString()
    {
        return $"{nameof(Title)}: {Title} \n{nameof(Description)}: {Description}";
    }
}

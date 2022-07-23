using ReadersAndWriters;
using ReadersAndWriters.Readers;
using ReadersAndWriters.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAlfabankTestApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{   
    public MainWindow()
    {
        InitializeComponent();
    }

    private List<Task> _writingTasks = new();
    private Task<Article[]> _readingTask;
    private string _readingMode;

    //need to put a restriction on data.xml access
    private readonly SemaphoreSlim _semaphore = new(1,1);

    private async void read_Click(object sender, RoutedEventArgs e)
    {
        writeTxt.IsEnabled = true;
        writeDocx.IsEnabled = true;
        writeExcel.IsEnabled = true;

        await _semaphore.WaitAsync();
        await Task.WhenAll(_writingTasks); // wait until every writing task is finished before i change data
        _readingMode = ((Button)sender).Tag.ToString() ?? throw new Exception();
        _readingTask = _readingMode switch
        {
            "regex" => Task.Run(() => RegexReader.Read()),
            "model" => Task.Run(() => ModelReader.Read()),
            _ => throw new System.NotImplementedException(),
        };
        _semaphore.Release();
    }

    private async void write_Click(object sender, RoutedEventArgs e)
    {
        await _semaphore.WaitAsync();
        var articles = await _readingTask;
        string writingMode = ((Button)sender).Tag.ToString() ?? throw new Exception();
        _writingTasks.Add(writingMode switch
        {
            "txt" => TxtWriter.WriteAsync(articles, GetSavingPath("txt")),
            "docx" => Task.Run(() => WordWriter.Write(articles, GetSavingPath("docx"))),
            "excel" => ExcelWriter.WriteAsync(articles, GetSavingPath("xlsx")),
            _ => throw new System.NotImplementedException(),
        });
        _semaphore.Release();
    }

    private string GetSavingPath(string fileExtension)
    {
        var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_articles_{_readingMode}.{fileExtension}";
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ivan_Lozovskij_Test_App");
        System.IO.Directory.CreateDirectory(folderPath);
        return Path.Combine(folderPath, fileName);
    }
}

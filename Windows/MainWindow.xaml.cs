using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace AdobePDFServicesFront.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window,INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        //
        DataContext = this;
        if (Directory.Exists(Resource.TempPath) == false)
        {
            Directory.CreateDirectory(Resource.TempPath);
        }
    }

    private async void _loaded(object sender, RoutedEventArgs e)
    {
        await _webView.EnsureCoreWebView2Async(null);
    }

    public string FilePath 
    { 
        get => _filePath;
        set 
        {
            if (_filePath ==value)
            {
                return;
            }
            if (string.IsNullOrEmpty( value)==true)
            {
                return;
            }
            _filePath = value;

            _webView.Source = new Uri(@"file://" + _filePath);

            _notifyPropertyChanged();
        }
    }
    private string _filePath =string.Empty;

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged =null;
    private void _notifyPropertyChanged([CallerMemberName]string name_="")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name_));
    }
    #endregion
}
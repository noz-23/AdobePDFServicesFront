using Adobe.PDFServicesSDK.auth;
using AdobePDFServicesFront.Controls;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace AdobePDFServicesFront.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
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

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged = null;
    private void _notifyPropertyChanged([CallerMemberName] string name_ = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name_));
    }
    #endregion
    #region プロパティ
    public string FilePath
    {
        get => _filePath;
        set
        {
            if (_filePath == value)
            {
                return;
            }
            if (string.IsNullOrEmpty(value) == true)
            {
                return;
            }
            _filePath = value;

            _webView.Source = new Uri(@"file://" + _filePath);

            _notifyPropertyChanged();
        }
    }
    private string _filePath = string.Empty;


    //// Initial setup, create credentials instance
    //ICredentials credentials = new ServicePrincipalCredentials(
    //    Environment.GetEnvironmentVariable("PDF_SERVICES_CLIENT_ID"),
    //    Environment.GetEnvironmentVariable("PDF_SERVICES_CLIENT_SECRET"));

    //private ICredentials? Credentials
    //{ 
    //}

    #endregion

    #region イベント
    private async void _loaded(object sender, RoutedEventArgs e)
    {
        await _webView.EnsureCoreWebView2Async(null);
    }
    #endregion

    public List<Control> _controlList = new ();

    private void _combineMenuItemClick(object sender_, RoutedEventArgs e_)
    {
        _stackPanel.Children.Add(new CombineControl(_stackPanel));
        Debug.WriteLine("追加 結合");
    }

    private void _autoTagMenuItemClick(object sender_, RoutedEventArgs e_)
    {
        _stackPanel.Children.Add(new AutoTagControl(_stackPanel));
        Debug.WriteLine("追加 自動タグ");
    }
    
}
using Adobe.PDFServicesSDK.io;
using AdobePDFServicesFront.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
        if (Directory.Exists(Properties.Resources.TempPath) == false)
        {
            Directory.CreateDirectory(Properties.Resources.TempPath);
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

    public ObservableCollection<EventControl> ControlList { get; } =new ();

    //// Initial setup, create credentials instance
    //ICredentials credentials = new ServicePrincipalCredentials(
    //    Environment.GetEnvironmentVariable("PDF_SERVICES_CLIENT_ID"),
    //    Environment.GetEnvironmentVariable("PDF_SERVICES_CLIENT_SECRET"));

    //private ICredentials? Credentials
    //{ 
    //}

    #endregion

    #region イベント
    private async void _loaded(object sender_, RoutedEventArgs e_)
    {
        await _webView.EnsureCoreWebView2Async(null);
    }

    private void _runButtonClick(object sender_, RoutedEventArgs e_)
    {
        var pdfServices = _credentialsControl.PdfServices;

        if (pdfServices == null)
        {
            MessageBox.Show("認証失敗。");
            return;
        }
        var asset = _selectPdfControl.GetAsset(pdfServices);

        foreach (var control in ControlList)
        {
            asset = control.EventProcess(pdfServices, asset);
        }

        if (asset != null)
        {
            _exportControl.EventProcess(pdfServices, asset);
        }
    }
    #endregion

    //public List<Control> _controlList = new ();

    #region メニュー
    private void _combineMenuItemClick(object sender_, RoutedEventArgs e_)=> ControlList.Add(new CombineControl(ControlList));
    private void _autoTagMenuItemClick(object sender_, RoutedEventArgs e_)=> ControlList.Add(new AutoTagControl(ControlList));
    private void _compressMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new CompressControl(ControlList));
    private void _ocrMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new OcrControl(ControlList));
    #endregion

    // D&D
    // https://qiita.com/gego/items/25298cd52c4612fc1edf
}
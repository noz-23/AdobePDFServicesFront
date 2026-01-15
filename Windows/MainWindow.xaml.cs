using AdobePDFServicesFront.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

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

        _statusProgressBar.Maximum = ControlList.Count + 1;
        _statusProgressBar.Value = 0;
        _statusTextBlock.Text = "処理 [読込]";
        //
        var asset = _selectPdfControl.GetAsset(pdfServices);
        _statusProgressBar.Value++;

        foreach (var control in ControlList)
        {
            _statusTextBlock.Text = $"処理 [{control.TitleName}]";
            asset = control.EventProcess(pdfServices, asset);
            _statusProgressBar.Value++;
        }

        if (asset != null)
        {
            _exportControl.EventProcess(pdfServices, asset);
        }
    }
    #endregion

    #region メニュー
    private void _combineMenuItemClick(object sender_, RoutedEventArgs e_)=> ControlList.Add(new CombineControl(_selectPdfControl, ControlList));
    private void _deleteMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new DeleteControl(_selectPdfControl, ControlList));
    private void _autoTagMenuItemClick(object sender_, RoutedEventArgs e_)=> ControlList.Add(new AutoTagControl(_selectPdfControl, ControlList));
    private void _compressMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new CompressControl(_selectPdfControl, ControlList));
    private void _ocrMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new OcrControl(_selectPdfControl, ControlList));
    private void _protectMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new ProtectControl(_selectPdfControl, ControlList));
    private void _removeProtectionMenuItemClick(object sender_, RoutedEventArgs e_) => ControlList.Add(new RemoveProtectControl(_selectPdfControl, ControlList));
    #endregion

    // D&D
    // https://qiita.com/gego/items/25298cd52c4612fc1edf
}
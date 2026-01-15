using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using AdobePDFServicesFront.Interfaces;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Windows.Data.Pdf;
using Windows.Storage;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// OpenPdfControl.xaml の相互作用ロジック
/// </summary>
public partial class OpenPdfControl : UserControl, INotifyPropertyChanged, IPageCount, ITitileName
{
    // https://qiita.com/tricogimmick/items/62cd9f5deca365a83858
    public OpenPdfControl():base()
    {
        InitializeComponent();
        //
        //DataContext = this;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged = null;
    private void _notifyPropertyChanged([CallerMemberName] string name_ = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name_));
    }
    #endregion

    #region プロパティ
    public string Path
    {
        get => (string)GetValue(PathProperty);
        set
        {
            SetValue(PathProperty, value);
            _notifyPropertyChanged();
        }
    }
    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(OpenPdfControl));

    public string TitleName
    {
        get => _textBlock.Text;
        set => _textBlock.Text = value;
    }
    public int PageCount
    {
        get => _pageCount;
        set
        {
            if (_pageCount == value)
            {
                return;
            }
            _pageCount = value;
            _notifyPropertyChanged();
        }
    }
    private int _pageCount = 0;

    public string Info
    {
        get=> string.Format("{0} 頁, 保護 [{1}]",PageCount,(_isProtected == true) ? "あり" : "なし");
    }
    private bool _isProtected = false;

    #endregion

    private void _buttonClick(object sender_, RoutedEventArgs e_)
    {
        var dlg = new OpenFileDialog
        {
            DefaultExt = Properties.Resources.ExtensionPDF,
            Filter = $"PDF ファイル (*{Properties.Resources.ExtensionPDF})|*{Properties.Resources.ExtensionPDF}",
            Multiselect = false
        };
        //
        if (dlg.ShowDialog() == false)
        {
            return;
        }
        //
        this.Path = dlg.FileName;
        Debug.WriteLine($"Open [{Path}]");

        var file = StorageFile.GetFileFromPathAsync(this.Path).Get();
        var pdf =PdfDocument.LoadFromFileAsync(file).AsTask().Result;
        //
        PageCount = (int)pdf.PageCount;
        _isProtected = pdf.IsPasswordProtected;
        _notifyPropertyChanged(nameof(Info));
    }

    public IAsset? GetAsset(PDFServices? pdfServices_)
    {
        if (File.Exists(Path) == true)
        {
            using var stream = File.OpenRead(Path);

            return pdfServices_?.Upload(stream, PDFServicesMediaType.PDF.GetMIMETypeValue());
        }
        return null;
    }
}

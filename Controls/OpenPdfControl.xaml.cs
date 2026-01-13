using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
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
public partial class OpenPdfControl : UserControl,INotifyPropertyChanged
{
    // https://qiita.com/tricogimmick/items/62cd9f5deca365a83858
    public OpenPdfControl()
    {
        InitializeComponent();
        //
        DataContext = this;
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
        get => (string)GetValue(SelectFilePathProperty);
        set
        {
            SetValue(SelectFilePathProperty, value);
            _notifyPropertyChanged();
        }
    }
    // static ↓ ?
    public static readonly DependencyProperty SelectFilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(OpenPdfControl));

    public string Title
    {
        get => _title;
        set 
        {
            if (_title == value)
            {
                return;
            }
            _title = value;
            _notifyPropertyChanged();
        }
    }
    private string _title = "対象";

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
        this.FilePath =dlg.FileName;
        Debug.WriteLine($"Open [{FilePath}]");

        var file = StorageFile.GetFileFromPathAsync(this.FilePath).Get();
        //ここから
        var pdf =PdfDocument.LoadFromFileAsync(file).AsTask().Result;
        PageCount = (int)pdf.PageCount;
    }

    public IAsset? GetAsset(PDFServices? pdfServices_)
    {
        if (File.Exists(FilePath) == true)
        {
            using var stream = File.OpenRead(FilePath);

            return pdfServices_?.Upload(stream, PDFServicesMediaType.PDF.GetMIMETypeValue());
        }
        return null;
    }


}

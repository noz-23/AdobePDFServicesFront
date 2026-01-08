using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// SelectPdfControl.xaml の相互作用ロジック
/// </summary>
public partial class SelectPdfControl : UserControl,INotifyPropertyChanged
{
    // https://qiita.com/tricogimmick/items/62cd9f5deca365a83858
    public SelectPdfControl()
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
    public static readonly DependencyProperty SelectFilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(SelectPdfControl));

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

    #endregion

    private void _buttonClick(object sender_, RoutedEventArgs e_)
    {
        var dlg = new OpenFileDialog
        {
            DefaultExt = Resource.ExtensionPDF,
            Filter = $"PDF ファイル (*{Resource.ExtensionPDF})|*{Resource.ExtensionPDF}",
            Multiselect = false
        };
        //
        if (dlg.ShowDialog() == false)
        {
            return;
        }
        //
        this.FilePath =dlg.FileName;
        Debug.WriteLine($"FilePath {FilePath}");
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

using AdobePDFServicesFront.Interfaces;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// OpenPdfControl.xaml の相互作用ロジック
/// </summary>
public partial class SaveFileControl : UserControl,INotifyPropertyChanged, ITitileName
{
    // https://qiita.com/tricogimmick/items/62cd9f5deca365a83858
    public SaveFileControl()
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
        get => _path;
        set
        {
            _path = value;
            _notifyPropertyChanged();
        }
    }
    private string _path = string.Empty; 

    public string Extension{ get;set;}

    public string TitleName
    {
        get => _textBlock.Text;
        set => _textBlock.Text =value;
    }
    #endregion

    private void _buttonClick(object sender_, RoutedEventArgs e_)
    {
        SetFilePath();
    }

    public void SetFilePath()
    {
        var dlg = new SaveFileDialog
        {
            DefaultExt = Extension,
            Filter = $"保存 ファイル (*{Extension})|*{Extension}",
        };
        //
        if (dlg.ShowDialog() == false)
        {
            return;
        }
        //
        this.Path = dlg.FileName;
        Debug.WriteLine($"Save [{Path}]");
    }
}

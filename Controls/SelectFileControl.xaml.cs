using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// SelectFileControl.xaml の相互作用ロジック
/// </summary>
public partial class SelectFileControl : UserControl,INotifyPropertyChanged
{
    // https://qiita.com/tricogimmick/items/62cd9f5deca365a83858
    public SelectFileControl()
    {
        InitializeComponent();
        //
        DataContext = this;
    }

    public string FilePath
    {
        get => (string)GetValue(SelectFilePathProperty);
        set
        {
            SetValue(SelectFilePathProperty, value);
            _notifyPropertyChanged();
        }
    }
    //public static readonly DependencyProperty SelectFilePathProperty = DependencyProperty.Register(nameof(FilePath),
    //                                typeof(string),
    //                                typeof(SelectFileControl),
    //                                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, _textPropertyChanged));
    //public static readonly DependencyProperty SelectFilePathProperty = 
    //    DependencyProperty.Register(nameof(FilePath),
    //                                typeof(string),
    //                                typeof(SelectFileControl),
    //                                //new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    //                                new FrameworkPropertyMetadata(string.Empty, _textPropertyChanged));
    public static readonly DependencyProperty SelectFilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(SelectFileControl));


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

    private static void _textPropertyChanged(DependencyObject obj_, DependencyPropertyChangedEventArgs e_)
    {
        if (obj_ is SelectFileControl ctrl)
        {
            //ctrl.FilePath = string.Empty;
            Console.WriteLine($"SelectFileControl: FilePath changed to {e_.NewValue}");
        }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged = null;
    private void _notifyPropertyChanged([CallerMemberName] string name_ = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name_));
    }
    #endregion

}

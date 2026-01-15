using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

public class EventControl:UserControl, ITitileName
{
    protected EventControl() : base()
    {
        _main = null; 
        _controlList = null;
        //DataContext = this;
    }

    protected EventControl(IPageCount main_, ObservableCollection<EventControl> list_) : this()
    {
        _main = main_;
        _controlList = list_;
        //DataContext = this;
    }

    //#region INotifyPropertyChanged
    //public event PropertyChangedEventHandler? PropertyChanged = null;
    //private void _notifyPropertyChanged([CallerMemberName] string name_ = "")
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name_));
    //}
    //#endregion

    #region プロパティ
    //public string TitleName
    //{
    //    get => _titleName;
    //    set
    //    {
    //        if (_titleName == value)
    //        {
    //            return;
    //        }
    //        _titleName = value;
    //        //_notifyPropertyChanged();
    //    }
    //}
    //private string _titleName = string.Empty;
    public virtual string TitleName { get; set; }
    #endregion

    #region イベント
    protected void _ButtonClick(object sender_, System.Windows.RoutedEventArgs e_)
    {
        _controlList?.Remove(this);

        Debug.WriteLine($"削除 [{sender_.GetType().ToString()}]");
    }
    #endregion

    private readonly IPageCount? _main;
    private readonly ObservableCollection<EventControl>? _controlList;

    public virtual IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_) 
    {
        return asset_;
    }

    public int NowPageCount()
    {
        var rtn = _main?.PageCount??0;

        foreach (var control in _controlList)
        {
            if (control == this)
            {
                break;
            }
            if (control is IPageCount pageControl)
            {
                rtn += pageControl.PageCount;
            }
        }

        return rtn;
    }
}

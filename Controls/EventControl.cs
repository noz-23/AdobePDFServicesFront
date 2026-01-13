using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

public class EventControl:UserControl
{
    protected EventControl() : base()
    {
        _controlList = new();
    }

    protected EventControl(ObservableCollection<EventControl> list_) : base()
    {
        _controlList = list_;
    }

    #region イベント
    protected void _ButtonClick(object sender_, System.Windows.RoutedEventArgs e_)
    {
        _controlList.Remove(this);

        Debug.WriteLine($"削除 [{sender_.GetType().ToString()}]");
    }
    #endregion

    //private Panel? _owner = null;
    private readonly ObservableCollection<EventControl> _controlList;

    public virtual IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_) 
    {
        return asset_;
    }

    public int NowPageCount()
    {
        var rtn = 0;

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

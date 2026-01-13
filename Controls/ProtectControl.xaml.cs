using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.ocr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
/// ProtectControl.xaml の相互作用ロジック
/// </summary>
public partial class ProtectControl : EventControl
{
    public ProtectControl(ObservableCollection<EventControl> list_) : base()
    {
        Debug.WriteLine("追加 保護");
        InitializeComponent();
    }

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 保護");

        return asset_;
    }

}

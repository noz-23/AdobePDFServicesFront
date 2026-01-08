using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.autotag;
using Adobe.PDFServicesSDK.pdfjobs.results;
using Microsoft.Win32;
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
/// AutoTagControl.xaml の相互作用ロジック
/// </summary>
public partial class AutoTagControl : EventControl
{
    public AutoTagControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 自動タグ");

        InitializeComponent();

        //_owner = owner_;
    }

    //private Panel? _owner = null;

    //#region イベント
    //private void _buttonClick(object sender, System.Windows.RoutedEventArgs e)
    //{
    //    _owner?.Children.Remove(this);

    //    Debug.WriteLine($"削除 自動タグ");
    //}
    //#endregion

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 自動タグ");

        var builder = AutotagPDFParams.AutotagPDFParamsBuilder();
        if (_checkBox.IsChecked == true)
        {
            builder.ShiftHeadings();
        }
        var autotagPDFParams = builder.Build();

        // Create parameters for the job
        var autotagPDFJob = new AutotagPDFJob(asset_).SetParams(autotagPDFParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(autotagPDFJob);
        var pdfServicesResponse = pdfServices_?.GetJobResult<AutotagPDFResult>(location, typeof(AutotagPDFResult));

        // Get content from the resulting asset(s)
        //IAsset resultAsset = pdfServicesResponse.Result.TaggedPDF;
        //StreamAsset streamAsset = pdfServices.GetContent(resultAsset);

        return pdfServicesResponse?.Result.TaggedPDF;
    }
}

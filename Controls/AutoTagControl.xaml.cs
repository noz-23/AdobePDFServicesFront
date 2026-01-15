using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.autotag;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// AutoTagControl.xaml の相互作用ロジック
/// </summary>
public partial class AutoTagControl : EventControl
{
    //public AutoTagControl():base()
    //{
    //    InitializeComponent();
    //}

    public AutoTagControl(IPageCount main_, ObservableCollection<EventControl> list_) : base(main_,list_)
    {
        InitializeComponent();
        TitleName = "自動タグ";

        Debug.WriteLine($"追加 [{TitleName}]");
    }
    #region プロパティ
    public override string TitleName
    {
        get => _textBlock.Text;
        set => _textBlock.Text = value;
    }
    #endregion

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine($"処理 [{TitleName}]");

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
        return pdfServicesResponse?.Result.TaggedPDF;
    }
}

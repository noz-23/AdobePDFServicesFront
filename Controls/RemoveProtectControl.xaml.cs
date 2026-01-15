using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.removeprotection;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// RemoveProtectControl.xaml の相互作用ロジック
/// </summary>
public partial class RemoveProtectControl : EventControl
{
    public RemoveProtectControl(IPageCount main_, ObservableCollection<EventControl> list_) : base(main_, list_)
    {
        InitializeComponent();
        //
        TitleName = "保護解除";
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

        // Create parameters for the job
        var removeProtectionParams = new RemoveProtectionParams(_textBox.Text.Trim());

        // Creates a new job instance
        var removeProtectionJob = new RemoveProtectionJob(asset_, removeProtectionParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(removeProtectionJob);
        var pdfServicesResponse =pdfServices_?.GetJobResult<RemoveProtectionResult>(location, typeof(RemoveProtectionResult));

        // Get content from the resulting asset(s)
        return pdfServicesResponse?.Result.Asset;
    }

}

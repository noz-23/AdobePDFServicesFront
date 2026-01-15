using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.compresspdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// CompressControl.xaml の相互作用ロジック
/// </summary>
public partial class CompressControl : EventControl
{
    public CompressControl(IPageCount main_, ObservableCollection<EventControl> list_) : base(main_,list_)
    {
        InitializeComponent();
        //
        TitleName = "軽量化";

        _comboBox.ItemsSource = Enum.GetValues<CompressionLevel>();
        _comboBox.SelectedValue = CompressionLevel.MEDIUM;
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

        if (_comboBox.SelectedValue is CompressionLevel val)
        {
            // Create parameters for the job
            var compressPDFParams = CompressPDFParams.CompressPDFParamsBuilder().WithCompressionLevel(val).Build();

            // Creates a new job instance
            var compressPDFJob = new CompressPDFJob(asset_).SetParams(compressPDFParams);

            // Submits the job and gets the job result
            var location = pdfServices_?.Submit(compressPDFJob);
            var pdfServicesResponse = pdfServices_?.GetJobResult<CompressPDFResult>(location, typeof(CompressPDFResult));

            // Get content from the resulting asset(s)
            return pdfServicesResponse?.Result.Asset;
        }
        return asset_;
    }
}
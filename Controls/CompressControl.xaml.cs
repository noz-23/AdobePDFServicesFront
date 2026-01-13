using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.compresspdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// CompressControl.xaml の相互作用ロジック
/// </summary>
public partial class CompressControl : EventControl
{
    public CompressControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 軽量化");

        InitializeComponent();
        //Debug.WriteLine("CompressControl");
        //_comboBox.ItemsSource = CompressionLevelEnum;
        _comboBox.ItemsSource = Enum.GetValues<CompressionLevel>();
        _comboBox.SelectedValue = CompressionLevel.MEDIUM;
    }

    //public IEnumerable<CompressionLevel> CompressionLevelEnum { get => Enum.GetValues<CompressionLevel>(); }

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 軽量化");

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
            //IAsset resultAsset = pdfServicesResponse.Result.Asset;
            //var streamAsset = pdfServices_?.GetContent(resultAsset);

            // Creating output streams and copying stream asset's content to it
            //String outputFilePath = CreateOutputFilePath();
            //new FileInfo(Directory.GetCurrentDirectory() + outputFilePath).Directory.Create();
            //Stream outputStream = File.OpenWrite(Directory.GetCurrentDirectory() + outputFilePath);
            //streamAsset.Stream.CopyTo(outputStream);
            //outputStream.Close();

            return pdfServicesResponse?.Result.Asset;
        }
        return asset_;
    }
}
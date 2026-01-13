using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.ocr;
using Adobe.PDFServicesSDK.pdfjobs.results;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// OcrControl.xaml の相互作用ロジック
/// </summary>
public partial class OcrControl : EventControl
{
    public OcrControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 OCR");
        InitializeComponent();

        _comboBox.ItemsSource = Enum.GetValues<OCRSupportedLocale>();
        _comboBox.SelectedValue = OCRSupportedLocale.JA_JP;

    }
    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 OCR");

        if (_comboBox.SelectedValue is OCRSupportedLocale val)
        {
            // Create parameters for the job
            var ocrParams = OCRParams.OCRParamsBuilder().WithOcrLocale(val).WithOcrType(OCRSupportedType.SEARCHABLE_IMAGE_EXACT).Build();

            // Creates a new job instance
            var ocrJob = new OCRJob(asset_).SetParams(ocrParams);

            // Submits the job and gets the job result
            var location = pdfServices_?.Submit(ocrJob);
            var pdfServicesResponse = pdfServices_?.GetJobResult<OCRResult>(location, typeof(OCRResult));

            // Get content from the resulting asset(s)
            return pdfServicesResponse?.Result.Asset;

        }
        return asset_;
    }
}

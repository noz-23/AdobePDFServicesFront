using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.exportpdf;
using Adobe.PDFServicesSDK.pdfjobs.parameters.exportpdftoimages;
using Adobe.PDFServicesSDK.pdfjobs.parameters.extractpdf;
using Adobe.PDFServicesSDK.pdfjobs.parameters.splitpdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using System.IO;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// ExportControl.xaml の相互作用ロジック
/// </summary>
public partial class ExportControl : EventControl
{
    public ExportControl():base()
    {
        InitializeComponent();
    }

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        _exportPdf(pdfServices_, asset_);
        _exportXlsx(pdfServices_, asset_);
        _exportPptx(pdfServices_, asset_);
        _exportDocx(pdfServices_, asset_);
        _exportImage(pdfServices_, asset_);
        _exportText(pdfServices_, asset_);

        return asset_;
    }

    private void _exportPdf(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (string.IsNullOrEmpty(_pdfSaveControl.Path) == true)
        {
            _pdfSaveControl.SetFilePath();
        }
        var outputFilePath = _pdfSaveControl.Path;

        if (_splitCheckBox.IsChecked == false)
        {
            // Get content from the resulting asset(s)
            var streamAsset = pdfServices_?.GetContent(asset_);

            // Creating output streams and copying stream asset's content to it
            using var outputStream = File.OpenWrite(outputFilePath);
            streamAsset?.Stream.CopyTo(outputStream);
            outputStream.Close();
        }
        else
        {
            // Create parameters for the job
            var splitPDFParams = new SplitPDFParams();
            splitPDFParams.SetPageCount(int.Parse(_splitTextBox.Text.Trim()));

            // Creates a new job instance
            var splitPDFJob = new SplitPDFJob(asset_, splitPDFParams);

            // Submits the job and gets the job result
            var location = pdfServices_?.Submit(splitPDFJob);
            var pdfServicesResponse = pdfServices_?.GetJobResult<SplitPDFResult>(location, typeof(SplitPDFResult));
            var resultAssets = pdfServicesResponse?.Result.Assets;

            // Save the result to the specified location.
            var index = 0;
            foreach (var resultAsset in resultAssets)
            {
                var splitFilePath = Path.Combine(Path.GetDirectoryName(outputFilePath), Path.GetFileNameWithoutExtension(outputFilePath) + $"({index})" + Path.GetExtension(outputFilePath));

                // Get content from the resulting asset(s)
                var streamAsset = pdfServices_?.GetContent(resultAsset);

                // Creating output streams and copying stream asset's content to it
                using var outputStream = File.OpenWrite(splitFilePath);
                streamAsset?.Stream.CopyTo(outputStream);
                outputStream.Close();
                index++;
            }
        }
    }

    private void _exportXlsx(PDFServices? pdfServices_, IAsset? asset_) => _exportOffice(pdfServices_, asset_, ExportPDFTargetFormat.XLSX);
    private void _exportPptx(PDFServices? pdfServices_, IAsset? asset_) => _exportOffice(pdfServices_, asset_, ExportPDFTargetFormat.PPTX);
    private void _exportDocx(PDFServices? pdfServices_, IAsset? asset_) => _exportOffice(pdfServices_, asset_, ExportPDFTargetFormat.DOCX);
    private void _exportOffice(PDFServices? pdfServices_, IAsset? asset_, ExportPDFTargetFormat format_)
    {
        if (_docxCheckBox.IsChecked == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(_docxSaveControl.Path) == true)
        {
            _docxSaveControl.SetFilePath();
        }
        var outputFilePath = _docxSaveControl.Path;

        // Create parameters for the job
        var exportPDFParams = ExportPDFParams.ExportPDFParamsBuilder(format_).Build();

        // Creates a new job instance
        var exportPDFJob = new ExportPDFJob(asset_, exportPDFParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(exportPDFJob);
        var pdfServicesResponse = pdfServices_?.GetJobResult<ExportPDFResult>(location, typeof(ExportPDFResult));

        // Get content from the resulting asset(s)
        var resultAsset = pdfServicesResponse?.Result.Asset;
        var streamAsset = pdfServices_?.GetContent(resultAsset);

        // Creating output streams and copying stream asset's content to it
        using var outputStream = File.OpenWrite(outputFilePath);
        streamAsset?.Stream.CopyTo(outputStream);
        outputStream.Close();
    }

    private void _exportImage(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (_imageCheckBox.IsChecked == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(_imageSaveControl.Path) == true)
        {
            _imageSaveControl.SetFilePath();
        }
        var outputFilePath = _imageSaveControl.Path;

        // Create parameters for the job
        var exportPDFToImagesParams = ExportPDFToImagesParams.ExportPDFToImagesParamsBuilder(ExportPDFToImagesTargetFormat.PNG, ExportPDFToImagesOutputType.ZIP_OF_PAGE_IMAGES).Build();

        // Creates a new job instance
        var exportPDFToImagesJob = new ExportPDFToImagesJob(asset_, exportPDFToImagesParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(exportPDFToImagesJob);
        var pdfServicesResponse = pdfServices_?.GetJobResult<ExportPDFToImagesResult>(location, typeof(ExportPDFToImagesResult));

        // Get content from the resulting asset(s)
        var resultAssets = pdfServicesResponse?.Result.Assets;
        var streamAsset = pdfServices_?.GetContent(resultAssets[0]);

        // Creating output streams and copying stream asset's content to it
        using var outputStream = File.OpenWrite(outputFilePath);
        streamAsset?.Stream.CopyTo(outputStream);
        outputStream.Close();
    }

    private void _exportText(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (_textCheckBox.IsChecked == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(_textSaveControl.Path) == true)
        {
            _textSaveControl.SetFilePath();
        }
        var outputFilePath = _textSaveControl.Path;

        // Create parameters for the job
        var extractPDFParams = ExtractPDFParams.ExtractPDFParamsBuilder().AddElementToExtract(ExtractElementType.TEXT).Build();

        // Creates a new job instance
        ExtractPDFJob extractPDFJob = new ExtractPDFJob(asset_).SetParams(extractPDFParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(extractPDFJob);
        var pdfServicesResponse = pdfServices_?.GetJobResult<ExtractPDFResult>(location, typeof(ExtractPDFResult));

        // Get content from the resulting asset(s)
        var resultAsset = pdfServicesResponse?.Result.Resource;
        var streamAsset = pdfServices_?.GetContent(resultAsset);

        // Creating output streams and copying stream asset's content to it
        using var outputStream = File.OpenWrite(outputFilePath);
        streamAsset?.Stream.CopyTo(outputStream);
        outputStream.Close();
    }

}

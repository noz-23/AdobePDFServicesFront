using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.exportpdf;
using Adobe.PDFServicesSDK.pdfjobs.parameters.exportpdftoimages;
using Adobe.PDFServicesSDK.pdfjobs.parameters.extractpdf;
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
        if (string.IsNullOrEmpty(_pdfSaveControl.FilePath) == true)
        {
            _pdfSaveControl.SetFilePath();
        }
        var outputFilePath = _pdfSaveControl.FilePath;

        // Get content from the resulting asset(s)
        var streamAsset = pdfServices_?.GetContent(asset_);

        // Creating output streams and copying stream asset's content to it
        using var outputStream = File.OpenWrite(outputFilePath);
        streamAsset?.Stream.CopyTo(outputStream);
        outputStream.Close();
    }

    private void _exportXlsx(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (string.IsNullOrEmpty(_xlsxSaveControl.FilePath) == true)
        {
            _xlsxSaveControl.SetFilePath();
        }
        var outputFilePath = _xlsxSaveControl.FilePath;

        // Create parameters for the job
        var exportPDFParams = ExportPDFParams.ExportPDFParamsBuilder(ExportPDFTargetFormat.XLSX).Build();

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

    private void _exportPptx(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (string.IsNullOrEmpty(_pptxSaveControl.FilePath) == true)
        {
            _pptxSaveControl.SetFilePath();
        }
        var outputFilePath = _pptxSaveControl.FilePath;

        // Create parameters for the job
        var exportPDFParams = ExportPDFParams.ExportPDFParamsBuilder(ExportPDFTargetFormat.PPTX).Build();

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
    private void _exportDocx(PDFServices? pdfServices_, IAsset? asset_)
    {
        if (string.IsNullOrEmpty(_docxSaveControl.FilePath) == true)
        {
            _docxSaveControl.SetFilePath();
        }
        var outputFilePath = _docxSaveControl.FilePath;

        // Create parameters for the job
        var exportPDFParams = ExportPDFParams.ExportPDFParamsBuilder(ExportPDFTargetFormat.DOCX).Build();

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
        if (string.IsNullOrEmpty(_imageSaveControl.FilePath) == true)
        {
            _imageSaveControl.SetFilePath();
        }
        var outputFilePath = _imageSaveControl.FilePath;

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
        if (string.IsNullOrEmpty(_textSaveControl.FilePath) == true)
        {
            _textSaveControl.SetFilePath();
        }
        var outputFilePath = _textSaveControl.FilePath;

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

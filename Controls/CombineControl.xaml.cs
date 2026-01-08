using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.combinepdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// CombineControl.xaml の相互作用ロジック
/// </summary>
public partial class CombineControl : EventControl
{
    // 丸ボタン
    // https://qiita.com/rioLi0N/items/be25d8fe2d8c3b4b3c29
    public CombineControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 結合");

        InitializeComponent();

        //_owner = owner_;
    }
    #region プロパティ
    public string CombinePath
    {
        get => _selectPdfControl.FilePath;
        set => _selectPdfControl.FilePath = value;
    }

    #endregion 

    //#region イベント
    //private void _buttonClick(object sender, System.Windows.RoutedEventArgs e)
    //{
    //    _owner?.Children.Remove(this);

    //    Debug.WriteLine($"削除 結合[{CombinePath}]");
    //}
    //#endregion

    //private Panel? _owner=null;

    //public CombinePDFParams.Builder? CombinePDF(PDFServices? pdfServices_, CombinePDFParams.Builder? builder_=null)
    //{
    //    var asset = _selectPdfControl.GetAsset(pdfServices_);
    //    if (asset != null)
    //    {
    //        builder_ =(builder_==null) ?CombinePDFParams.CombinePDFParamsBuilder().AddAsset(asset): builder_.AddAsset(asset);
    //    }

    //    return builder_;
    //}

    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 結合");

        var combineAsset = _selectPdfControl.GetAsset(pdfServices_);
        if (combineAsset != null)
        {
            // Create parameters for the job
            var combinePDFParams = CombinePDFParams.CombinePDFParamsBuilder().AddAsset(asset_).AddAsset(combineAsset).Build();

            // Creates a new job instance
            var combinePDFJob = new CombinePDFJob(combinePDFParams);

            // Submits the job and gets the job result
            var location = pdfServices_?.Submit(combinePDFJob);
            var pdfServicesResponse =pdfServices_?.GetJobResult<CombinePDFResult>(location, typeof(CombinePDFResult));

            // Get content from the resulting asset(s)
            //IAsset resultAsset = pdfServicesResponse.Result.Asset;
            //StreamAsset streamAsset = pdfServices.GetContent(resultAsset);

            // Creating output streams and copying stream asset's content to it
            ////String outputFilePath = CreateOutputFilePath();
            ////new FileInfo(Directory.GetCurrentDirectory() + outputFilePath).Directory.Create();
            ////Stream outputStream = File.OpenWrite(Directory.GetCurrentDirectory() + outputFilePath);
            ////streamAsset.Stream.CopyTo(outputStream);
            ////outputStream.Close();

            return pdfServicesResponse?.Result.Asset;

        }


        return asset_;
    }
}

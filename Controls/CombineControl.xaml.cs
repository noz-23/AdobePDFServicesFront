using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.combinepdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// CombineControl.xaml の相互作用ロジック
/// </summary>
public partial class CombineControl : EventControl,IPageCount
{
    // 丸ボタン
    // https://qiita.com/rioLi0N/items/be25d8fe2d8c3b4b3c29
    public CombineControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 結合");

        InitializeComponent();
    }
    #region プロパティ
    public string CombinePath
    {
        get => _selectPdfControl.FilePath;
        set => _selectPdfControl.FilePath = value;
    }

    public int PageCount
    {
        get => _selectPdfControl.PageCount;
        set => _selectPdfControl.PageCount =value;
    }

    #endregion 

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
            return pdfServicesResponse?.Result.Asset;

        }

        return asset_;
    }
}

using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters;
using Adobe.PDFServicesSDK.pdfjobs.parameters.deletepages;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// DeleteControl.xaml の相互作用ロジック
/// </summary>
public partial class DeleteControl : EventControl,IPageCount
{
    public DeleteControl(ObservableCollection<EventControl> list_) : base(list_)
    {
        Debug.WriteLine("追加 削除");

        InitializeComponent();
    }

    #region プロパティ
    public int PageCount { get; set; } = 0;
    #endregion
    public override IAsset? EventProcess(PDFServices? pdfServices_, IAsset? asset_)
    {
        Debug.WriteLine("処理 削除");

        var list =(new HashSet<string>(_textBox.Text.Replace(" ", string.Empty).Split(','))).ToList();

        var nowPageCount = NowPageCount();

        // Delete pages of the document (as specified by PageRanges).
        //PageRanges pageRangeForDeletion = GetPageRangeForDeletion();
        // Specify order of the pages for an output document.
        var pageRanges = new PageRanges();
        PageCount = 0;
        foreach (var page in list)
        {
            // Add pages 3 to 4.
            //pageRanges.AddRange(3, 4);

            // Add page 1.

            var delPage = int.Parse(page);

            if (delPage > nowPageCount)
            {
                continue;
            }
            pageRanges.AddSinglePage(delPage);
            PageCount--;
        }

        // Create parameters for the job
        var deletePagesParams = new DeletePagesParams(pageRanges);

        // Creates a new job instance
        var deletePagesJob = new DeletePagesJob(asset_, deletePagesParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(deletePagesJob);
        var pdfServicesResponse = pdfServices_?.GetJobResult<DeletePagesResult>(location, typeof(DeletePagesResult));

        // Get content from the resulting asset(s)
        return pdfServicesResponse?.Result.Asset;
    }
}

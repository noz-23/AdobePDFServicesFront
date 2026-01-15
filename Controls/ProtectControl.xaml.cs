using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfjobs.jobs;
using Adobe.PDFServicesSDK.pdfjobs.parameters.protectpdf;
using Adobe.PDFServicesSDK.pdfjobs.results;
using AdobePDFServicesFront.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// ProtectControl.xaml の相互作用ロジック
/// </summary>
public partial class ProtectControl : EventControl
{
    public ProtectControl(IPageCount main_, ObservableCollection<EventControl> list_) : base(main_,list_)
    {
        InitializeComponent();
        //
        TitleName = "保護";
        Debug.WriteLine($"追加 [{TitleName}]");

        _encryptionComboBox.ItemsSource = Enum.GetValues<EncryptionAlgorithm>();
        _encryptionComboBox.SelectedValue=EncryptionAlgorithm.AES_256;
        _contentComboBox.ItemsSource = Enum.GetValues<ContentEncryption>();
        _contentComboBox.SelectedValue=ContentEncryption.ALL_CONTENT;
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

        // Create new permissions instance and add the required permissions
        var permissions = new Permissions();

        if (_lowPrintCheckBox.IsChecked == true)
        {
            permissions.AddPermission(Permission.PRINT_LOW_QUALITY);
        }
        if (_highPrintCheckBox.IsChecked ==true)
        {
            permissions.AddPermission(Permission.PRINT_HIGH_QUALITY);
        }
        if (_editContentCheckBox.IsChecked == true)
        {
            permissions.AddPermission(Permission.EDIT_CONTENT);
        }
        if (_editDocumentCheckBox.IsChecked == true)
        {
            permissions.AddPermission(Permission.EDIT_DOCUMENT_ASSEMBLY);
        }
        if (_editAnnotationsCheckBox.IsChecked == true)
        {
            permissions.AddPermission(Permission.EDIT_ANNOTATIONS);
        }
        if (_copyCheckBox.IsChecked == true)
        {
            permissions.AddPermission(Permission.COPY_CONTENT);
        }

        var password = _passwordTextBox.Text.Trim();
        var encryption = (_encryptionComboBox.SelectedValue is EncryptionAlgorithm en) ? en: EncryptionAlgorithm.AES_256;
        var content = (_contentComboBox.SelectedValue is  ContentEncryption con) ?con: ContentEncryption.ALL_CONTENT;

        // Create parameters for the job
        var protectPDFParams = ProtectPDFParams.PasswordProtectParamsBuilder().SetOwnerPassword(password).SetPermissions(permissions).SetEncryptionAlgorithm(encryption).SetContentEncryption(content).Build();

        // Creates a new job instance
        var protectPDFJob = new ProtectPDFJob(asset_, protectPDFParams);

        // Submits the job and gets the job result
        var location = pdfServices_?.Submit(protectPDFJob);
        var pdfServicesResponse =pdfServices_?.GetJobResult<ProtectPDFResult>(location, typeof(ProtectPDFResult));

        // Get content from the resulting asset(s)
        return pdfServicesResponse?.Result.Asset;
    }
}

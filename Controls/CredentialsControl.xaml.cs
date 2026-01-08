using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.auth;
using System.Windows.Controls;

namespace AdobePDFServicesFront.Controls;

/// <summary>
/// CredentialsControl.xaml の相互作用ロジック
/// </summary>
public partial class CredentialsControl : UserControl
{
    public CredentialsControl()
    {
        InitializeComponent();
    }

    private Adobe.PDFServicesSDK.auth.ICredentials? _credentials()
    {
        var clientID = _clientIdTextBox.Text.Trim();
        var clientSecret = _clientSecretsTextBox.Text.Trim();

        return new ServicePrincipalCredentials( clientID, clientSecret);
    }



    public PDFServices? PdfServices 
    {
        get
        {
            var credentials = _credentials();

            if (credentials == null)
            {
                return null;
            }
            return new PDFServices(credentials);
        }
    }
}

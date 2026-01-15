using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.auth;
using AdobePDFServicesFront.Jsons;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;

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
        var clientSecret = _clientSecretTextBox.Text.Trim();

        if (string.IsNullOrEmpty(clientID) == true)
        {
            return null;
        }
        if (string.IsNullOrEmpty(clientSecret) == true)
        {
            return null;
        }


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

    private void _loaded(object sender_, System.Windows.RoutedEventArgs e_)
    {
        _clientIdTextBox.Text = Properties.Settings.Default.ClientID;
        _clientSecretTextBox.Text = Properties.Settings.Default.ClientSecret;
    }

    private void _unloaded(object sender_, System.Windows.RoutedEventArgs e_)
    {
        Properties.Settings.Default.ClientID = _clientIdTextBox.Text.Trim();
        Properties.Settings.Default.ClientSecret = _clientSecretTextBox.Text.Trim();
        Properties.Settings.Default.Save();
    }

    private void _drop(object sender_, System.Windows.DragEventArgs e_)
    {
        if (e_.Data.GetData(DataFormats.FileDrop) is string[] fileList)
        {
            if(fileList.Any()==true)
            {
                Debug.WriteLine($"Drop [{fileList[0]}]");

                var jsonText = System.IO.File.ReadAllText(fileList[0]);
                var credentialsJson = JsonSerializer.Deserialize<CredentialsJson>(jsonText);

                _clientIdTextBox.Text = credentialsJson?.Client.ClientId ?? string.Empty;
                _clientSecretTextBox.Text = credentialsJson?.Client.ClientSecret ?? string.Empty;
            }
        }
    }
}

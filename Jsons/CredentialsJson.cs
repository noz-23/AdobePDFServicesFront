using System.Text.Json.Serialization;

namespace AdobePDFServicesFront.Jsons;

public class ClientCredentialsJson
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;
}

public class ServicePrincipalCredentialsJson
{
    [JsonPropertyName("organization_id")]
    public string OrganizationId { get; set; } = string.Empty;
}

public class CredentialsJson
{
    [JsonPropertyName("client_credentials")]
    public ClientCredentialsJson Client { get; set; } =new ();


    [JsonPropertyName("service_principal_credentials")]
    public ServicePrincipalCredentialsJson ServicePrincipal { get; set; } = new();
}

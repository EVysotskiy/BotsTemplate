using Newtonsoft.Json;

namespace Domain.Model;

[Serializable]
public readonly struct AuthKey
{
    [JsonProperty]
    private readonly string _jwt;

    [JsonConstructor]
    public AuthKey(string jwt) => this._jwt = "Bearer " + jwt;

    public override string ToString() => this._jwt;
}
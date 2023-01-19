using Newtonsoft.Json;

namespace WhatsappBot.Model;

[Serializable]
public class Webhook
{
    [JsonProperty("hub.mode")]
    public string Mode { get; set; }
    
    [JsonProperty("hub.challenge")]
    public int Challenge { get; set; }
    
    [JsonProperty("hub.verify_token")]
    public int Verify_token { get; set; }
    
}
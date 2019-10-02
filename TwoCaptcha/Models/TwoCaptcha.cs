namespace TwoCaptcha.Models
{
    using Newtonsoft.Json;

    public partial class TwoCaptcha
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }

        public string IdRequest { get; set; }
    }

    public partial class TwoCaptcha
    {
        public static TwoCaptcha FromJson(string json) => JsonConvert.DeserializeObject<TwoCaptcha>(json, Converter.Settings);
    }
}

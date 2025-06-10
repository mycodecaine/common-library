namespace Codecaine.Common.AiServices.OpenAi
{
    public class OpenAiSetting
    {
        public const string DefaultSectionName = "OpenAi";
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openai.com/v1";
        public int MaxTokens { get; set; } = 1000;
        public double Temperature { get; set; } = 0.7;
        public string Model { get; set; } = "text-embedding-ada-002";
    }
}

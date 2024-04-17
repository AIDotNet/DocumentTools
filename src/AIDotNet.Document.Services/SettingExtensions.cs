namespace Microsoft.Extensions.DependencyInjection;

public static class SettingExtensions
{
    private const string Prefix = "Setting:";

    public class OpenAI
    {
        public const string Default = Prefix + nameof(OpenAI);

        /// <summary>
        /// 默认提示词
        /// </summary>
        public const string ChatPrompt = Default + ":" + nameof(ChatPrompt);
    }
}
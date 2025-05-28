using System;

namespace SwissPension.WasmPrototype.Common
{
    public static class BuildConstants
    {
        private const string InjectedApiUrl = "%API_URL%";
        public static Func<string> ApiUrlResolver { get; set; } = () => InjectedApiUrl;
        public static string ApiUrl => ApiUrlResolver();
    }
}
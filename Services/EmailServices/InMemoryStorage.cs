namespace ZahimarProject.Services.EmailServices
{
    public static class InMemoryStorage
    {
        private static Dictionary<string, string> verificationCodes = new Dictionary<string, string>();

        public static void StoreVerificationCode(string email, string code)
        {
            verificationCodes[email] = code;
        }

        public static string GetVerificationCode(string email)
        {
            verificationCodes.TryGetValue(email, out var code);
            return code;
        }

        public static void RemoveVerificationCode(string email)
        {
            verificationCodes.Remove(email);
        }
    }

}

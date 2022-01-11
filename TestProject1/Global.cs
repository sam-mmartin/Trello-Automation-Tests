using OpenQA.Selenium;

namespace TestProject1
{
    public class Global
    {
        public static IWebDriver driver;
        public static string binPath = @System.Environment.CurrentDirectory.ToString();
        public static bool OsLinux { get; internal set; }
        public static Selenium selenium;
        public static Business bus;
    }
}

using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Global.selenium = new Selenium();
            Global.bus = new Business();

            string chromeDriverPath;
            var options = new ChromeOptions();
            var os = Environment.OSVersion;
            Global.OsLinux = os.Platform == PlatformID.Unix;
            ChromeDriverService service;

            if (Global.OsLinux)
            {
                chromeDriverPath = "/usr/bin/";
                service = ChromeDriverService.CreateDefaultService(chromeDriverPath, "chromedriver");
            }
            else
            {
                chromeDriverPath = Global.binPath;
                service = ChromeDriverService.CreateDefaultService(chromeDriverPath, "chromedriver.exe");
            }

            //options.AddArgument("--headless");
            options.AddArgument("--incognito");
            options.AddArgument("--window-size=1366,768");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--ignore-certificate-errors-spki-list");
            options.AddArgument("use-fake-ui-for-media-stream");
            //options.AddArgument("--start-maximized");
            //options.AddArgument("--touch-events=enabled");
            options.AcceptInsecureCertificates = true;

            Global.driver = new ChromeDriver(service, options);
            Global.selenium.Navigate(Global.driver, "https://www.trello.com/");
        }

        [Test]
        public void Test()
        {
            var listName = "Teste-Criar-Lista";
            var cardName = "Teste-Criar-Cartao";

            Global.bus.Login("smeirelesmartins@gmail.com", "Lord0101#");
            Global.bus.CriarQuadro("Teste-Criar-Quadro");

            for (int i = 0; i < 3; i++)
            {
                Global.bus.CriarLista($"{listName}-{i}");

                for (int j = 0; j < 3; j++)
                {
                    Global.bus.CriarCartao($"{listName}-{i}", $"{cardName}-{j}");
                    Global.bus.AdicionarEtiqueta($"{listName}-{i}", $"{cardName}-{j}");
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Global.bus.RemoverEtiqueta($"{listName}-{i}", $"{ cardName}-{j}");
                    Global.bus.ExcluirCartao($"{listName}-{i}", $"{cardName}-{j}");
                }

                Global.bus.ExcluirLista($"{listName}-{i}");
            }

            /*Global.bus.CriarLista(listName);
            Global.bus.CriarCartao(listName, cardName);
            Global.bus.AdicionarEtiqueta(listName, cardName);

            Global.bus.RemoverEtiqueta(listName, cardName);

            Global.bus.ExcluirCartao(listName, cardName);
            Global.bus.ExcluirLista(listName);*/
            Global.bus.ExcluirQuadro("Teste-Criar-Quadro");
        }

        [TearDown]
        public void TearDown()
        {
            Global.driver.Quit();
        }
    }
}
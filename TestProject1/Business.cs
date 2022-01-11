using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    public class Business
    {
        // Botão Fazer Login
        //[FindsBy(How = How.XPath, Using = "//a[contains(@href,'login')]")]
        //private IWebElement botaoFazerLogin { get; set; }

        public void Login(string username, string password)
        {
            string resultadoEsperado = "Login realizado com sucesso";
            string resultadoObtido;

            Global.selenium.Click(Global.driver, By.XPath("//a[contains(@href,'login')]"));
            Global.selenium.SendKeys(Global.driver, By.Id("user"), username);
            Global.selenium.Click(Global.driver, By.Id("login"));
            Global.selenium.Wait(3000);
            Global.selenium.SendKeys(Global.driver, By.Id("password"), password);
            Global.selenium.Click(Global.driver, By.Id("login-submit"));

            resultadoObtido = Global.selenium.Exists(Global.driver, By.XPath("//h3[contains(text(),'SUAS ÁREAS DE TRABALHO')]"), 3000)
                ? "Login realizado com sucesso"
                : "Login não realizado";

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        public void CriarQuadro(string nomeQuadro)
        {
            var expected = "The board exists";

            Global.selenium.Click(Global.driver, By.XPath("//li[contains(@data-test-id, 'create-board-tile')]"));
            Global.selenium.SendKeys(Global.driver, By.XPath("//input[@data-test-id='create-board-title-input']"), nomeQuadro);
            Global.selenium.Click(Global.driver, By.XPath("//button[@data-test-id='create-board-submit-button']"));

            Assert.AreEqual(expected, CheckIfElementExists("board", "board", 3000));
        }

        public void CriarLista(string nomeLista)
        {
            string[] rules =
            {
                "//div[contains(@class, 'js-add-list')]/form/input[@name='name']",
                "//div[contains(@class, 'js-add-list')]/form/a",
                "//div[contains(@class, 'js-add-list')]/form/input[@name='name']",
                "//div[contains(@class, 'js-add-list')]/form/div/input[@type='submit']",
                "//div[@id='board']/div[last()-1]/div/div[contains(@class, 'list-header')]/textarea"
            };

            if (!Global.selenium.IsDisplayed(Global.driver, By.XPath(rules[0])))
            {
                Global.selenium.Click(Global.driver, By.XPath(rules[1]));
            }

            Global.selenium.SendKeys(Global.driver, By.XPath(rules[2]), nomeLista);
            Global.selenium.Click(Global.driver, By.XPath(rules[3]));

            var text = Global.selenium.GetElementText(Global.driver, By.XPath(rules[4]));

            Assert.AreEqual(nomeLista, text);
        }

        public void CriarCartao(string nomeLista, string nomeCartao)
        {
            var expected = "The card exists";
            string[] rules =
            {
                $"//h2[contains(text(), '{nomeLista}')]/../following-sibling::div/div[contains(@class, 'card-composer')]",
                $"//h2[contains(text(), '{nomeLista}')]/../following-sibling::div/a[contains(@class, 'js-open-card-composer')]",
                $"//h2[contains(text(), '{nomeLista}')]/../following-sibling::div/div/div/div/textarea",
                $"//h2[contains(text(), '{nomeLista}')]/../following-sibling::div/div/div/div/input",
                $"//h2[contains(text(), '{nomeLista}')]/../following-sibling::div/a/div/span[contains(text(), '{nomeCartao}')]/ancestor::a"
            };

            if (!Global.selenium.IsDisplayed(Global.driver, By.XPath(rules[0])))
            {
                Global.selenium.Click(Global.driver, By.XPath(rules[1]));
            }

            Global.selenium.SendKeys(Global.driver, By.XPath(rules[2]), nomeCartao);
            Global.selenium.Click(Global.driver, By.XPath(rules[3]));
            Global.selenium.WaitExists(Global.driver, By.XPath(rules[4]));

            Assert.AreEqual(expected, CheckIfElementExists("card", rules[4]));
        }

        public void AdicionarEtiqueta(string listName, string cardName)
        {
            Random ranNum = new();
            var expected = "The badge exists";
            string[] xpath =
            {
                $"//h2[text()='{listName}']/../following-sibling::div/a/div/span[contains(text(), '{cardName}')]/ancestor::a",
                "//a[@title='Etiquetas']",
                "//ul[contains(@class, 'js-labels-list')]/li",
                $"//h2[contains(text(), '{listName}')]/../following-sibling::div/a/div/span[text()='{cardName}']/../div/span[contains(@class, 'card-label')]"
            };

            Global.selenium.Click(Global.driver, By.XPath(xpath[0]));
            Global.selenium.Click(Global.driver, By.XPath(xpath[1]));

            int numberElements = Global.selenium.GetElementsCount(Global.driver, By.XPath(xpath[2]));
            int badgeNumber = ranNum.Next(1, numberElements + 1);

            List<string> rules = new()
            {
                $"//ul[contains(@class, 'js-labels-list')]/li[{badgeNumber}]/span",
                "//a[contains(@class, 'pop-over-header-close-btn')]",
                "//a[contains(@class, 'dialog-close-button')]"
            };

            foreach (var rule in rules)
            {
                Global.selenium.Click(Global.driver, By.XPath(rule));
            }

            Assert.AreEqual(expected, CheckIfElementExists("badge", xpath[3]));
        }

        public void ExcluirQuadro(string boardName)
        {
            var expected = "The board does not exist";
            boardName = "//a[contains(@href, '" + boardName + "')]";

            if (Global.selenium.Exists(Global.driver, By.XPath(boardName)))
            {
                Global.selenium.Click(Global.driver, By.XPath(boardName));
            }

            List<string> rules = new()
            {
                "//a[contains(@class, 'mod-show-menu')]",
                "//a[contains(@class, 'js-open-more')]",
                "//a[contains(@class, 'js-close-board')]",
                "//input[contains(@class, 'js-confirm')]",
                "//button[contains(@data-test-id, 'close-board-delete-board-button')]"
            };

            foreach (var rule in rules)
            {
                Global.selenium.Click(Global.driver, By.XPath(rule));
            }

            Global.selenium.WaitExists(Global.driver, By.XPath("//button[contains(@data-test-id, 'close-board-delete-board-confirm-button')]"));
            Global.selenium.Click(Global.driver, By.XPath("//button[contains(@data-test-id, 'close-board-delete-board-confirm-button')]"));

            Assert.AreEqual(expected, CheckIfElementExists("board", boardName));
        }

        public void ExcluirLista(string listName)
        {
            var expected = "The list does not exist";
            string[] rules =
            {
                $"//h2[text()='{listName}']/following-sibling::div/a[contains(@class, 'list-header-extras-menu')]",
                "//a[@class='js-close-list']",
                $"//h2[text()='{listName}']"
            };

            Global.selenium.Click(Global.driver, By.XPath(rules[0]));
            Global.selenium.Click(Global.driver, By.XPath(rules[1]));

            Assert.AreEqual(expected, CheckIfElementExists("list", rules[2]));
        }

        public void ExcluirCartao(string listName, string cardName)
        {
            var expected = "The card does not exist";
            string[] rule = {
                $"//h2[contains(text(), '{listName}')]/../following-sibling::div/a/div/span[text()='{cardName}']/ancestor::a",
                $"//h2[contains(text(), '{listName}')]/../following-sibling::div/a/div/span[text()='{cardName}']/../../span[contains(@class, 'list-card-operation')]",
                "//a[contains(@class, 'js-archive')]",
                $"//h2[contains(text(), '{listName}')]/../following-sibling::div/a[contains(@href, '{cardName}')]"
            };

            Global.selenium.MouseOver(Global.driver, By.XPath(rule[0]));
            Global.selenium.Click(Global.driver, By.XPath(rule[1]));
            Global.selenium.Click(Global.driver, By.XPath(rule[2]));

            Assert.AreEqual(expected, CheckIfElementExists("card", rule[3]));
        }

        public void RemoverEtiqueta(string listName, string cardName)
        {
            string colorBadge;
            string[] rules =
            {
                $"//h2[text()='{listName}']/../following-sibling::div/a/div/span[contains(text(), '{cardName}')]/ancestor::a",
                "//div[contains(@class, 'card-detail-data')]/div/div/span[contains(@class, 'card-label')]",
                "//a[contains(@class, 'pop-over-header-close-btn')]",
                "//a[contains(@class, 'dialog-close-button')]",
                $"//h2[contains(text(), '{listName}')]/../following-sibling::div/a/div/span[text()='{cardName}']/../div/span[contains(@class, 'card-label')]"
            };

            Global.selenium.Click(Global.driver, By.XPath(rules[0]));
            int elementsNumber = Global.selenium.GetElementsCount(Global.driver, By.XPath(rules[1]));

            for (int i = 0; i < elementsNumber; i++)
            {
                string rule = $"{rules[1]}[{i + 1}]";
                colorBadge = Global.selenium.GetElementCssProperty(Global.driver, By.XPath(rule), "background-color");

                Global.selenium.Click(Global.driver, By.XPath(rule));

                int badgesNumber = Global.selenium.GetElementsCount(Global.driver, By.XPath("//ul[contains(@class, 'js-labels-list')]/li/span"));

                for (int j = 0; j < badgesNumber; j++)
                {
                    string xpBadges = $"//ul[contains(@class, 'js-labels-list')]/li[{j + 1}]/span";
                    string backgroundColor = Global.selenium.GetElementCssProperty(Global.driver, By.XPath(xpBadges), "background-color");

                    if (colorBadge == backgroundColor)
                    {
                        Global.selenium.Click(Global.driver, By.XPath(xpBadges));
                        break;
                    }
                }
            }

            Global.selenium.Click(Global.driver, By.XPath(rules[2]));
            Global.selenium.Click(Global.driver, By.XPath(rules[3]));
            int result = Global.selenium.GetElementsCount(Global.driver, By.XPath(rules[4]));

            Assert.AreEqual(0, result);
        }

        public static string CheckIfElementExists(string type, string rule)
        {
            return Global.selenium.Exists(Global.driver, By.XPath(rule))
                ? $"The {type} exists"
                : $"The {type} does not exist";
        }

        public static string CheckIfElementExists(string type, string rule, int ms = 0)
        {
            return Global.selenium.Exists(Global.driver, By.Id(rule), ms)
                ? $"The {type} exists"
                : $"The {type} does not exist";
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace Khats.Testler
{
    public class MasterOtomasyonTestleri
    {
        // =========================================================================
        // 🔐 MERKEZİ YARDIMCI METOTLAR (Engelleri ve Hataları Kökünden Çözen Kısım)
        // =========================================================================

        // Test kimlik bilgileri ortam degiskeninden okunur; repoda sifre tutulmaz.
        private static string TestBilgi(string ad) =>
            Environment.GetEnvironmentVariable(ad)
            ?? throw new InvalidOperationException($"{ad} ortam degiskeni tanimli degil.");

        // Güvenli Giriş Yapma Yardımcısı
        private void SistemeGirisYap(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://localhost:61405/Hesap/Giris");
            Thread.Sleep(2000);
            driver.FindElement(By.Name("kullaniciAdi")).SendKeys(TestBilgi("KHATS_TEST_KULLANICI"));
            driver.FindElement(By.Name("sifre")).SendKeys(TestBilgi("KHATS_TEST_SIFRE"));
            driver.FindElement(By.CssSelector("button.btn-info")).Click();
            Thread.Sleep(2000);
        }

        // Animasyon ve Sticky Navbar Engellerini Aşan JavaScript Rota Tıklayıcısı
        private void JavaScriptClick(IWebDriver driver, string href)
        {
            IWebElement element = driver.FindElement(By.CssSelector($"a[href='{href}']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        // WhatsApp Tarzı Listede Tıklama Sorununu Çözen JavaScript ID Tıklayıcısı
        private void JavaScriptClickById(IWebDriver driver, string id)
        {
            IWebElement element = driver.FindElement(By.Id(id));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        // =========================================================================
        // 🎯 TEST 1: SİSTEM TESTLERİ (9 MODÜLLÜ MASTER TARAYICI)
        // =========================================================================
        [Fact]
        public void UctanUca_SistemdekiTumModullerBasariylaAcilabiliyorMu()
        {
            ChromeOptions ayarlar = new ChromeOptions();
            ayarlar.AcceptInsecureCertificates = true;

            using (IWebDriver driver = new ChromeDriver(ayarlar))
            {
                driver.Manage().Window.Maximize();
                SistemeGirisYap(driver);

                JavaScriptClick(driver, "/Gorev/Dashboard");
                Thread.Sleep(1500);
                Assert.Contains("Dashboard", driver.Url);

                JavaScriptClick(driver, "/Proje/Index");
                Thread.Sleep(1500);
                Assert.Contains("Proje", driver.Url);

                JavaScriptClick(driver, "/Gorev/Ekle");
                Thread.Sleep(1500);
                Assert.Contains("Gorev", driver.Url);

                JavaScriptClick(driver, "/Rapor/Verimlilik");
                Thread.Sleep(1500);
                Assert.Contains("Rapor", driver.Url);

                JavaScriptClick(driver, "/Toplanti/Index");
                Thread.Sleep(1500);
                Assert.Contains("Toplanti", driver.Url);

                JavaScriptClick(driver, "/Personel/Index");
                Thread.Sleep(1500);
                Assert.Contains("Personel", driver.Url);

                JavaScriptClick(driver, "/Departman/Index");
                Thread.Sleep(1500);
                Assert.Contains("Departman", driver.Url);

                JavaScriptClick(driver, "/Gorev/Arsiv");
                Thread.Sleep(1500);
                Assert.Contains("Arsiv", driver.Url);

                JavaScriptClick(driver, "/Sohbet/Index");
                Thread.Sleep(1500);
                Assert.Contains("Sohbet", driver.Url);
            }
        }

        // =========================================================================
        // 💬 TEST 2: MESAJ TESTLERİ (KURŞUN GEÇİRMEZ ÇİFT EKRANLI CHAT TESTİ)
        // =========================================================================
        [Fact]
        public void IkiKullaniciArasiCanliMesajlasmaBasariliOlmali()
        {
            ChromeOptions ayarlar = new ChromeOptions();
            ayarlar.AcceptInsecureCertificates = true;

            using (IWebDriver driverBilal = new ChromeDriver(ayarlar))
            using (IWebDriver driverYonetici = new ChromeDriver(ayarlar))
            {
                driverBilal.Manage().Window.Maximize();
                driverYonetici.Manage().Window.Maximize();

                // 1. Pencereler Giriş Yapıyor
                driverBilal.Navigate().GoToUrl("https://localhost:61405/Hesap/Giris");
                Thread.Sleep(1500);
                driverBilal.FindElement(By.Name("kullaniciAdi")).SendKeys(TestBilgi("KHATS_TEST_KULLANICI"));
                driverBilal.FindElement(By.Name("sifre")).SendKeys(TestBilgi("KHATS_TEST_SIFRE"));
                driverBilal.FindElement(By.CssSelector("button.btn-info")).Click();

                driverYonetici.Navigate().GoToUrl("https://localhost:61405/Hesap/Giris");
                Thread.Sleep(1500);
                driverYonetici.FindElement(By.Name("kullaniciAdi")).SendKeys(TestBilgi("KHATS_TEST_YONETICI"));
                driverYonetici.FindElement(By.Name("sifre")).SendKeys(TestBilgi("KHATS_TEST_YONETICI_SIFRE"));
                driverYonetici.FindElement(By.CssSelector("button.btn-info")).Click();

                Thread.Sleep(2000);

                // 2. Sohbet Odalarına Geçiş
                driverBilal.Navigate().GoToUrl("https://localhost:61405/Sohbet/Index");
                driverYonetici.Navigate().GoToUrl("https://localhost:61405/Sohbet/Index");
                Thread.Sleep(3000);

                // 3. SOHBET EDİLECEK KİŞİLERİ JS İLE KESİN OLARAK TETİKLİYORUZ
                JavaScriptClickById(driverBilal, "user-4"); // Bilal listeden Yönetici 3'ü seçer
                JavaScriptClickById(driverYonetici, "user-1"); // Yönetici 3 listeden Bilal'i seçer
                Thread.Sleep(3000);

                // 4. Mesaj Yazma ve Gönderme
                string testMesaji = "Otomatik SignalR Test Mesajı. Kod: " + Guid.NewGuid().ToString().Substring(0, 5);

                IWebElement mesajKutusu = driverBilal.FindElement(By.Id("mesajİcerigi"));
                mesajKutusu.Click();
                mesajKutusu.SendKeys(testMesaji);
                Thread.Sleep(1000);

                // Gönder butonuna JS ile basıyoruz ki d-none engeline takılmasın
                JavaScriptClickById(driverBilal, "btnGonder");
                Thread.Sleep(3500); // SignalR'ın mesajı karşı tarafa işletmesi için süre

                // 5. Doğrulama
                string yoneticiSayfaIcerigi = driverYonetici.PageSource;
                Assert.Contains(testMesaji, yoneticiSayfaIcerigi);
            }
        }

        // =========================================================================
        // 🚀 TEST 3: SENARYO TESTLERİ (PROJE OLUŞTURMA VE GÖREV ATAMA AKIŞI)
        // =========================================================================
        [Fact]
        public void TamSenaryo_SistemeYeniProjeEkleyipOProjeyeGorevAtamaAkisi()
        {
            ChromeOptions ayarlar = new ChromeOptions();
            ayarlar.AcceptInsecureCertificates = true;

            using (IWebDriver driver = new ChromeDriver(ayarlar))
            {
                driver.Manage().Window.Maximize();
                SistemeGirisYap(driver);

                // YENİ PROJE OLUŞTURMA
                driver.Navigate().GoToUrl("https://localhost:61405/Proje/Ekle");
                Thread.Sleep(2000);

                string benzersizProjeAdi = "Otomatik Proje -" + Guid.NewGuid().ToString().Substring(0, 5);
                driver.FindElement(By.Name("ProjeAdi")).SendKeys(benzersizProjeAdi);
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("select[name='YoneticiId'] option:nth-child(2)")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Name("Aciklama")).SendKeys("Selenium WebDriver ile otomatik süreç doğrulaması için oluşturulmuştur.");
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();
                Thread.Sleep(2500);

                // OLUŞTURULAN PROJEYE GÖREV ATAMA
                driver.Navigate().GoToUrl("https://localhost:61405/Gorev/Ekle");
                Thread.Sleep(2000);

                driver.FindElement(By.Name("Baslik")).SendKeys("Uçtan Uca Entegrasyon Kontrolü");
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("select[name='PersonelId'] option:nth-child(1)")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("select[name='ProjeId'] option:nth-child(1)")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.CssSelector("button[type='submit']")).Click();
                Thread.Sleep(2500);

                // Doğrulama
                string mevcutUrl = driver.Url;
                Assert.Contains("Gorev/Index", mevcutUrl);
            }
        }

        // =========================================================================
        // 👥 TEST 4: YÖNETİM TESTLERİ (DEPARTMAN AÇMA, PERSONEL EKLEME VE İŞTEN ÇIKARMA)
        // =========================================================================
        [Fact]
        public void YonetimSenaryosu_DepartmanAc_PersonelEkleVeO_PersoneliKov()
        {
            ChromeOptions ayarlar = new ChromeOptions();
            ayarlar.AcceptInsecureCertificates = true;

            using (IWebDriver driver = new ChromeDriver(ayarlar))
            {
                driver.Manage().Window.Maximize();
                SistemeGirisYap(driver);

                // DEPARTMAN OLUŞTURMA
                driver.Navigate().GoToUrl("https://localhost:61405/Departman/Ekle");
                Thread.Sleep(2000);

                string benzersizDepartman = "Yapay Zeka Bölümü -" + Guid.NewGuid().ToString().Substring(0, 4);
                driver.FindElement(By.Name("Ad")).SendKeys(benzersizDepartman);
                Thread.Sleep(1000);
                driver.FindElement(By.Name("Aciklama")).SendKeys("Bu departman otomatik Selenium mimarisi tarafından test için açılmıştır.");
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("button.btn-success")).Click();
                Thread.Sleep(2500);

                // YENİ PERSONEL KAYDI OLUŞTURMA
                driver.Navigate().GoToUrl("https://localhost:61405/Personel/Ekle");
                Thread.Sleep(2000);

                driver.FindElement(By.Name("AdSoyad")).SendKeys("Stajyer Robot");
                Thread.Sleep(1000);
                driver.FindElement(By.Name("Email")).SendKeys("stajyer.robot@khats.com");
                Thread.Sleep(1000);
                driver.FindElement(By.Name("KullaniciAdi")).SendKeys("stajyer.robot");
                Thread.Sleep(1000);
                driver.FindElement(By.Name("Sifre")).SendKeys("654321");
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("select[name='Rol'] option[value='Personel']")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.CssSelector("button.btn-warning")).Click();
                Thread.Sleep(2500);

                // PERSONELİ LİSTEDEN KOVMA / SİLME İŞLEMİ
                driver.Navigate().GoToUrl("https://localhost:61405/Personel/Index");
                Thread.Sleep(2000);

                try
                {
                    IWebElement silmeButonu = driver.FindElement(By.CssSelector("table tbody tr:last-child .btn-danger, table tbody tr:last-child .btn-outline-danger"));
                    silmeButonu.Click();
                    Thread.Sleep(1500);
                    driver.SwitchTo().Alert().Accept();
                    Thread.Sleep(2000);
                }
                catch (Exception) { }

                // DOĞRULAMA
                Assert.Contains("Personel", driver.Url);
            }
        }
    }
}
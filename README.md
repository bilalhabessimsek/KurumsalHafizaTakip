# 🏢 KHATS - Kurumsal Hafıza ve Takip Sistemi

![.NET Core](https://img.shields.io/badge/.NET%208.0-Purple?style=for-the-badge&logo=dotnet)
![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=microsoft)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=dotnet)
![Selenium](https://img.shields.io/badge/Selenium-43B02A?style=for-the-badge&logo=selenium)
![xUnit](https://img.shields.io/badge/xUnit-Tested-blue?style=for-the-badge)

**KHATS (Kurumsal Hafıza Takip Sistemi)**, kurumsal organizasyonlardaki bilgi kaybını önlemek, proje ve görev yönetimini tek merkezden yürütmek, anlık iletişimi sağlamak ve personel performansını somut metriklerle analiz etmek amacıyla geliştirilmiş kurumsal bir **ASP.NET Core 8.0 MVC** platformudur.

---

## ✨ Temel Özellikler

* **⚡ Gerçek Zamanlı İletişim (SignalR):** Çalışanlar arasında WebSocket tabanlı uçtan uca canlı mesajlaşma, dosya/belge aktarımı ve anlık bildirim kanalı.
* **🔔 Dinamik Bildirim & Oturum Yönetimi:** Yeni görev tanımlandığında veya proje atandığında tetiklenen Toast bildirimleri ve yetki iptalinde istemci tarafını anında yönlendiren oturum sonlandırma sinyali (`OturumuKapat`).
* **⚖️ Hiyerarşik Görev Akışı:** Personelin işi tamamlayıp onaya göndermesi, yöneticinin görevi onaylaması veya revize notu düşerek reddetmesi süreçlerini kapsayan iki aşamalı karar mekanizması[cite: 9].
* **📜 Kurumsal Hafıza & Notlandırma:** Görev bazlı kronolojik revize geçmişi ve not akışı sayesinde projelerin karar adımlarının kayıt altına alınması[cite: 9].
* **📊 Ağırlık Puanlı Verimlilik Raporlaması:** Görev zorluk puanları (100, 250, 500) üzerinden çalışanların tamamladığı işlere göre haftalık/aylık performans hesaplama ve **Excel (CSV)** çıktısı alma[cite: 9].
* **📝 Toplantı Tutanakları & Arşiv:** Kurumsal toplantı kararlarının dijitalleştirilmesi, imzalı PDF/belge yüklemeleri ve tamamlanan/iptal edilen projelerin saklandığı güvenli arşiv alanı[cite: 9].
* **🤖 Uçtan Uca E2E Otomasyon Testleri:** `Khats.Testler` projesi altında xUnit ve Selenium WebDriver ile yazılmış 4 ana senaryodan oluşan otomatik tarayıcı testleri[cite: 9].

---

## 🛠️ Teknoloji Yığını & Mimari

* **Framework:** ASP.NET Core 8.0 MVC[cite: 9]
* **Veritabanı & ORM:** Microsoft SQL Server, Entity Framework Core (Code-First)[cite: 9]
* **Canlı Veri İletişimi:** Microsoft SignalR Hubs[cite: 9]
* **Arayüz:** HTML5, CSS3, Bootstrap 5 (Dark Theme), JavaScript, jQuery, AJAX[cite: 9]
* **Test Altyapısı:** xUnit, Selenium WebDriver, ChromeDriver[cite: 9]
* **Yetkilendirme:** Cookie Authentication tabanlı Rol Bazlı Erişim Kontrolü (RBAC)[cite: 9]

---

## 👥 Kullanıcı Rolleri ve Yetki Matrisi

Sistem 3 temel rol hiyerarşisine göre çalışır[cite: 9]:

| Yetki / Modül | Admin (Sistem Yöneticisi) | Görev Sorumlusu (Yönetici) | Saha Personeli |
| :--- | :---: | :---: | :---: |
| **Özet Paneli (Dashboard)** | Şirket Geneli[cite: 9] | Sorumlu Olunan Projeler[cite: 9] | Kendi Görevleri[cite: 9] |
| **Proje & Ekip Yönetimi** | Tam Yetki[cite: 9] | Sorumlu Olduğu Projeler[cite: 9] | ❌ |
| **Görev Atama & Puanlama** | Tam Yetki[cite: 9] | Ekibine Görev Atama[cite: 9] | ❌ |
| **Hiyerarşik Onay/Ret** | Tam Yetki[cite: 9] | Yönetici Onayı[cite: 9] | Onaya Gönderme[cite: 9] |
| **Performans & CSV Raporu** | Tam Yetki[cite: 9] | Ekip Raporları[cite: 9] | ❌ |
| **Departman & Personel** | Tam Yetki[cite: 9] | ❌ | ❌ |
| **Canlı Sohbet & Arşiv** | Aktif[cite: 9] | Aktif[cite: 9] | Aktif[cite: 9] |

---

## 🧪 Otomasyon Testleri (Selenium WebDriver)

`Khats.Testler/UnitTest1.cs` dosyası üzerinden yürütülen ana test senaryoları[cite: 9]:

1. **Uçtan Uca Modül Doğrulaması:** 9 farklı sistem rotasının (Dashboard, Projeler, Görevler, Raporlama, Tutanaklar, Personel, Departman, Arşiv, Sohbet) kesintisiz yüklendiğini denetler[cite: 9].
2. **Çift Pencereli SignalR Testi:** İki farklı ChromeDriver örneği açarak kullanıcılar arasında anlık mesaj iletimini doğrular[cite: 9].
3. **Proje & Görev Döngüsü Testi:** Dinamik proje kaydı oluşturup ilgili projeye ağırlık puanlı görev bağlama akışını simüle eder[cite: 9].
4. **Yönetim Akışı Testi:** Otomatik departman açma, personel tanımlama ve personel yetkisini sonlandırıp erişim kontrolünü doğrulama sürecini yürütür[cite: 9].

---

## 📸 Ekran Görüntüleri

### 1. Rol Tabanlı Yetkilendirme (RBAC) & Dashboard

| Sistem Yöneticisi (Admin) Paneli | Saha Personeli Paneli |
| :---: | :---: |
| <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20191745.jpg" width="100%" alt="Admin Paneli"/> | <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20190452%20(1).jpg" width="100%" alt="Personel Paneli"/> |
| *Tüm departman, proje, tutanak ve personel verilerini kapsayan tam yetkili panel.*[cite: 9] | *Yalnızca ilgili personele atanan işleri barındıran kısıtlı görünüm.*[cite: 9] |

---

### 2. Canlı İletişim (SignalR) & Görev Tanımlama

| Gerçek Zamanlı Sohbet & Toast Bildirimleri | Ağırlık Puanlı Görev Atama |
| :---: | :---: |
| <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20191537.jpg" width="100%" alt="Canlı Sohbet"/> | <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20191934.png" width="100%" alt="Görev Atama"/> |
| *WebSocket tabanlı uçtan uca anlık mesajlaşma ve sistem içi olay bildirimleri.*[cite: 9] | *Zorluk derecesine (100, 250, 500) göre personele iş atama penceresi.*[cite: 9] |

---

### 3. Hiyerarşik Onay & Kurumsal Hafıza

| Aktif Görev Takibi & Ekip Yönetimi | Onay Süreci & Karar Geçmişi |
| :---: | :---: |
| <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20205815.png" width="100%" alt="Görev Detay"/> | <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20190443.png" width="100%" alt="Onay Süreci"/> |
| *Görev geçmişine dair revize notları ve projeye bağlı dinamik ekip üyeleri.*[cite: 9] | *İki kademeli (Personel onaya sunar -> Yönetici onaylar/reddeder) onay mekanizması.*[cite: 9] |

---

### 4. Departman Yönetimi & Kurumsal Arşiv

| Departman İşlem Merkezi | Kurumsal Hafıza Arşivi |
| :---: | :---: |
| <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20192031.png" width="100%" alt="Departman Yönetimi"/> | <img src="docs/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202026-09-11%20192052.jpg" width="100%" alt="Arşiv"/> |
| *Departman bazlı hızlı personel ekleme, görev atama ve silme aksiyonları.*[cite: 9] | *İptal edilen veya tamamlanan proje ve görevlerin kayıt altına alındığı arşiv.*[cite: 9] |

---

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel geliştirme ortamınızda çalıştırmak için:

1. Depoyu klonlayın:
   ```bash
   git clone [https://github.com/KullaniciAdin/Kurumsal-Hafiza-Takip-Sistemi.git](https://github.com/KullaniciAdin/Kurumsal-Hafiza-Takip-Sistemi.git)
   cd Kurumsal-Hafiza-Takip-Sistemi

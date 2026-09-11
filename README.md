# 🏢 KHATS - Kurumsal Hafıza ve Takip Sistemi

![.NET Core](https://img.shields.io/badge/.NET%208.0-Purple?style=for-the-badge&logo=dotnet)
![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=microsoft)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=dotnet)
![Selenium](https://img.shields.io/badge/Selenium-43B02A?style=for-the-badge&logo=selenium)

**KHATS (Kurumsal Hafıza Takip Sistemi)**, şirket içi iletişimi, proje ve görev yönetimini, personel performansını ve kurumsal arşivlemeyi tek bir çatı altında toplayan kapsamlı bir **ASP.NET Core MVC** otomasyonudur.

Şirket içindeki bilgi kayıplarını önlemek, süreçleri dijitalleştirmek ve çalışan verimliliğini somut verilerle (görev ağırlık puanları) ölçmek amacıyla geliştirilmiştir.

## ✨ Temel Özellikler

- **⚡ Gerçek Zamanlı Mesajlaşma (SignalR):** Çalışanlar arası WhatsApp tarzı uçtan uca anlık mesajlaşma ve dosya/belge paylaşımı. Sayfa yenilemeye gerek kalmadan mesajlar anında iletilir.
- **🔔 Dinamik Bildirim Sistemi:** Yeni görev atamaları, proje güncellemeleri veya mesajlar geldiğinde ekranın sağ üst köşesinde beliren (Toast) ve menüden takip edilebilen anlık bildirim altyapısı.
- **🎯 Görev ve Proje Yönetimi:** Proje oluşturma, ekibe personel atama, görev ağırlık puanı (basit/orta/zor) belirleme ve hiyerarşik (Onay Bekliyor -> Onaylandı) iş akışı yönetimi.
- **📊 Performans ve Verimlilik Raporlaması:** Personelin tamamladığı görevlerin ağırlık puanlarına göre verimlilik yüzdesi hesaplama. "Fazla Mesai" ve "Performans Durumu" analizlerini tarih bazlı süzerek **Excel (CSV)** formatında dışa aktarma.
- **📝 Toplantı Tutanakları ve Kurumsal Arşiv:** Gerçekleşen toplantıların kararlarını kayıt altına alma, imzalı PDF/Görsel dosyalarını sisteme yükleme ve tamamlanan/iptal edilen projeleri güvenli arşive kaldırma.
- **🤖 Uçtan Uca Otomasyon Testleri (E2E):** Selenium WebDriver ve xUnit kullanılarak yazılmış, sistemdeki tüm modüllerin erişilebilirliğini ve SignalR anlık mesajlaşma senaryolarını test eden otomatik test mimarisi.

## 🛠️ Kullanılan Teknolojiler & Mimari

- **Backend:** C#, ASP.NET Core 8.0 MVC
- **Veritabanı & ORM:** Microsoft SQL Server, Entity Framework Core (Code-First Yaklaşımı)
- **Gerçek Zamanlı İletişim:** Microsoft SignalR (Hub mimarisi)
- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript, jQuery, AJAX
- **Test Otomasyonu:** xUnit, Selenium WebDriver
- **Mimari Desenler:** Repository, Dependency Injection (DI)

## 👥 Kullanıcı Rolleri ve Yetkilendirme

Sistemde güvenliği ve veri erişimini yönetmek için Cookie tabanlı kimlik doğrulama ve 3 farklı rol yapısı kullanılmıştır:

1. **Admin (Sistem Yöneticisi):** Departman açma, sistemden personel çıkarma, tüm projeleri ve performans raporlarını görme yetkisi.
2. **Görev Sorumlusu (Yönetici):** Kendi sorumlu olduğu projelere ekip atama, personellere görev yazma ve biten görevleri onaylama/reddetme yetkisi.
3. **Personel (Çalışan):** Yalnızca kendisine atanan görevleri görme, görev notları ekleme, işi bitirip onaya sunma ve diğer çalışanlarla canlı sohbet etme yetkisi.

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1. Projeyi klonlayın:
   ```bash
   git clone [https://github.com/KullaniciAdin/Kurumsal-Hafiza-Takip-Sistemi.git](https://github.com/KullaniciAdin/Kurumsal-Hafiza-Takip-Sistemi.git)

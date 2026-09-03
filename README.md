<div align="center">

# 📱 .NET MAUI & Firebase Entegreli Çok Fonksiyonlu Mobil Uygulama
### Multi-Page Mobile Application with .NET MAUI 9.0 & Firebase

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-512BD4.svg)](https://learn.microsoft.com/en-us/dotnet/maui/)
[![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20Windows-brightgreen.svg)](https://dotnet.microsoft.com/)
[![Firebase Auth](https://img.shields.io/badge/Firebase-Authentication-FFCA28.svg?logo=firebase&logoColor=black)](https://firebase.google.com/)
[![Firebase Realtime](https://img.shields.io/badge/Firebase-Realtime%20Database-FFA000.svg?logo=firebase&logoColor=black)](https://firebase.google.com/)
[![Theme](https://img.shields.io/badge/Theme-Dark%20%2F%20Light%20Mode-blueviolet.svg)](#)

*.NET 9.0 MAUI ile geliştirilmiş; Firebase Kimlik Doğrulama, Realtime Database CRUD, Canlı Döviz Kurları, RSS Haber Akışı, Hava Durumu ve Koyu Tema desteği sunan kapsamlı mobil uygulama.*

[Genel Bakış](#-genel-bakış) • [Modüller ve Özellikler](#-modüller-ve-özellikler) • [Mimari ve Teknolojiler](#-kullanılan-teknolojiler) • [Firebase Yapılandırması](#-firebase-yapılandırması) • [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma) • [Dizin Yapısı](#-proje-dizin-yapısı)

---

</div>

## 📌 Genel Bakış

Bu proje, modern çapraz platform mobil geliştirme standartlarına uygun olarak **.NET MAUI 9.0** üzerinde inşa edilmiştir. Uygulama; kullanıcı kimlik yönetiminden gerçek zamanlı bulut veritabanına, harici REST/RSS API tüketiminden cihazın yerel paylaşım ve depolama mekanizmalarına kadar tüm mobil geliştirme ihtiyaçlarını tek bir potada birleştirir.

---

## ⚡ Modüller ve Özellikler

### 🔐 1. Kimlik Doğrulama ve Güvenlik (Firebase Authentication)
- **Kayıt Ol (`RegisterPage`)** ve **Giriş Yap (`LoginPage`)** ekranları.
- E-posta ve şifre formatı doğrulaması, hata yakalama ve bilgilendirici kullanıcı uyarıları.
- Güvenli token oturum yönetimi.

### 👤 2. Ana Sayfa ve Karşılama (`MainPage`)
- Oturum açan kullanıcıyı karşılayan profil kartı.
- Uygulama modüllerine hızlı erişim sağlayan modern dashboard tasarımı.

### 💱 3. Canlı Döviz Kurları (`KurlarPage`)
- Finansal API üzerinden canlı döviz (Dolar, Euro, Sterlin) ve altın verilerinin anlık çekilmesi.
- Veri yüklenirken çalışan `ActivityIndicator` (yükleme animasyonu).
- Alış/Satış fiyatları ve günlük değişim oranları gösterimi.

### 📰 4. Kategori Bazlı Haber Akışı (`HaberlerPage` & `HaberDetayPage`)
- **5 farklı kategoride** (Son Dakika, Ekonomi, Teknoloji, Spor vb.) dinamik RSS akış ayrıştırma (XML Parsing).
- Haber detay sayfası ile haberin tam metnini okuma.
- **Native Share API** entegrasyonu: Haberi cihazın yerel paylaşım menüsü üzerinden (WhatsApp, X, E-posta vb.) tek tıkla paylaşabilme.

### 🌤️ 5. Hava Durumu Servisi (`HavaDurumuPage`)
- Canlı meteoroloji REST API entegrasyonu.
- Türkçe karakter düzeltme algoritması (`ç, ğ, ı, ö, ş, ü` karakterlerinin API uyumlu dönüştürülmesi).
- Sıcaklık, nem oranı, rüzgar hızı ve hava durumu ikonları.
- Eklenen şehirlerin **JSON formatında yerel depolamada (Preferences / Local Storage)** kalıcı olarak saklanması.

### 📋 6. Yapılacaklar Listesi (`YapilacaklarPage` & `TodoDetayPage`)
- **Firebase Realtime Database** ile tam senkronize çalışan bulut tabanlı görev yöneticisi.
- Tam **CRUD** desteği: Görev Ekleme, Listeleme, Güncelleme ve Silme.
- Görev tamamlama onay kutusu ve anlık veritabanı yansıması.

### 🌓 7. Ayarlar ve Tema Yönetimi (`AyarlarPage`)
- Dinamik Koyu Tema (Dark Mode) ve Açık Tema (Light Mode) geçiş desteği.
- Kullanıcının tema tercihinin oturumlar arasında yerel olarak korunması.

---

## 🛠️ Kullanılan Teknolojiler

| Bileşen | Sürüm | Açıklama |
| :--- | :--- | :--- |
| **.NET MAUI** | `net9.0-android` | Çapraz platform mobil UI çatı mimarisi |
| **FirebaseAuthentication.net** | `4.1.0` | Firebase kimlik doğrulama istemcisi |
| **FirebaseDatabase.net** | `5.0.0` | Firebase Realtime NoSQL veritabanı bağlantısı |
| **Newtonsoft.Json** | `13.0.4` | Yüksek performanslı JSON serileştirme/ayrıştırma |
| **Microsoft.Maui.Controls** | `9.0` | Temel MAUI arayüz bileşenleri ve Shell navigasyonu |

---

## 📂 Proje Dizin Yapısı

```
MAUIMobilUygulamas-Proj/
├── odev3gorsel.csproj              # Proje bağımlılıkları ve platform ayarları
├── App.xaml / App.xaml.cs          # Uygulama yaşam döngüsü
├── AppShell.xaml / AppShell.xaml.cs# Rota yönetimi ve navigasyon hiyerarşisi
├── MauiProgram.cs                  # Servis kayıtları ve başlatıcı
│
├── LoginPage.xaml (.cs)            # Giriş ekranı
├── RegisterPage.xaml (.cs)         # Kayıt ekranı
├── MainPage.xaml (.cs)             # Karşılama ve profil sayfası
├── KurlarPage.xaml (.cs)           # Canlı döviz kurları
├── HaberlerPage.xaml (.cs)         # RSS haber akışı
├── HaberDetayPage.xaml (.cs)       # Haber detay ve paylaşım sayfası
├── HavaDurumuPage.xaml (.cs)       # Canlı hava durumu ve şehir yönetimi
├── YapilacaklarPage.xaml (.cs)     # To-Do listesi (Firebase CRUD)
├── TodoDetayPage.xaml (.cs)        # Görev detay sayfası
├── AyarlarPage.xaml (.cs)          # Tema ayarları
│
├── Models/                         # Veri Modelleri (Doviz, Haber, TodoItem, Weather)
├── Services/                       # FirebaseServisi ve yardımcı API servisleri
└── Resources/                      # İkonlar, fontlar ve splash ekranları
```

---

## 🔥 Firebase Yapılandırması

Firebase entegrasyonu `Services/FirebaseService.cs` dosyası üzerinden yönetilir:

```csharp
// Firebase Realtime Database URL
private readonly string _firebaseDbUrl = "https://your-project-id.firebaseio.com/";

// Firebase Web API Key (Authentication için)
private readonly string _firebaseApiKey = "AIzaSy...";
```

> Kendi Firebase projenizi bağlamak için [Firebase Console](https://console.firebase.google.com/) üzerinden bir proje oluşturup `Authentication (Email/Password)` ve `Realtime Database` servislerini aktifleştirmeniz yeterlidir.

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (.NET MAUI iş yükü yüklü) veya VS Code + .NET MAUI eklentisi.
- Android Emulator veya fiziksel geliştirici modunda Android cihaz.

### Adım Adım Çalıştırma

#### 1. Depoyu Klonlayın:
```bash
git clone https://github.com/gmudz/MAUIMobilUygulamas-Proj.git
cd MAUIMobilUygulamas-Proj
```

#### 2. Bağımlılıkları Geri Yükleyin:
```bash
dotnet restore odev3gorsel.csproj
```

#### 3. Android Emülatöründe Çalıştırın:
```bash
dotnet build odev3gorsel.csproj -t:Run -f net9.0-android
```

---

## 📄 Lisans

Bu proje eğitim ve akademik çalışma kapsamında hazırlanmıştır.

---

<div align="center">
  <sub>Developed by <a href="https://github.com/gmudz">Murad Ashkar (gmudz)</a></sub>
</div>

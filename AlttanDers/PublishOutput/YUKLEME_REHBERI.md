# AlttanDers - Yükleme Rehberi

Bu klasör (`PublishOutput`), `alttanders.xyz` sitesine yüklenmeye hazır tüm dosyaları içerir.
Veritabanı bağlantı ayarları yapılmıştır.

> [!NOTE]
> **Veritabanı Kurulumu Hakkında:**
> Yazılımımız, site ilk açıldığında veritabanını **otomatik olarak kuracak** şekilde ayarlanmıştır.
> Bu yüzden Plesk üzerinden manuel olarak SQL dosyası yüklemenize **GEREK YOKTUR**.

## Adım 1: Dosyaları Yükleme
1.  **Plesk Panel**'e girin.
2.  **Dosyalar (File Manager)** menüsüne gidin.
3.  `httpdocs` klasörünü açın.
4.  İçindeki **eski dosyaları temizleyin** (varsa).
5.  Bu klasördeki (`PublishOutput`) **TÜM dosyaları ve klasörleri** oraya sürükleyip bırakın.

## Adım 2: Siteyi Açma
Tarayıcıdan `alttanders.xyz` adresine gidin.
- İlk açılış birkaç saniye uzun sürebilir (Veritabanını oluşturduğu için).
- Site açıldığında işlem tamamlanmış demektir!

**Sorun Çıkarsa:**
- Eğer hata alırsanız, `web.config` dosyasının `httpdocs` içinde olduğundan emin olun.
- `appsettings.json` dosyasını kontrol edin.

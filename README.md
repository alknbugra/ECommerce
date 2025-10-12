# 🛒 ECommerce API

Modern, ölçeklenebilir ve güvenli bir e-ticaret API'si. Clean Architecture, CQRS pattern ve .NET 9.0 teknolojileri kullanılarak geliştirilmiştir.

## 📋 İçindekiler

- [Özellikler](#-özellikler)
- [Teknoloji Yığını](#-teknoloji-yığını)
- [Mimari](#-mimari)
- [Kurulum](#-kurulum)
- [API Dokümantasyonu](#-api-dokümantasyonu)
- [Kullanım](#-kullanım)
- [OpenSearch Entegrasyonu](#-opensearch-entegrasyonu)
- [Docker Kurulumu](#-docker-kurulumu)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [Lisans](#-lisans)

## ✨ Özellikler

### 🔐 Kimlik Doğrulama ve Yetkilendirme

- **JWT Token** tabanlı kimlik doğrulama
- **Role-based Authorization** (RBAC)
- **Permission-based Access Control** (PBAC)
- **Refresh Token** mekanizması
- **Password Hashing** (PBKDF2 + SHA256)

### 🛍️ E-Ticaret Özellikleri

- **Ürün Yönetimi** - CRUD işlemleri, resim yükleme
- **Kategori Yönetimi** - Hiyerarşik kategori yapısı
- **Sipariş Yönetimi** - Sipariş oluşturma, durum takibi
- **Kullanıcı Yönetimi** - Profil yönetimi, şifre değiştirme
- **Adres Yönetimi** - Teslimat ve fatura adresleri

### 🏗️ Mimari ve Kalite

- **Clean Architecture** - Katmanlı mimari
- **CQRS Pattern** - Command Query Responsibility Segregation
- **Repository Pattern** - Veri erişim soyutlaması
- **Unit of Work** - İşlem yönetimi
- **Dependency Injection** - Bağımlılık yönetimi

### 🔧 Geliştirici Deneyimi

- **FluentValidation** - Veri doğrulama
- **Global Exception Handling** - Merkezi hata yönetimi
- **Structured Logging** - Serilog + OpenTelemetry
- **OpenSearch Integration** - Log görselleştirme
- **Swagger/OpenAPI** - API dokümantasyonu
- **Health Checks** - Sistem durumu kontrolü

### ⚡ Performans ve Ölçeklenebilirlik

- **In-Memory Caching** - Performans optimizasyonu
- **Async/Await** - Asenkron programlama
- **Soft Delete** - Veri güvenliği
- **Database Migrations** - Veritabanı versiyonlama
- **Seed Data** - Otomatik test verisi

## 🛠️ Teknoloji Yığını

### Backend

- **.NET 9.0** - Framework
- **ASP.NET Core Web API** - Web API
- **Entity Framework Core** - ORM
- **SQL Server** - Veritabanı
- **AutoMapper** - Object Mapping

### Authentication & Security

- **JWT Bearer Token** - Kimlik doğrulama
- **BCrypt** - Şifre hashleme
- **FluentValidation** - Veri doğrulama

### Logging & Monitoring

- **Serilog** - Structured logging
- **OpenTelemetry** - Distributed tracing
- **OpenSearch** - Log aggregation
- **OpenSearch Dashboard** - Log visualization

### Development Tools

- **Swagger/OpenAPI** - API documentation
- **Docker** - Containerization
- **Git** - Version control

## 🏛️ Mimari

Proje Clean Architecture prensiplerine uygun olarak 4 katmanlı yapıda tasarlanmıştır:

```
ECommerce/
├── ECommerce.Domain/          # Domain Layer
│   ├── Entities/             # Domain entities
│   ├── Enums/               # Domain enums
│   ├── Interfaces/          # Repository interfaces
│   └── Exceptions/          # Domain exceptions
├── ECommerce.Application/     # Application Layer
│   ├── Features/            # CQRS features
│   ├── DTOs/               # Data transfer objects
│   ├── Common/             # Shared application logic
│   └── Mappings/           # AutoMapper profiles
├── ECommerce.Infrastructure/  # Infrastructure Layer
│   ├── Data/               # Database context
│   ├── Repositories/       # Repository implementations
│   ├── Services/           # External services
│   └── Configuration/      # Configuration classes
└── ECommerce.API/           # Presentation Layer
    ├── Endpoints/          # API endpoints
    ├── Common/            # Shared API logic
    └── Middleware/        # Custom middleware
```

### CQRS Pattern

- **Commands** - Veri değiştirme işlemleri
- **Queries** - Veri okuma işlemleri
- **Handlers** - İş mantığı implementasyonu
- **Validators** - Veri doğrulama

## 🚀 Kurulum

### Gereksinimler

- .NET 9.0 SDK
- SQL Server (LocalDB desteklenir)
- Visual Studio 2022 veya VS Code
- Docker (OpenSearch için)

### 1. Repository'yi Klonlayın

```bash
git clone https://github.com/ismailoze/ECommerce.git
cd ECommerce
```

### 2. Veritabanı Bağlantısını Yapılandırın

`ECommerce.API/appsettings.json` dosyasında connection string'i güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Paketleri Yükleyin

```bash
dotnet restore
```

### 4. Veritabanını Oluşturun

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.API
```

### 5. Uygulamayı Çalıştırın

```bash
dotnet run --project ECommerce.API
```

Uygulama `https://localhost:7047` adresinde çalışacaktır.

## 📚 API Dokümantasyonu

### Swagger UI

Uygulama çalıştıktan sonra Swagger UI'ya erişin:

- **Swagger UI**: `https://localhost:7047/swagger`
- **OpenAPI JSON**: `https://localhost:7047/swagger/v1/swagger.json`

### Ana Endpoint'ler

#### 🔐 Authentication

- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Giriş yapma
- `POST /api/auth/refresh-token` - Token yenileme

#### 👥 Users

- `GET /api/users` - Kullanıcıları listele
- `GET /api/users/{id}` - Kullanıcı detayı
- `PUT /api/users/{id}/profile` - Profil güncelle
- `PUT /api/users/{id}/password` - Şifre değiştir

#### 🛍️ Products

- `GET /api/products` - Ürünleri listele
- `GET /api/products/{id}` - Ürün detayı
- `POST /api/products` - Ürün oluştur
- `PUT /api/products/{id}` - Ürün güncelle
- `DELETE /api/products/{id}` - Ürün sil

#### 📂 Categories

- `GET /api/categories` - Kategorileri listele
- `GET /api/categories/{id}` - Kategori detayı
- `POST /api/categories` - Kategori oluştur
- `PUT /api/categories/{id}` - Kategori güncelle
- `DELETE /api/categories/{id}` - Kategori sil

#### 🛒 Orders

- `GET /api/orders` - Siparişleri listele
- `GET /api/orders/{id}` - Sipariş detayı
- `POST /api/orders` - Sipariş oluştur
- `PUT /api/orders/{id}/status` - Sipariş durumu güncelle

#### 🔑 Permissions

- `GET /api/permissions` - Yetkileri listele
- `GET /api/permissions/roles/{roleId}` - Rol yetkileri
- `POST /api/permissions/roles/assign` - Role yetki ata
- `GET /api/permissions/users/{userId}` - Kullanıcı yetkileri

#### 📁 File Upload

- `POST /api/files/upload` - Dosya yükle
- `DELETE /api/files/{id}` - Dosya sil

## 💻 Kullanım

### 1. Kullanıcı Kaydı

```bash
curl -X POST "https://localhost:7047/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

### 2. Giriş Yapma

```bash
curl -X POST "https://localhost:7047/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!"
  }'
```

### 3. Ürün Oluşturma (JWT Token ile)

```bash
curl -X POST "https://localhost:7047/api/products" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Örnek Ürün",
    "description": "Ürün açıklaması",
    "price": 99.99,
    "categoryId": "category-guid"
  }'
```

## 🔍 OpenSearch Entegrasyonu

### OpenSearch Kurulumu

```bash
# Docker Compose ile OpenSearch başlatın
docker-compose -f docker-compose.opensearch.yml up -d
```

### OpenSearch Dashboard

- **URL**: `http://localhost:5601`
- **Log Index**: `ecommerce-logs-*`
- **Template**: `ecommerce-logs-template`

### Log Görüntüleme

1. OpenSearch Dashboard'ı açın
2. "Discover" sekmesine gidin
3. `ecommerce-logs-*` index'ini seçin
4. Logları filtreleyin ve analiz edin

Detaylı kurulum için [OPENSEARCH_SETUP.md](OPENSEARCH_SETUP.md) dosyasını inceleyin.

## 🐳 Docker Kurulumu

### OpenSearch ve Dashboard

```bash
# OpenSearch servislerini başlat
docker-compose -f docker-compose.opensearch.yml up -d

# Servisleri durdur
docker-compose -f docker-compose.opensearch.yml down
```

### Servisler

- **OpenSearch**: `http://localhost:9200`
- **OpenSearch Dashboard**: `http://localhost:5601`
- **Logstash** (opsiyonel): `http://localhost:5044`

## 🧪 Test

### Unit Testleri Çalıştırma

```bash
dotnet test
```

### Test Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Monitoring ve Logging

### Health Checks

- **Health Check**: `GET /health`
- **OpenSearch Health**: OpenSearch bağlantı durumu

### Log Seviyeleri

- **Information**: Genel bilgi logları
- **Warning**: Uyarı logları
- **Error**: Hata logları
- **Critical**: Kritik hata logları

### Structured Logging

Tüm loglar JSON formatında OpenSearch'e gönderilir:

```json
{
  "timestamp": "2025-01-12T10:30:00Z",
  "level": "Information",
  "message": "User login successful",
  "userId": "user-guid",
  "email": "user@example.com",
  "sourceContext": "ECommerce.Application.Features.Auth.Commands.Login.LoginCommandHandler"
}
```

## 🔧 Yapılandırma

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "ECommerce.API",
    "Audience": "ECommerce.Users",
    "ExpiryMinutes": 60
  },
  "FileUpload": {
    "MaxFileSizeMB": 10,
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".gif"],
    "UploadPath": "uploads"
  },
  "Cache": {
    "DefaultExpirationMinutes": 30,
    "MaxSize": 1000
  },
  "OpenSearch": {
    "Enabled": true,
    "NodeUris": ["http://localhost:9200"],
    "IndexFormat": "ecommerce-logs-{0:yyyy.MM.dd}"
  }
}
```

## 🚀 Deployment

### Production Ortamı

1. `appsettings.Production.json` oluşturun
2. Connection string'i güncelleyin
3. JWT secret key'i güvenli bir değerle değiştirin
4. OpenSearch yapılandırmasını güncelleyin

### Docker ile Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ECommerce.API/ECommerce.API.csproj", "ECommerce.API/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]
RUN dotnet restore "ECommerce.API/ECommerce.API.csproj"
COPY . .
WORKDIR "/src/ECommerce.API"
RUN dotnet build "ECommerce.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]
```

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add some amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

### Geliştirme Kuralları

- Clean Code prensiplerini takip edin
- Unit test yazın
- Swagger dokümantasyonunu güncelleyin
- Conventional Commits formatını kullanın

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 👨‍💻 Geliştirici

**İsmail Özer**

- GitHub: [@ismailoze](https://github.com/ismailoze)
- LinkedIn: [linkedin.com/in/ismailoze](https://linkedin.com/in/ismailoze)
- Email: ismailozer35041@gmail.com

## 🙏 Teşekkürler

- .NET Community
- Clean Architecture advocates
- Open Source contributors

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!

## 📈 Roadmap

### Gelecek Özellikler

- [ ] Payment Gateway entegrasyonu
- [ ] Email notification sistemi
- [ ] Real-time notifications (SignalR)
- [ ] Advanced search (Elasticsearch)
- [ ] API rate limiting
- [ ] Multi-language support
- [ ] Mobile API optimizations
- [ ] GraphQL endpoint
- [ ] Microservices architecture
- [ ] Kubernetes deployment

### Versiyon Geçmişi

- **v1.0.0** - İlk sürüm (Clean Architecture, CQRS, JWT Auth)
- **v1.1.0** - Permission sistemi eklendi
- **v1.2.0** - OpenSearch entegrasyonu
- **v1.3.0** - Caching ve performance optimizasyonları

---

**Son güncelleme**: 2025-10-13

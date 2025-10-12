# OpenSearch ve OpenSearch Dashboard Kurulumu

Bu doküman, ECommerce uygulaması için OpenSearch ve OpenSearch Dashboard kurulumunu açıklar.

## 🚀 Hızlı Başlangıç

### 1. Docker Compose ile Kurulum

```bash
# OpenSearch ve OpenSearch Dashboard'u başlat
docker-compose -f docker-compose.opensearch.yml up -d

# Servislerin durumunu kontrol et
docker-compose -f docker-compose.opensearch.yml ps

# Logları görüntüle
docker-compose -f docker-compose.opensearch.yml logs -f
```

### 2. Servis Erişim URL'leri

- **OpenSearch**: http://localhost:9200
- **OpenSearch Dashboard**: http://localhost:5601
- **Health Check**: http://localhost:9200/\_cluster/health

### 3. Uygulamayı Başlat

```bash
# ECommerce API'yi başlat
cd ECommerce.API
dotnet run
```

## 📊 Log Görselleştirme

### 1. OpenSearch Dashboard'a Erişim

1. Tarayıcıda http://localhost:5601 adresine git
2. "Index Patterns" bölümüne git
3. `ecommerce-logs-*` pattern'ini oluştur
4. Time field olarak `@timestamp` seç

### 2. Dashboard Oluşturma

#### Temel Dashboard

- **Log Level Distribution**: Log seviyelerinin dağılımı
- **Request Count**: API istek sayıları
- **Response Time**: Yanıt süreleri
- **Error Rate**: Hata oranları
- **Top Endpoints**: En çok kullanılan endpoint'ler

#### Gelişmiş Dashboard

- **User Activity**: Kullanıcı aktiviteleri
- **Performance Metrics**: Performans metrikleri
- **Security Events**: Güvenlik olayları
- **Business Metrics**: İş metrikleri

### 3. Örnek Queries

#### Hata Logları

```json
{
  "query": {
    "bool": {
      "must": [
        {
          "term": {
            "Level": "Error"
          }
        }
      ]
    }
  }
}
```

#### Yavaş İstekler

```json
{
  "query": {
    "bool": {
      "must": [
        {
          "range": {
            "Elapsed": {
              "gte": 1000
            }
          }
        }
      ]
    }
  }
}
```

#### Belirli Kullanıcı

```json
{
  "query": {
    "bool": {
      "must": [
        {
          "term": {
            "UserId": "user-123"
          }
        }
      ]
    }
  }
}
```

## 🔧 Yapılandırma

### 1. appsettings.json

```json
{
  "OpenSearch": {
    "Enabled": true,
    "NodeUris": ["http://localhost:9200"],
    "IndexFormat": "ecommerce-logs-{0:yyyy.MM.dd}",
    "BatchSize": 1000,
    "BatchPostingIntervalSeconds": 2,
    "AutoRegisterTemplate": true,
    "NumberOfShards": 1,
    "NumberOfReplicas": 0
  }
}
```

### 2. Production Ayarları

```json
{
  "OpenSearch": {
    "Enabled": true,
    "NodeUris": ["https://opensearch-cluster.example.com:9200"],
    "Username": "opensearch-user",
    "Password": "secure-password",
    "VerifySsl": true,
    "CertificateFingerprint": "SHA256:...",
    "NumberOfShards": 3,
    "NumberOfReplicas": 1,
    "IndexRetentionDays": 90
  }
}
```

## 📈 Monitoring ve Alerting

### 1. Health Checks

```bash
# Cluster health
curl http://localhost:9200/_cluster/health

# Node stats
curl http://localhost:9200/_nodes/stats

# Index stats
curl http://localhost:9200/_stats
```

### 2. Alerting Rules

#### High Error Rate

```json
{
  "trigger": {
    "schedule": {
      "interval": "1m"
    }
  },
  "conditions": [
    {
      "script": {
        "source": "ctx.results[0].hits.total.value > 10"
      }
    }
  ],
  "actions": [
    {
      "webhook": {
        "url": "https://hooks.slack.com/services/...",
        "body": {
          "text": "High error rate detected in ECommerce API"
        }
      }
    }
  ]
}
```

## 🛠️ Troubleshooting

### 1. Bağlantı Sorunları

```bash
# OpenSearch erişilebilir mi?
curl http://localhost:9200

# Cluster durumu
curl http://localhost:9200/_cluster/health?pretty
```

### 2. Log Sorunları

```bash
# Serilog logları
tail -f logs/ecommerce-*.log

# Dead letter queue
ls -la logs/dead-letter-queue/
```

### 3. Performance Sorunları

```bash
# Index stats
curl http://localhost:9200/_stats?pretty

# Cluster stats
curl http://localhost:9200/_cluster/stats?pretty
```

## 📚 Kaynaklar

- [OpenSearch Documentation](https://opensearch.org/docs/)
- [OpenSearch Dashboard Guide](https://opensearch.org/docs/dashboards/)
- [Serilog OpenSearch Sink](https://github.com/serilog/serilog-sinks-opensearch)
- [OpenSearch Best Practices](https://opensearch.org/docs/latest/opensearch/install/important-settings/)

## 🔒 Güvenlik

### 1. Development

- Security plugin devre dışı
- HTTP bağlantısı
- Authentication yok

### 2. Production

- Security plugin etkin
- HTTPS bağlantısı
- Authentication/Authorization
- Certificate pinning
- Network security groups
- VPC isolation

## 📊 Dashboard Templates

### 1. ECommerce API Dashboard

- Request/Response metrics
- Error tracking
- Performance monitoring
- User activity
- Business metrics

### 2. Infrastructure Dashboard

- System metrics
- Resource utilization
- Health checks
- Alert status
- Capacity planning

### 3. Security Dashboard

- Authentication events
- Authorization failures
- Suspicious activities
- Rate limiting
- Security alerts

# 個人理財管理系統 (Personal Finance Management System)

一個使用 ASP.NET Core Web API 和 Entity Framework Core 開發的個人理財管理後端系統，提供收支記錄、分類管理和統計分析功能。

## 專案架構

### 系統架構圖
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   前端應用      │    │   Web API       │    │   資料庫        │
│   (Vue.js)      │◄──►│   (.NET Core)   │◄──►│   (SQL Server)  │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### 專案結構
```
PersonalFinanceAPI/
├── Controllers/           # API 控制器
│   ├── AuthController.cs
│   ├── TransactionController.cs
│   ├── CategoryController.cs
│   └── ReportController.cs
├── Models/               # 資料模型
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Transaction.cs
│   │   └── Category.cs
│   └── DTOs/
│       ├── AuthDTOs.cs
│       ├── TransactionDTOs.cs
│       └── ReportDTOs.cs
├── Data/                 # 資料存取層
│   ├── ApplicationDbContext.cs
│   └── Repositories/
│       ├── ITransactionRepository.cs
│       └── TransactionRepository.cs
├── Services/             # 業務邏輯層
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── ITransactionService.cs
│   └── TransactionService.cs
├── Middleware/           # 中介軟體
│   └── ErrorHandlingMiddleware.cs
└── Program.cs           # 程式進入點
```

## 功能特色

- 🔐 JWT 身份驗證
- 💰 收支交易記錄管理
- 📊 分類統計與報表
- 📱 RESTful API 設計
- 🛡️ 資料驗證與錯誤處理
- 📈 月度/年度財務報告

## 技術棧

- **後端框架**: ASP.NET Core 6.0
- **資料庫**: SQL Server
- **ORM**: Entity Framework Core
- **身份驗證**: JWT Bearer Token
- **API 文件**: Swagger/OpenAPI
- **容器化**: Docker

## 安裝步驟

### 環境需求
- .NET 6.0 SDK
- SQL Server 2019+ 或 SQL Server Express
- Visual Studio 2022 或 VS Code

### 1. 複製專案
```bash
git clone https://github.com/your-username/personal-finance-api.git
cd personal-finance-api
```

### 2. 設定資料庫連接字串
在 `appsettings.json` 中修改資料庫連接字串：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalFinanceDB;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here-minimum-32-characters",
    "Issuer": "PersonalFinanceAPI",
    "Audience": "PersonalFinanceClient",
    "ExpirationMinutes": 60
  }
}
```

### 3. 安裝相依套件
```bash
dotnet restore
```

### 4. 建立資料庫
```bash
dotnet ef database update
```

### 5. 執行專案
```bash
dotnet run
```

API 將在 `https://localhost:5001` 或 `http://localhost:5000` 上運行。

### 6. 使用 Docker (可選)
```bash
# 建置映像檔
docker build -t personal-finance-api .

# 執行容器
docker run -p 5000:80 personal-finance-api
```

## API 使用說明

### Base URL
```
https://localhost:5001/api
```

### 身份驗證

#### 註冊用戶
```http
POST /auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!",
  "name": "使用者姓名"
}
```

#### 用戶登入
```http
POST /auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**回應範例:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "name": "使用者姓名"
  },
  "expiresAt": "2025-06-07T10:30:00Z"
}
```

### 交易管理 (需要驗證)

> **注意**: 以下 API 需要在 Header 中包含 JWT Token: `Authorization: Bearer <token>`

#### 取得所有交易記錄
```http
GET /transactions?page=1&pageSize=10&categoryId=1&startDate=2025-01-01&endDate=2025-12-31
```

#### 新增交易記錄
```http
POST /transactions
Content-Type: application/json

{
  "amount": 1500.00,
  "description": "午餐費用",
  "categoryId": 2,
  "transactionDate": "2025-06-06T12:30:00Z",
  "type": "expense"
}
```

#### 更新交易記錄
```http
PUT /transactions/{id}
Content-Type: application/json

{
  "amount": 1800.00,
  "description": "午餐費用 (已修正)",
  "categoryId": 2,
  "transactionDate": "2025-06-06T12:30:00Z",
  "type": "expense"
}
```

#### 刪除交易記錄
```http
DELETE /transactions/{id}
```

### 分類管理

#### 取得所有分類
```http
GET /categories
```

#### 新增分類
```http
POST /categories
Content-Type: application/json

{
  "name": "餐飲",
  "description": "用餐相關支出",
  "type": "expense",
  "color": "#FF6B6B"
}
```

### 報表統計

#### 取得月度報表
```http
GET /reports/monthly?year=2025&month=6
```

**回應範例:**
```json
{
  "totalIncome": 50000.00,
  "totalExpense": 35000.00,
  "netAmount": 15000.00,
  "categoryBreakdown": [
    {
      "categoryName": "餐飲",
      "amount": 8000.00,
      "percentage": 22.86
    }
  ],
  "dailyTrends": [
    {
      "date": "2025-06-01",
      "income": 0,
      "expense": 150.00
    }
  ]
}
```

#### 取得年度報表
```http
GET /reports/yearly?year=2025
```

#### 取得分類統計
```http
GET /reports/categories?startDate=2025-01-01&endDate=2025-12-31&type=expense
```

## 資料模型

### Transaction (交易)
- `Id`: 交易 ID
- `Amount`: 金額
- `Description`: 描述
- `CategoryId`: 分類 ID
- `TransactionDate`: 交易日期
- `Type`: 類型 (income/expense)
- `UserId`: 用戶 ID

### Category (分類)
- `Id`: 分類 ID
- `Name`: 分類名稱
- `Description`: 描述
- `Type`: 類型 (income/expense)
- `Color`: 顏色代碼

### User (用戶)
- `Id`: 用戶 ID
- `Email`: 電子郵件
- `Name`: 姓名
- `PasswordHash`: 密碼雜湊

## 狀態碼說明

- `200 OK`: 請求成功
- `201 Created`: 資源創建成功
- `400 Bad Request`: 請求格式錯誤
- `401 Unauthorized`: 未授權存取
- `404 Not Found`: 資源不存在
- `500 Internal Server Error`: 伺服器內部錯誤

## 開發計劃

### Phase 1 (基礎功能) ✅
- [x] 用戶註冊登入
- [x] 基本 CRUD 操作
- [x] JWT 身份驗證

### Phase 2 (統計報表)
- [ ] 月度/年度報表
- [ ] 分類統計圖表
- [ ] 趨勢分析

### Phase 3 (進階功能)
- [ ] 預算設定與提醒
- [ ] 定期交易
- [ ] 匯入銀行對帳單
- [ ] 資料備份與匯出

## 測試

```bash
# 執行單元測試
dotnet test

# 執行測試並產生覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

## 貢獻指南

1. Fork 此專案
2. 建立功能分支 (`git checkout -b feature/new-feature`)
3. 提交變更 (`git commit -am 'Add new feature'`)
4. 推送到分支 (`git push origin feature/new-feature`)
5. 建立 Pull Request

## 授權條款

此專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

## 聯絡方式

- 開發者: 康智詠
- Email: kcyo0928@gmail.com
- GitHub: https://github.com/kangcy28

---

## 相關連結

- [API 文件 (Swagger)](https://localhost:5001/swagger)
- [專案看板](https://github.com/your-username/personal-finance-api/projects)
- [問題回報](https://github.com/your-username/personal-finance-api/issues)

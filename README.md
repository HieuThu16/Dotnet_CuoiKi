# MyStore - E-Commerce Mobile App
## Uno Platform + MAUI + SQLite + EF Core

A complete e-commerce mobile application built with Uno Platform, featuring offline support, shopping cart management, and checkout functionality.

---

## 📋 Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Building & Running](#building--running)
- [Unit Tests](#unit-tests)
- [Packaging for Release](#packaging-for-release)
- [Troubleshooting](#troubleshooting)

---

## ✨ Features

- ✅ **Browse Products**: Fetch from API or mock data
- ✅ **Product Details**: View detailed information with image, price, stock
- ✅ **Shopping Cart**: Add/remove items, adjust quantities, persistent storage
- ✅ **Offline Support**: Cart data saved locally with SQLite
- ✅ **Checkout**: Customer information form with validation
- ✅ **Order Submission**: POST to backend API
- ✅ **Real-time UI Updates**: Event-driven cart state management
- ✅ **Error Handling**: Network error detection and user feedback
- ✅ **Responsive Design**: Works on Android, iOS, WebAssembly, Windows

---

## 📁 Project Structure

```
MyStore/
├── MyStore.Core/                    # Shared library (Models, Services, Data)
│   ├── Models/
│   │   ├── Product.cs               # Product entity
│   │   ├── CartItem.cs              # Cart item entity
│   │   ├── OrderModel.cs            # Order entity & DTOs
│   │   └── ProductDto.cs            # DTO for API responses
│   ├── Services/
│   │   ├── ICartService.cs          # Cart interface
│   │   ├── CartService.cs           # Cart implementation
│   │   ├── IStoreApiClient.cs       # Refit API interface
│   │   └── StoreApiClient.cs        # API client (Refit)
│   ├── Data/
│   │   └── AppDbContext.cs          # EF Core DbContext
│   └── MyStore.Core.csproj
│
├── MyStore.Mobile/                  # Uno Platform MAUI App (Shared)
│   ├── ViewModels/
│   │   ├── ViewModelBase.cs         # Base MVVM class
│   │   ├── ProductsViewModel.cs
│   │   ├── ProductDetailViewModel.cs
│   │   ├── CartViewModel.cs
│   │   └── CheckoutViewModel.cs
│   ├── Views/
│   │   ├── ProductsPage.xaml/.cs
│   │   ├── ProductDetailPage.xaml/.cs
│   │   ├── CartPage.xaml/.cs
│   │   └── CheckoutPage.xaml/.cs
│   ├── App.xaml/.cs
│   ├── AppShell.xaml/.cs
│   ├── MauiProgram.cs               # DI & App configuration
│   └── MyStore.Mobile.csproj
│
├── MyStore.Tests/                   # xUnit Test Project
│   ├── CartServiceTests.cs
│   └── MyStore.Tests.csproj
│
├── MyStore.sln                      # Solution file
├── README.md                        # This file
├── report.md                        # Final project report
└── acceptance_criteria.md           # Test criteria

```

---

## 🛠 Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | Uno Platform / MAUI | 5.0+ / 8.0+ |
| **Language** | C# | 11.0+ |
| **.NET Version** | .NET | 7.0 / 8.0 |
| **MVVM Toolkit** | CommunityToolkit.Mvvm | 8.2.0+ |
| **Database** | SQLite | 3.40+ |
| **ORM** | Entity Framework Core | 7.0 / 8.0 |
| **HTTP Client** | Refit | 6.3+ |
| **Testing** | xUnit | 2.4+ |
| **Package Manager** | NuGet | Latest |

---

## 📦 Prerequisites

### Required Software

1. **Visual Studio 2022** (Community/Professional/Enterprise)
   - Workload: .NET MAUI Development
   - Uno Platform extension

2. **.NET SDK 8.0 LTS** or later
   ```bash
   dotnet --version  # Verify installation
   ```

3. **Git** (for version control)

### For Specific Platforms

- **Android**: Android SDK 21+ (API Level 21, Android 5.0)
- **iOS**: Xcode 14+ (on macOS), iOS 12.0+
- **WebAssembly**: No additional setup needed
- **Windows**: .NET MAUI Workload

---

## 🚀 Installation & Setup

### 1. Clone Repository
```bash
cd c:\Users\ASUS Vivobook\Desktop\Dotnet_CuoiKi
git clone <repository-url> .
# or if already in repo:
git status
```

### 2. Restore NuGet Packages
```bash
# In Solution directory
dotnet restore

# Or using Visual Studio:
# Build > Clean Solution
# Build > Restore NuGet Packages
```

### 3. Build Solution (Windows)
```bash
# Using CLI:
dotnet build MyStore.sln -c Debug

# Using Visual Studio:
# File > Open > MyStore.sln
# Build > Build Solution (Ctrl+Shift+B)
```

### 4. Database Setup
- **Automatic**: Database created on first app launch
- **Manual**: Run in Package Manager Console:
  ```powershell
  # Set MyStore.Core as default project
  Add-Migration InitialCreate
  Update-Database
  ```

---

## 🏃 Building & Running

### Desktop (Windows)

#### CLI Method:
```bash
# Build
dotnet build MyStore.sln -c Debug

# Run
dotnet run --project MyStore.Mobile\MyStore.Mobile.csproj
```

#### Visual Studio Method:
1. Open `MyStore.sln`
2. Select debug profile: **Windows Machine** or **Simulator**
3. Click **▶ Start** or press **F5**

---

### Android

#### Prerequisites:
- Android SDK 21+ installed
- Android Emulator or physical device connected

#### CLI Method:
```bash
# List available Android devices:
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -c Debug -f net8.0-android

# Run on specific device:
dotnet run --project MyStore.Mobile\MyStore.Mobile.csproj -c Debug -f net8.0-android
```

#### Visual Studio Method:
1. Open `MyStore.Mobile.csproj`
2. Select target framework: `net8.0-android`
3. Select Android Emulator or device
4. Click **▶ Start** (F5)

#### Using Android Studio:
```bash
# Create emulator through Android Studio AVD Manager
# Then run via Visual Studio
```

---

### iOS (macOS Only)

#### Prerequisites:
- macOS Monterey or later
- Xcode 14+
- iOS SDK 12.0+

#### Build for iOS Simulator:
```bash
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -c Debug -f net8.0-ios

# Or from Visual Studio for Mac
```

#### Build for Physical Device:
1. Open project in Xcode
2. Configure signing: Team ID, Bundle ID
3. Select physical device
4. Build & run

---

### WebAssembly

#### CLI Method:
```bash
# Build WASM
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -c Debug -f net8.0-wasm

# Run WASM development server
dotnet run --project MyStore.Mobile\MyStore.Mobile.csproj -c Debug -f net8.0-wasm

# Navigate to: http://localhost:5000
```

#### Using IIS:
```bash
# Build release
dotnet publish -c Release -f net8.0-wasm -o .\bin\wasm-release

# Deploy to IIS
# Copy contents of bin\wasm-release\MyStore.Mobile\wwwroot to IIS folder
```

---

## 🧪 Unit Tests

### Running Tests with CLI

```bash
# Run all tests
dotnet test MyStore.Tests\MyStore.Tests.csproj

# Run with verbose output
dotnet test MyStore.Tests\MyStore.Tests.csproj -v d

# Run specific test
dotnet test MyStore.Tests\MyStore.Tests.csproj -k "CartServiceTests"

# Run with code coverage
dotnet test MyStore.Tests\MyStore.Tests.csproj /p:CollectCoverage=true
```

### Running Tests in Visual Studio

1. **Test Explorer**: View > Test Explorer (Ctrl+E, T)
2. **Run All**: Click "Run All Tests"
3. **Run Single**: Right-click test > Run
4. **Debug**: Right-click test > Debug

### Test Summary

- **CartServiceTests.cs**: 12 tests covering:
  - Adding items (single, multiple, duplicate)
  - Updating quantities
  - Removing items
  - Calculating totals
  - Clearing cart
  - Event handling
  - Stock validation

---

## 📦 Packaging for Release

### Android (.aab - Google Play)

#### 1. Generate Signing Key:
```bash
keytool -genkey -v -keystore MyStore.keystore -keyalg RSA `
  -keysize 2048 -validity 10000 -alias mystore_key
```

#### 2. Configure Signing in .csproj:
```xml
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <AndroidKeyStore>true</AndroidKeyStore>
    <AndroidSigningKeyStore>MyStore.keystore</AndroidSigningKeyStore>
    <AndroidSigningKeyAlias>mystore_key</AndroidSigningKeyAlias>
    <AndroidSigningKeyPass>YOUR_PASSWORD</AndroidSigningKeyPass>
    <AndroidSigningStorePass>YOUR_PASSWORD</AndroidSigningStorePass>
</PropertyGroup>
```

#### 3. Build Release:
```bash
dotnet publish -c Release -f net8.0-android -o .\publish\android

# Output: MyStore.Mobile.aab (in publish folder)
```

#### 4. Upload to Google Play Console:
- Go to Google Play Console
- Create new app
- Upload .aab file in Release > Testing track
- Promote to production after testing

---

### iOS (.ipa - Apple App Store)

#### 1. Generate Provisioning Profile:
- Apple Developer Account > Certificates, Identifiers & Profiles
- Create Bundle ID: `com.mycompany.mystore`
- Create Provisioning Profile (Distribution)
- Download and install

#### 2. Configure Code Signing:
```bash
# In Xcode project settings:
# General > Signing & Capabilities
# Select Team
# Set Bundle Identifier
```

#### 3. Build Archive:
```bash
dotnet publish -c Release -f net8.0-ios -o .\publish\ios
```

#### 4. Create .ipa:
```bash
# Using Xcode
xcodebuild -archive <path-to-archive> -exportOptionsPlist options.plist -exportPath ./ipa
```

#### 5. Upload to App Store:
- Use Transporter app
- Upload .ipa to TestFlight or Production

---

### Windows (MSIX Package)

```bash
# Build MSIX
dotnet publish -c Release -f net8.0-windows10.0.19041.0 -o .\publish\windows

# Output: MyStore.Mobile_<version>_x64.msix
```

---

## 🔧 Troubleshooting

### Common Issues

#### **Issue: Database file not found**
```
Solution: Run app with admin privileges. Database auto-creates at:
- Android: /data/data/com.mystore.mobile/files/mystore.db
- iOS: ~/Library/Application Support/mystore.db
- Windows: %LOCALAPPDATA%/MyStore/mystore.db
```

#### **Issue: API Connection Timeout**
```
Solution:
1. Check network connectivity
2. Verify API URL in MauiProgram.cs
3. Check firewall/proxy settings
4. Use MockStoreApiClient for testing
```

#### **Issue: NuGet Restore Fails**
```bash
# Clear cache and retry
dotnet nuget locals all --clear
dotnet restore MyStore.sln
```

#### **Issue: Build Fails - Missing Android SDK**
```bash
# Install Android SDK via Visual Studio
# Tools > Android > Android SDK Manager
# Install SDK 21+ (API Level 21)
```

#### **Issue: iOS Build Fails - Code Signing**
```
Solution:
1. Xcode > Preferences > Accounts
2. Add Apple ID
3. Click "Manage Certificates"
4. Create new certificate
5. Configure provisioning profile
```

---

## 📱 API Configuration

### Using Real API

Modify `MauiProgram.cs`:

```csharp
private static IStoreApiClient CreateApiClient()
{
    // Replace with your API URL
    const string baseUrl = "https://api.example.com";

    var handler = new HttpClientHandler
    {
        // Only for development - remove in production!
        // ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    };

    var httpClient = new HttpClient(handler);
    return RestService.For<IStoreApiClient>(httpClient, baseUrl);
}
```

### API Endpoints Required

```
GET  /api/products          → Returns List<ProductDto>
GET  /api/products/{id}     → Returns ProductDto
POST /api/orders            → Accepts OrderDto, returns ApiResponse<OrderResponseDto>
GET  /api/orders/{orderId}  → Returns OrderStatusDto
```

---

## 📚 Dependencies

### NuGet Packages Required

```xml
<!-- MyStore.Core.csproj -->
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
    <PackageReference Include="Refit" Version="6.3.0" />
    <PackageReference Include="Refit.Newtonsoft.Json" Version="6.3.0" />
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
</ItemGroup>

<!-- MyStore.Mobile.csproj -->
<ItemGroup>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
    <PackageReference Include="Uno.Maui" Version="5.0.0" />
</ItemGroup>

<!-- MyStore.Tests.csproj -->
<ItemGroup>
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.5.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
</ItemGroup>
```

---

## 🎯 Key Features Implementation

### Offline-First Architecture
- Local SQLite database stores cart items
- Cart persists across app restarts
- Network errors don't lose data

### Real-time UI Updates
- `CartChangedEvent` notifies ViewModels
- ObservableCollection updates UI automatically
- Property notification via MVVM Toolkit

### MVVM Pattern
- `ViewModelBase`: Common logic (loading, error handling)
- Commands: `IRelayCommand`, `IAsyncRelayCommand`
- ObservableProperty: Automatic PropertyChanged
- Dependency Injection: Service locator pattern

---

## 📊 Database Schema

### Products Table
```sql
CREATE TABLE Products (
    Id INTEGER PRIMARY KEY,
    Name NVARCHAR(256) NOT NULL,
    Description NVARCHAR(2000),
    Price DECIMAL(18,2),
    ImageUrl NVARCHAR(2048),
    Stock INTEGER
)
```

### CartItems Table
```sql
CREATE TABLE CartItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER NOT NULL,
    Name NVARCHAR(256) NOT NULL,
    UnitPrice DECIMAL(18,2),
    Quantity INTEGER,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
)
```

### Orders Table
```sql
CREATE TABLE Orders (
    OrderId NVARCHAR(32) PRIMARY KEY,
    CustomerName NVARCHAR(256) NOT NULL,
    CustomerAddress NVARCHAR(512) NOT NULL,
    CustomerPhone NVARCHAR(20) NOT NULL,
    Items NVARCHAR(MAX), -- JSON serialized
    Total DECIMAL(18,2),
    Status NVARCHAR(50),
    Notes NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
)
```

---

## 📞 Support & Debugging

### Enable Debug Logging:
```csharp
// In AppDbContext
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlite(...)
        .LogTo(Console.WriteLine);  // Enable logging
}
```

### Check Cart State:
```csharp
var cartService = ServiceLocator.Current.GetService<ICartService>();
var items = await cartService.GetCartAsync();
var total = await cartService.GetTotalPriceAsync();
Debug.WriteLine($"Cart: {items.Count} items, ${total}");
```

---

## 📄 License

This project is provided for educational purposes.

---

## ✅ Checklist Before Submission

- [ ] All unit tests pass
- [ ] App builds without warnings
- [ ] Android build produces .aab
- [ ] iOS build produces .ipa
- [ ] WASM runs on http://localhost:5000
- [ ] Cart persists across restarts
- [ ] Order submission works
- [ ] Error messages display correctly
- [ ] Code follows C# conventions
- [ ] All files documented

---

**Last Updated**: November 2024
**Author**: Student
**Version**: 1.0.0

# FFXIVLazyStore

FFXIVLazyStore 是一個基於 .NET 8.0 和 Blazor 的網頁應用程式，用於管理 Final Fantasy XIV 的虛擬商店。

- 收藏商品
- 無限滾動自動讀取剩餘商品
- 導向灰機與官方連結

### 先決條件

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) 或 [Visual Studio Code](https://code.visualstudio.com/)

### 專案依賴
- Radzen.Blazor
- Pomelo.EntityFrameworkCore.MySql

### 建置與執行

1. 克隆這個儲存庫：
    ```sh
    git clone https://github.com/dks50217/FFXIVLazyStore.git
    cd FFXIVLazyStore
    ```

2. 透過命令列建置專案：
    ```sh
    dotnet build src/FFXIVLazyStore/FFXIVLazyStore.sln
    ```

3. 執行應用程式：
    ```sh
    dotnet run --project src/FFXIVLazyStore/FFXIVLazyStore/FFXIVLazyStore.csproj
    ```

### TODO

## 資料庫設定 - 後續加入驗證

使用以下命令來生成資料庫上下文：
```sh
dotnet ef dbcontext scaffold "Server={ServerIP};Port=3306;Database=houseofsnow;Uid=root;Pwd={YourPassword};TreatTinyAsBoolean=true" Pomelo.EntityFrameworkCore.MySql -o Model --force
```
# FFXIVLazyTool

FFXIVLazyTool 是一個基於 .NET 8.0 和 Blazor 的懶人用網頁應用程式，用於管理 Final Fantasy XIV 的虛擬商店等功能

- 收藏商品
- 無限滾動自動讀取剩餘商品
- 導向灰機與官方連結
- 幻巧工具
- AI小幫手

### 先決條件

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) 或 [Visual Studio Code](https://code.visualstudio.com/)

### 專案依賴
- Radzen.Blazor
- Pomelo.EntityFrameworkCore.MySql

## 資料來源
- [PaissaDB](https://zhu.codes/paissa)
- [NetStone](https://github.com/xivapi/NetStone)
- [FFXIVStore](https://store.finalfantasyxiv.com/ffxivstore/en-us/)
- [FauxHollowsProbabilisticSolver](https://github.com/Sturalke/FauxHollowsProbabilisticSolver)

### 建置與執行

1. Clone這個儲存庫：
    ```sh
    git clone https://github.com/dks50217/FFXIVLazyTool.git
    cd FFXIVLazyStore
    ```

2. 透過命令列建置專案：
    ```sh
    dotnet build src/FFXIVLazyStore/FFXIVLazyTool.sln
    ```

3. `Appsetting.json` 設定環境變數

   ```json
   "AzureInference": {
        "Model": "openai/gpt-4.1", // or other model
        "ApiKey": "", // Github models key - free
        "Endpoint": "https://models.github.ai/inference"
    },
    "Home": {
        "FreeCompany": "", // Free Company name
        "World": "" // Bahamut or others
    }
   ```

4. 執行應用程式：
    ```sh
    dotnet run --project src/FFXIVLazyStore/FFXIVLazyStore/FFXIVLazyStore.csproj
    ```

---

FINAL FANTASY is a registered trademark of Square Enix Holdings Co., Ltd.

FINAL FANTASY XIV © SQUARE ENIX CO., LTD.

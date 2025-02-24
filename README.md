# TOTool (C# Version)
PlayTrickster Bot Tool - A memory reading and automation tool written in C#.

> This project was developed using [Cursor Editor](https://cursor.sh/)

## Features | 功能特點
- In-game overlay (Toggle with HOME key) | 遊戲內覆蓋界面 (按 HOME 鍵呼出)
- Memory reading and monitoring | 記憶體讀取與監控
- Modern WPF interface | 現代化 WPF 界面
- Hotkey support | 熱鍵支援
- Configuration saving | 配置保存

## Requirements | 系統需求
- Windows 10/11
- .NET 8.0 SDK | .NET 8.0 SDK
- Administrator privileges | 管理員權限

## Development Setup | 開發環境設置
1. Install required tools | 安裝必要工具
```bash
# Install .NET 8.0 SDK | 安裝 .NET 8.0 SDK
# Download from | 從以下網址下載
# https://dotnet.microsoft.com/download

# Verify installation | 驗證安裝
dotnet --version
```

2. Build project | 建置專案
```bash
# Restore packages | 還原套件
dotnet restore

# Build project | 建置專案
dotnet build

# Run tests | 運行測試
dotnet test
```

3. Publish executable | 發布執行檔
```bash
cd src/TOTool.UI
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## Project Structure | 專案結構
```
TOTool.Solution/
├── src/
│   ├── TOTool.Core/        # Core functionality | 核心功能實現
│   │   ├── Memory/         # Memory operations | 記憶體操作相關
│   │   ├── Patterns/       # Memory signatures | 記憶體特徵碼
│   │   └── Utilities/      # Common utilities | 通用工具類
│   │
│   ├── TOTool.UI/          # User interface | 使用者界面
│   │   ├── Windows/        # WPF windows | WPF 窗體
│   │   ├── Controls/       # Custom controls | 自定義控件
│   │   └── ViewModels/     # MVVM view models | MVVM 視圖模型
│   │
│   └── TOTool.Common/      # Shared components | 共用組件
│       ├── Models/         # Data models | 數據模型
│       └── Interfaces/     # Interfaces | 介面定義
│
└── tests/                  # Unit tests | 單元測試
    ├── TOTool.Core.Tests/
    └── TOTool.UI.Tests/
```

## Usage | 使用方式
1. Run as administrator | 以管理員權限執行
2. Press HOME to toggle overlay | 按 HOME 鍵切換覆蓋界面
3. Configure settings in UI | 在界面中配置設定

## Contributing | 貢獻指南
1. Fork the repository | 複製儲存庫
2. Create feature branch | 建立功能分支
3. Commit changes | 提交更改
4. Push to branch | 推送到分支
5. Create pull request | 建立拉取請求

## License | 授權
MIT License | MIT 授權

## Disclaimer | 免責聲明
This tool is for educational purposes only. | 本工具僅供學習用途。
Use at your own risk. | 使用風險自負。

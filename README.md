# 开源安卓投屏（Windows EXE）项目

本仓库用于从零开始开发一款**开源安卓投屏软件**，目标是：
- **仅提供 Windows 客户端（EXE 便携版）**，方便在任意电脑直接运行；
- **同时支持 USB 与无线**连接；
- **兼容多版本 Android 设备**，尽量减少手机端安装与配置步骤；
- **云端开发优先**：构建、测试、打包均可在 CI/CD 完成。

## 快速开始（Windows）
> 目前处于实施阶段，已加入最小可运行的诊断命令用于验证构建链路。

### 构建
```bash
dotnet build client/Screencast.Client/Screencast.Client.csproj -c Release
```

### 运行（诊断 ADB）
```bash
client/Screencast.Client/bin/Release/net8.0/Screencast.Client.exe doctor
```

详细需求与架构草案见：`SPEC.md`。

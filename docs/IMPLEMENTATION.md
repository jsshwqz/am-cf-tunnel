# 实施计划（阶段 1）

## 目标
- 提供可构建的 Windows EXE 客户端骨架。
- 验证 ADB 可用性与基础设备探测流程。

## 已完成
- 初始化 .NET 客户端骨架（`Screencast.Client`）。
- 添加 `doctor` 命令检查 ADB 可用性并输出设备列表。
- CI：Windows 构建流程。

## 下一步
1. 设备发现
   - USB：基于 `adb devices` 解析设备列表。
   - Wi‑Fi：同网段扫描 + ADB 配对流程。
2. 画面链路
   - 先集成 `adb exec-out` + H.264 流拉取。
   - Windows 端使用 FFmpeg/MediaFoundation 解码。
3. 输入控制
   - 鼠标/键盘映射到 ADB input。
   - 快捷键映射：返回/主页/多任务。
4. 文件传输
   - `adb push` + `adb install` 与拖拽事件绑定。


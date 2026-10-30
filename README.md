# 🚀 hello-dotnet

[![.NET CI/CD](https://github.com/h1z1qqe/hello-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/h1z1qqe/hello-dotnet/actions/workflows/ci.yml)
[![Release](https://img.shields.io/github/v/release/h1z1qqe/hello-dotnet?label=release)](https://github.com/h1z1qqe/hello-dotnet/releases/latest)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Platforms](https://img.shields.io/badge/platforms-Linux%20%7C%20macOS%20%7C%20Windows-blue)](https://github.com/h1z1qqe/hello-dotnet/releases/latest)

> Учебный проект: консольное приложение на **C# / .NET 8** с полным циклом **CI/CD** на **GitHub Actions**.
> Один runner собирает **5 самодостаточных бинарников** (Linux, macOS, Windows — x64 и ARM64) и автоматически публикует их в [GitHub Releases](https://github.com/h1z1qqe/hello-dotnet/releases) при push тега `v*`.

---

## ✨ Что демонстрирует проект

| Технология | Как используется |
|---|---|
| **dotnet CLI** | `build`, `test`, `publish`, `format` |
| **xUnit** | модульные тесты логики приложения |
| **Cross-compilation** | один Ubuntu-runner собирает под все платформы |
| **Runtime Identifiers (RID)** | `linux-x64`, `win-x64`, `osx-arm64` и др. |
| **Self-contained + PublishSingleFile** | один файл, не требует установленного .NET |
| **Матричные сборки** | 5 бинарников собираются параллельно |
| **softprops/action-gh-release** | автоматическая публикация релизов |
| **Семантическое версионирование** | релизы по тегам `v0.1.0`, `v0.2.0` |

## ⚙️ Как работает CI/CD

### Схема пайплайна

**CI — проверка кода** (каждый push в `main` и каждый Pull Request):

```text
push в main / PR
      │
      ▼
Job: test  (ubuntu-latest)
      ├─ checkout + установка .NET 8 SDK
      ├─ кэширование NuGet-пакетов
      ├─ dotnet format --verify-no-changes   ← проверка форматирования
      ├─ dotnet build --configuration Release
      └─ dotnet test                          ← xUnit-тесты
```

**CD — сборка и публикация** (только при push тега `v*`):

```text
push тега v* (например, v0.1.0)
      │
      ▼
Job: test ──(успешно)──▶ Job: release   [needs: test]
                          │  matrix — 5 сборок параллельно:
                          ├─ linux-x64    ├─ linux-arm64
                          ├─ win-x64      ├─ osx-x64
                          └─ osx-arm64
                          │
                          │  каждая сборка:
                          │  dotnet publish -r <RID> --self-contained true
                          │  -p:PublishSingleFile=true
                          ▼
              GitHub Release + 5 бинарников
              (softprops/action-gh-release@v2)
```

### Триггеры workflow

| Событие | Job `test` (CI) | Job `release` (CD) |
|---|---|---|
| Push в ветку `main` | ✅ формат + сборка + тесты | ⏭️ пропускается |
| Pull Request | ✅ формат + сборка + тесты | ⏭️ пропускается |
| Push тега `v*` | ✅ формат + сборка + тесты | 🚀 5 бинарников + релиз |

### Матрица сборки (5 платформ)

| RID | Платформа | Файл в релизе |
|---|---|---|
| `linux-x64` | Linux (Intel/AMD) | `hello-dotnet-linux-x64` |
| `linux-arm64` | Linux (ARM-серверы) | `hello-dotnet-linux-arm64` |
| `win-x64` | Windows (Intel/AMD) | `hello-dotnet-windows-x64.exe` |
| `osx-x64` | macOS (Intel) | `hello-dotnet-macos-x64` |
| `osx-arm64` | macOS (Apple Silicon) | `hello-dotnet-macos-arm64` |

## 📁 Структура проекта

```text
hello-dotnet/
├── .github/
│   └── workflows/
│       └── ci.yml              # CI/CD pipeline
├── src/                        # исходный код приложения
│   ├── HelloDotnet.csproj      # проект + <Version> (версия — в одном месте)
│   ├── Greeting.cs             # логика: Greet(), SumRange()
│   └── Program.cs              # точка входа CLI
├── tests/                      # xUnit-тесты
│   ├── HelloDotnet.Tests.csproj
│   └── GreetingTests.cs
├── .gitignore
└── README.md
```

## 📥 Скачать и запустить

Бинарники **self-contained** (~70 МБ): .NET-runtime уже внутри — устанавливать ничего не нужно.
Все версии: [github.com/h1z1qqe/hello-dotnet/releases](https://github.com/h1z1qqe/hello-dotnet/releases)

**Linux (x64):**

```shell
wget https://github.com/h1z1qqe/hello-dotnet/releases/latest/download/hello-dotnet-linux-x64 -O hello-dotnet
chmod +x hello-dotnet
./hello-dotnet
```

**macOS (Apple Silicon):**

```shell
curl -L https://github.com/h1z1qqe/hello-dotnet/releases/latest/download/hello-dotnet-macos-arm64 -o hello-dotnet
chmod +x hello-dotnet
./hello-dotnet
```

**Windows (PowerShell):**

```powershell
Invoke-WebRequest -Uri "https://github.com/h1z1qqe/hello-dotnet/releases/latest/download/hello-dotnet-windows-x64.exe" -OutFile "hello-dotnet.exe"
.\hello-dotnet.exe
```

> 💡 **macOS:** при первом запуске сработает Gatekeeper. Обход: Системные настройки → Конфиденциальность и безопасность → **Открыть всё равно**.

### Пример вывода

```shell
$ ./hello-dotnet
hello-dotnet version 0.1.0
Hello from C# in GitHub Actions! 🚀📦
OS: Linux 6.5.0-27-generic
Arch: X64
Hello, GitHub!
Sum 1..10 = 55

$ ./hello-dotnet привет мир
hello-dotnet version 0.1.0
Hello from C# in GitHub Actions! 🚀📦
OS: Linux 6.5.0-27-generic
Arch: X64
Hello, GitHub!
Sum 1..10 = 55
Аргументы:
  1: привет
  2: мир
```

Значения `OS` и `Arch` зависят от машины, на которой запущен бинарник.

## 🛠 Локальная разработка

Достаточно **Docker** — .NET SDK запускается в контейнере, на хосте ничего ставить не нужно.

### 🧪 Тесты

```shell
docker run --rm \
  -u "$(id -u):$(id -g)" \
  -e HOME=/tmp \
  -e DOTNET_CLI_HOME=/tmp \
  -e NUGET_PACKAGES=/tmp/nuget \
  -e DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  -e DOTNET_NOLOGO=1 \
  -v "$(pwd)":/app \
  -w /app \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  dotnet test tests/HelloDotnet.Tests.csproj
```

Ожидаемый результат: `Passed! - Failed: 0, Passed: 2, Skipped: 0, Total: 2`

### 🎨 Форматирование

CI проверяет код командой `dotnet format --verify-no-changes` — при нарушениях workflow падает. Исправить локально:

```shell
docker run --rm \
  -u "$(id -u):$(id -g)" \
  -e HOME=/tmp \
  -e DOTNET_CLI_HOME=/tmp \
  -e NUGET_PACKAGES=/tmp/nuget \
  -e DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  -e DOTNET_NOLOGO=1 \
  -v "$(pwd)":/app \
  -w /app \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  sh -c "dotnet format src/HelloDotnet.csproj && dotnet format tests/HelloDotnet.Tests.csproj"
```

### 📦 Сборка бинарника

```shell
docker run --rm \
  -u "$(id -u):$(id -g)" \
  -e HOME=/tmp \
  -e DOTNET_CLI_HOME=/tmp \
  -e NUGET_PACKAGES=/tmp/nuget \
  -e DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  -e DOTNET_NOLOGO=1 \
  -v "$(pwd)":/app \
  -w /app \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  dotnet publish src/HelloDotnet.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o dist/linux-x64
```

Проверка собранного файла в чистом Debian (без .NET):

```shell
docker run --rm -v "$(pwd)/dist/linux-x64":/dist debian:stable-slim /dist/hello-dotnet
```

## 🔄 Версионирование и релизы

Версионирование — по [SemVer](https://semver.org/) (`MAJOR.MINOR.PATCH`):

| Что изменили | Тип | Пример |
|---|---|---|
| Исправили баг | PATCH | `0.1.0` → `0.1.1` |
| Добавили функцию | MINOR | `0.1.0` → `0.2.0` |
| Сломали совместимость | MAJOR | `0.1.0` → `1.0.0` |

Версия хранится **в одном месте** — в `src/HelloDotnet.csproj` (тег `<Version>`). `Program.cs` читает её из атрибута сборки автоматически, больше нигде менять не нужно.

### Чек-лист релиза

```shell
# 1. Внести изменения в код (src/Program.cs)
# 2. Обновить <Version> в src/HelloDotnet.csproj
# 3. Проверить формат и тесты (Docker-команды выше)
# 4. Коммит и пуш
git add .
git commit -m "feat: bump version to 0.2.0"
git push origin main
# 5. Тег → GitHub Actions соберёт 5 бинарников и опубликует релиз
git tag v0.2.0
git push origin v0.2.0
```

> ⚠️ Релизы в GitHub неизменяемы: перезаписать файлы внутри `v0.1.0` нельзя — для нового кода выпускается новая версия. Старые релизы остаются доступны для скачивания.

### Если ошиблись в теге

```shell
git tag -d v0.2.0                  # удалить тег локально
git push origin :refs/tags/v0.2.0  # удалить тег на GitHub
# затем удалить Release на GitHub (вкладка Releases → Delete)
# и повторить шаги после исправления кода
```

## 🐍 Почему .NET, а не Python

| | Python | .NET |
|---|---|---|
| Cross-compilation | ❌ нужен runner под каждую ОС | ✅ один runner собирает под все платформы |
| Где хранится версия | в двух файлах | в одном `.csproj` |

> **Главный урок:** .NET сочетает простоту cross-compilation (как Go) с корпоративной экосистемой (как Java). Один runner — 5 бинарников, один файл версии — никаких рассинхронов.

## 🧹 Шпаргалка

| Команда | Что делает |
|---|---|
| `dotnet build` | сборка проекта |
| `dotnet test` | запуск xUnit-тестов |
| `dotnet format --verify-no-changes` | проверка форматирования (как в CI) |
| `dotnet publish -r <RID> --self-contained true -p:PublishSingleFile=true` | один бинарник под платформу `<RID>` |
| `git tag v0.2.0 && git push origin v0.2.0` | выпустить релиз |
| `git push origin :refs/tags/v0.2.0` | удалить тег с GitHub |

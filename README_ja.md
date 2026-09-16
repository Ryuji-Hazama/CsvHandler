# CSV Handler

[日本語](README_ja.md) | [English](README.md)

このREADMEファイルでは、`C#`プログラミング言語用の`CSV Handler`ライブラリについて説明します。

ちなみに、この`README`ファイルが退屈かどうかは気にしません :zany_face:

## :memo: 概要

`CSV Handler`は、CSV（カンマ区切り値）ファイルの読み書きを容易にするために設計された`C#`ライブラリです。CSV データを簡単に処理する方法を提供し、開発者がアプリケーション内で表形式のデータを簡単に操作およびプロセスできるようにします。

### :sparkles: 主な機能

- **CSVファイルの読み込み**: CSVファイルからデータを構造化形式にロードして、簡単にアクセスして操作できます。
- **CSVファイルの書き込み**: CSVファイルにデータを保存し、既存ファイルを上書きまたは追加するオプションがあります。

## :arrow_down: リポジトリのクローン

一時的に `git bundle` を使用してリポジトリを管理していますが、将来的には通常の `git` リポジトリに切り替える予定です。以下のコマンドを使用してリポジトリをクローンしてください。

```bash
git clone /path/to/csv_handler.bundle csv_handler
```

## :package: プロジェクトにライブラリをインポートする

`C#`プロジェクトで`CSV Handler`ライブラリを使用するには、以下の手順に従ってください。

### :arrow_down: Aras Innovator サーバーメソッドにライブラリをインポート

1. **ライブラリをダウンロード**: `CSV Handler`ライブラリファイルまたはコンパイル済みDLLが利用可能であることを確認します。
2. **ライブラリを配置**: ライブラリファイルまたはDLLをAras Innovatorサーバーがアクセスできるディレクトリにコピーします。
    - DLLを使用している場合は、Aras Innovatorインストールの `bin` ディレクトリに配置します。
    - ソースファイルを使用している場合は、プロジェクトに含めて適宜コンパイルされていることを確認してください。
3. **ライブラリを参照**: Aras Innovatorサーバーメソッドで、`CSV Handler`ライブラリへの参照を追加します。
    - DLLを使用する場合は、`C#`コードファイルの先頭に `using` ディレクティブを追加することで、メソッドコード内で参照できます。

      ```csharp
      using CsvHandler; // `CSV Handler`ライブラリの実際の名前空間に置き換えてください
      ```

    - ソースファイルを使用している場合は、メソッドコード内で名前空間が正しく参照されていることを確認してください。

### :arrow_down: ローカル`C#`プロジェクトにライブラリをインポート

1. **ライブラリをダウンロード**: `CSV Handler`ライブラリファイルまたはコンパイル済みDLLが利用可能であることを確認します。
2. **参照を追加**: ローカル`C#`プロジェクトで、`CSV Handler`ライブラリへの参照を追加します。
    - Visual Studioを使用している場合:
        - ソリューションエクスプローラーでプロジェクトを右クリックします。
        - 「追加」> 「参照...」を選択します。
        - `CSV Handler` DLLの場所を参照して追加します。
        - または、ソリューションにソースコードがある場合はプロジェクト参照を追加します。
    - 別のIDE（Code、Riderなど）を使用している場合:
        - プロジェクトファイル（例：`.csproj`）を編集して、`CSV Handler`ライブラリへの参照を含めます。
        - `.csproj` の例：

          ```xml
          <ItemGroup>
              <Reference Include="CsvHandler">
                  <HintPath>path\to\CsvHandler.dll</HintPath>
              </Reference>
          </ItemGroup>
          ```

            または

          ```xml
          <ItemGroup>
              <ProjectReference Include="path\to\CsvHandlerProject.csproj" />
          </ItemGroup>
          ```

3. **ライブラリを使用**: `C#`コードファイルで、`CSV Handler`の機能にアクセスするために必要な `using` ディレクティブを含めます。

   ```csharp
   using CsvHandler; // `CSV Handler`ライブラリの実際の名前空間に置き換えてください
   ```

## :gear: 使用方法

### :open_file_folder: 初期化

```csharp
class CsvHandler(
    string file_name,
    Encoding? encoding,
    Delimiter? delimiter,
)
```

|パラメーター|型|デフォルト|説明|
|---|---|---|---|
|**`file_name`**|`string`|なし|読み込みまたは書き込みするCSVファイルの名前。|
|**`encoding`**|`Encoding?`|`null`|CSVファイルを読み込みまたは書き込みするときに使用する文字エンコーディング。|
|**`delimiter`**|`Delimiter?`|`null`|CSVファイルの値を区切るために使用される文字。|

例：

```csharp
ICsvHandler csvHandler = new CsvHandler("data.csv", Encoding.UTF8, Delimiter.Comma);
```

### :outbox_tray: CSVファイルの読み込み

```csharp
List<List<string>> Read(
    bool header
)
```

|パラメーター|型|デフォルト|説明|
|---|---|---|---|
|**`header`**|`bool`|`false`|最初の行がヘッダーであり、スキップされるべきかどうかを示します。|

例：

```csharp
using CsvHandler;

class Program
{
    static void Main()
    {
        var csvReader = new CsvHandler("path/to/your/file.csv");
        var data = csvReader.Read();

        // 必要に応じてデータを処理します
    }
}
```

### :inbox_tray: CSVファイルの書き込み

```csharp
void Write(
    List<List<string>> data_rows,
    string? new_line,
    WriteMode? write_mode
)
```

|パラメーター|型|デフォルト|説明|
|---|---|---|---|
|**`data_rows`**|`List<List<string>>`|なし|CSVファイルに書き込むデータ。各内部リストは行を表します。|
|**`new_line`**|`string`|`null`|CSVファイルで改行に使用する文字列。nullの場合、デフォルトの改行文字が使用されます。|
|**`write_mode`**|`WriteMode?`|`null`|既存ファイルを上書きするか追加するかを指定します。使用可能な値は、上書きの場合は`WriteMode.Overwrite`、追加の場合は`WriteMode.Append`です。|

例：

```csharp
using CsvHandler;

class Program
{
    static void Main()
    {
        var csvWriter = new CsvHandler("path/to/your/file.csv");
        var dataToWrite = new List<List<string>>
        {
            new List<string> { "Name", "Age", "City" },
            new List<string> { "Alice", "30", "New York" },
            new List<string> { "Bob", "25", "Los Angeles" }
        };

        csvWriter.Write(dataToWrite, Environment.NewLine, WriteMode.Overwrite); // ファイルを新しいデータで上書きします
    }
}
```

## :warning: 例外

|例外|説明|
|---|---|
|`HandlerExceptions`|`CSV Handler`に関連するすべての例外の基本例外クラス。|
|`HandlerExceptions.InvalidCSVException`|CSVファイルが無効であるか、処理できない場合にスローされます。|

## :package: DLLのビルド

`CSV Handler`ライブラリのソースコードからDLLをビルドするには、以下の手順に従ってください。

### :hammer_and_wrench: `Visual Studio` を使用する場合

1. **プロジェクトを開く**: Visual Studioで`CSV Handler`プロジェクトを開きます。
2. **ビルド構成を設定する**:
   - 本番用のDLLを作成するために、ビルド構成を`Release`に設定します。
   - ターゲットフレームワークを希望のバージョンの.NETに設定します（例: `.NET 6.0`, `.NET 7.0`など）。
3. **プロジェクトをビルドする**:
   - `ビルド`メニューに移動し、`ソリューションのビルド`を選択します（または`Ctrl+Shift+B`を押します）。
   - Visual Studioがプロジェクトをコンパイルし、DLLファイルを生成します。（通常は`bin\Release\netX.X`ディレクトリにあります。`X.X`はターゲットフレームワークのバージョンです。）

### :computer: コマンドラインを使用する場合 (dotnet CLI)

1. **コマンドプロンプトまたはターミナルを開く**: `CSV Handler`プロジェクトのルートディレクトリに移動します。
2. **ターゲットフレームワークを設定する**: プロジェクトファイル（`.csproj`）が希望のターゲットフレームワークを指定していることを確認します（例: `<TargetFramework>net6.0</TargetFramework>`）。
3. **ビルドコマンドを実行する**:

    ```bash
    dotnet build -c Release
    ```

## :bulb: 貢献

貢献を歓迎します！改善や新機能について提案がある場合は、お気軽にプルリクエストを送信するか、会社のGitリポジトリで問題を開いてください。

## :email: お問い合わせ

`CSV Handler`ライブラリについてご質問がある場合や、サポートが必要な場合は、会社のGitリポジトリに記載されているメンテナーにお問い合わせください。

*`CSV Handler`ライブラリをご利用いただきありがとうございます！* :wave:  
*コーディングを楽しんでください！* :computer:

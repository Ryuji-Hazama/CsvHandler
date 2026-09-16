# CSV Handler

[Japanese](README_ja.md) | [English](README.md)

I will show you about my `CSV Handler` library for `C#` programming language in this README file.

BTW, I don't care if you are bored or not about this README file :zany_face:

## :memo: Overview

The `CSV Handler` is a `C#` library designed to facilitate reading from and writing to CSV (Comma-Separated Values) files. It provides a simple and efficient way to handle CSV data, allowing developers to easily manipulate and process tabular data in their applications.

### :sparkles: Core Features

- **Read CSV Files**: Load data from CSV files into a structured format for easy access and manipulation.
- **Write CSV Files**: Save data to CSV files, with options to overwrite or append to existing files.

## :arrow_down: Clone the Repository

We are temporarily using a git bundle to manage the repository. But we will switch to a normal git repository in the future. Please clone the repository using the following command:

```bash
git clone /path/to/csv_handler.bundle csv_handler
```

## :package: Importing the Library to Your Project

To use the `CSV Handler` library in your `C#` project, follow these steps:

### :arrow_down: Import Library to Aras Innovator Server Method

1. **Download the Library**: Ensure you have the `CSV Handler` library files or the compiled DLL available.
2. **Place the Library**: Copy the library files or DLL to a directory accessible by your Aras Innovator server.
    - If you are using a DLL, place it in the `bin` directory of your Aras Innovator installation.

    ```text
    ./Server_root (e.g., C:\Program Files\Aras\Innovator\Server)
        ├── bin/
        │   └── csv_handler.dll
        └── ...
    ```

    - If you are using source files, ensure they are included in your project and compiled accordingly.
3. **Reference the Library**: In your Aras Innovator server method, add a reference to the `CSV Handler` library.
    - If using a DLL, you can reference it in your method code by adding a `using` directive at the top of your `C#` code file:

      ```csharp
      using CsvHandler; // Replace with the actual namespace of the `CSV Handler` library
      ```

    - If using source files, ensure that the namespace is correctly referenced in your method code.

### :arrow_down: Import Library to Your Local `C#` Project

1. **Download the Library**: Ensure you have the `CSV Handler` library files or the compiled DLL available.
2. **Add Reference**: In your local `C#` project, add a reference to the `CSV Handler` library.
    - If you are using Visual Studio:
        - Right-click on your project in the Solution Explorer.
        - Select "Add" > "Reference..."
        - Browse to the location of the `CSV Handler` DLL and add it.
        - Or add project reference if you have the source code in your solution.
    - If you are using a different IDE (e.g., Code, Rider, etc.):
        - Edit your project file (e.g., `.csproj`) to include a reference to the `CSV Handler` library.
        - Example for `.csproj`:

          ```xml
          <ItemGroup>
              <Reference Include="CsvHandler">
                  <HintPath>path\to\CsvHandler.dll</HintPath>
              </Reference>
          </ItemGroup>
          ```

            or

          ```xml
          <ItemGroup>
              <ProjectReference Include="path\to\CsvHandlerProject.csproj" />
          </ItemGroup>
          ```

3. **Using the Library**: In your `C#` code files, include the necessary `using` directive to access the `CSV Handler` functionality:

   ```csharp
   using CsvHandler; // Replace with the actual namespace of the `CSV Handler` library
   ```

## :gear: Usage

### :open_file_folder: Initialization

```csharp
class CsvHandler(
    string file_name,
    Encoding? encoding,
    Delimiter? delimiter,
)
```

|Parameter|Type|Default|Description|
|---|---|---|---|
|**`file_name`**|`string`|N/A|The name of the CSV file to read from or write to.|
|**`encoding`**|`Encoding?`|`null`|The character encoding to use when reading or writing the CSV file.|
|**`delimiter`**|`Delimiter?`|`null`|The character used to separate values in the CSV file.|

example:

```csharp
ICsvHandler csvHandler = new CsvHandler("data.csv", Encoding.UTF8, Delimiter.Comma);
```

### :outbox_tray: Reading CSV Files

```csharp
List<List<string>> Read(
    bool header
)
```

|Parameter|Type|Default|Description|
|---|---|---|---|
|**`header`**|`bool`|`false`|Indicates whether the first row is a header and should be skipped.|

example:

```csharp
using CsvHandler;

class Program
{
    static void Main()
    {
        var csvReader = new CsvHandler("path/to/your/file.csv");
        var data = csvReader.Read();

        // Process the data as needed
    }
}
```

### :inbox_tray: Writing CSV Files

```csharp
void Write(
    List<List<string>> data_rows,
    string? new_line,
    WriteMode? write_mode
)
```

|Parameter|Type|Default|Description|
|---|---|---|---|
|**`data_rows`**|`List<List<string>>`|N/A|The data to be written to the CSV file, where each inner list represents a row.|
|**`new_line`**|`string`|`null`|The string to use for new lines in the CSV file. If null, the default new line character will be used.|
|**`write_mode`**|`WriteMode?`|`null`|Specifies whether to overwrite the existing file or append to it. Acceptable values are `WriteMode.Overwrite` for overwrite and `WriteMode.Append` for append.|

example:

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

        csvWriter.Write(dataToWrite, Environment.NewLine, WriteMode.Overwrite); // Overwrite the file with new data
    }
}
```

## :warning: Exceptions

|Exception|Description|
|---|---|
|`HandlerExceptions`|Base exception class for all exceptions related to the `CSV Handler`.|
|`HandlerExceptions.InvalidCSVException`|Thrown when the CSV file is invalid or cannot be processed.|

## :package: Build a DLL

To build a DLL from the `CSV Handler` library source code, follow these steps:

### :hammer_and_wrench: Using Visual Studio

1. **Open the Project**: Open the `CSV Handler` project in Visual Studio.
2. **Set the Build Configuration**:
   - Set the build configuration to `Release` for a production-ready DLL.
   - Set the target framework to the desired version of .NET (e.g., .NET 6.0, .NET 7.0, etc.).
3. **Build the Project**:
   - Go to the `Build` menu and select `Build Solution` (or press `Ctrl+Shift+B`).
   - Visual Studio will compile the project and generate the DLL file. (Usually located in the `bin\Release\netX.X` directory, where `X.X` is the target framework version.)

### :computer: Using Command Line (dotnet CLI)

1. **Open Command Prompt or Terminal**: Navigate to the root directory of the `CSV Handler` project.
2. **Set the Target Framework**: Ensure that the project file (`.csproj`) specifies the desired target framework (e.g., `<TargetFramework>net6.0</TargetFramework>`).
3. **Run the Build Command**:

    ```bash
    dotnet build -c Release
    ```

## :bulb: Contributing

Contributions are welcome! If you have suggestions for improvements or new features, please feel free to submit a pull request or open an issue on the company Git repository.

## :email: Contact

For any questions or support regarding the `CSV Handler` library, please contact the maintainer named in the company Git repository.

*Thank you for using the `CSV Handler` library!* :wave:  
*Happy coding!* :computer:

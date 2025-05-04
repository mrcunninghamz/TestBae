# TestBae
A set of base test classes that take care of mocking / dependency injection / automapper

## Installation

TestBae is available as a NuGet package. You can add it to your project using one of the following methods:

### Using .NET CLI

```bash
dotnet add package TestBae
```

### Using Package Manager Console in Visual Studio

```powershell
Install-Package TestBae
```

### Using PackageReference in your project file

```xml
<PackageReference Include="TestBae" Version="0.0.2" />
```

### Using Visual Studio

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages..."
3. Switch to the "Browse" tab
4. Search for "TestBae"
5. Click "Install"

## Base Classes

TestBae provides several base test classes to simplify unit testing with different approaches:

| Name | Description |
|------|-------------|
| [BaseTest\<TSubject>](docs/BaseTest.md) | Traditional base test class with manual dependency injection and mocking. Best for fine-grained control over dependencies and explicit setup. |
| [AutoFixture.BaseTest\<TSubject>](docs/AutoFixtureBaseTest.md) | Uses AutoFixture to automate test data generation and dependency injection. Ideal for reducing boilerplate code and handling complex object graphs. |
| [EntityFramework.BaseTest\<TSubject>](docs/EntityFrameworkBaseTest.md) | Base test class designed for testing Entity Framework-based components with an in-memory SQLite database. Provides database context setup and transaction management. |
| [AutoFixture.EntityFramework.BaseTest\<TSubject, TDbContext>](docs/AutoFixtureEntityFrameworkBaseTest.md) | Combines AutoFixture's test data generation with Entity Framework testing capabilities. Perfect for testing components that need both automated dependency mocking and database interactions. |

### Creating Local NuGet Packages

To create a local NuGet package for testing:

1. Build the project in Release mode:
   ```
   dotnet build -c Release
   ```

2. Pack the project to create a NuGet package:
   ```
   dotnet pack -c Release -o [LOCAL_PACKAGES_PATH]
   ```

   Replace `[LOCAL_PACKAGES_PATH]` with your platform-specific path:
   - Windows: `C:\LocalPackages`
   - macOS/Linux: `~/LocalPackages`

This will create a `.nupkg` file in your LocalPackages directory.

### Setting Up Local NuGet Feed

1. Create a LocalPackages directory in a location accessible to all your projects:
   
   **Windows:**
   ```
   mkdir C:\LocalPackages
   ```
   
   **macOS/Linux:**
   ```
   mkdir ~/LocalPackages
   ```

2. Edit or create a `nuget.config` file in your .nuget folder (typically `~/.nuget/NuGet/` on macOS/Linux or `%APPDATA%\NuGet\` on Windows) with the following content:
   
   **Windows:**
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <packageSources>
       <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
       <add key="LocalPackages" value="C:\LocalPackages" />
     </packageSources>
   </configuration>
   ```
   
   **macOS/Linux:**
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <packageSources>
       <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
       <add key="LocalPackages" value="~/LocalPackages" />
     </packageSources>
   </configuration>
   ```

3. When you reference the package in your projects, NuGet will now look in your LocalPackages directory as well as the standard NuGet feeds.

### Using Local Packages in Your Projects

Add a package reference to your project file:

```xml
<PackageReference Include="TestBae" Version="1.0.0" />
```

When you run `dotnet restore`, it will check the local feed before looking online.

### Updating Local Packages

To update your local package:

1. Increment the version in the project file:
   ```xml
   <PropertyGroup>
     <Version>1.0.1</Version>
   </PropertyGroup>
   ```

2. Rebuild and repack the project:
   
   For stable releases:
   ```
   dotnet build -c Release
   dotnet pack -c Release -o [LOCAL_PACKAGES_PATH]
   ```
   
   For prerelease versions, use the --version-suffix parameter with epoch timestamp:
   ```
   # Get current epoch timestamp
   TIMESTAMP=$(date +%s)
   
   # For macOS/Linux
   dotnet build -c Release
   dotnet pack -c Release --version-suffix "beta-$TIMESTAMP" -o ~/LocalPackages
   
   # For Windows (PowerShell)
   # $timestamp = [Math]::Floor([decimal](Get-Date(Get-Date).ToUniversalTime()-UFormat "%s"))
   # dotnet pack -c Release --version-suffix "beta-$timestamp" -o C:\LocalPackages
   ```
   
   This will create a package with version like `1.0.1-beta-1713887619` which includes the epoch timestamp.
   You can also combine predefined prefixes with the timestamp: alpha, preview, rc

   Replace `[LOCAL_PACKAGES_PATH]` with the appropriate path for your OS:
   - Windows: `C:\LocalPackages`
   - macOS/Linux: `~/LocalPackages`

3. Update your consuming projects to use the new version:
   
   For stable versions:
   ```xml
   <PackageReference Include="TestBae" Version="1.0.1" />
   ```
   
   For prerelease versions:
   ```xml
   <PackageReference Include="TestBae" Version="1.0.1-beta-1713887619" />
   ```

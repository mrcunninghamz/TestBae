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
<PackageReference Include="TestBae" Version="0.0.1" />
```

### Using Visual Studio

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages..."
3. Switch to the "Browse" tab
4. Search for "TestBae"
5. Click "Install"

## Classes

### BaseTest
This is an abstract class used in unit tests class to automatically setup automapper and puts your mocks into dependency injection.

#### Setup
In your unit test class reference the class you are testing. In this example the `PermissionsController` is the class being tested.

```c#
public class PermissionsControllerTests : BaseTest<PermissionsController>
```

The `BaseTest` class will look for automapper profiles in the assembly the `PermissionsController` comes from.

Next, override the `ConfigureServices` method.

```c#
protected override void ConfigureServices(IServiceCollection services)
        {
            _mockExternalEntityManager = new Mock<IExternalEntityManager>();
            _microsoftGraphUserAdapterMock = new Mock<IMicrosoftGraphUserAdapter>();
            _mockLogger = new Mock<ILogger<PermissionsController>>();

            //these are mocks for the controller
            services.AddTransient(_ => _mockExternalEntityManager.Object);
            services.AddTransient(_ => _microsoftGraphUserAdapterMock.Object);
            services.AddTransient(_ => _mockLogger.Object);

            //this is stuff that AutoMapper cares about
            _httpcontext = new DefaultHttpContext();
            var _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(_httpcontext);

            services.AddSingleton(_httpContextAccessor.Object);
            services.AddSingleton<PagingResponseConverter<ExternalEntity, ExternalEntity>>();

            base.ConfigureServices(services);
        }
```

All the mocks added to the `services` will be injected into the test class and automapper (if it needs it).
The base class will then create an instance of the `PermissionsController` and make it available in the `TestSubject` property of the class.


An example test:
```c#
[Fact]
public async Task GetGroupsByUser_UserExists_ReturnsOK()
{
    //Arrange
    var groups = new List<string> { "GroupA", "GroupB", "GroupC" };
    var objectId = "0e14e273-4282-4e5f-a368-4b0a9f266be5";

    _microsoftGraphUserAdapterMock.Setup(x => x.GetAssignedGroupsAsync(It.IsAny<string>())).ReturnsAsync(groups);

    //Act
    var response = await TestSubject.GetGroupsByUser(objectId);

    //Assert
    Assert.IsType<ActionResult<GroupAssignmentResponse>>(response);
    Assert.NotNull(response.Value.Groups);
    Assert.True(response.Value.Groups.Count == 3);
}
```
#### Properties

- TestSubject
  - This is the class you are testing setup by the base class and the dependencies that were configured.
- Mapper
  - An instance of automapper setup by the base class and the dependencies that were configured.
- ServiceProvider
  - The service provider with all the dependencies.

## Local NuGet Package Development

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

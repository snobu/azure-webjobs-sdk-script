# escape=`

# Installer image
FROM microsoft/windowsservercore:1709 AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
RUN iwr https://github.com/Azure/azure-webjobs-sdk-script/archive/dev.zip -OutFile dev.zip; `
    Expand-Archive dev.zip .; `
    cd azure-webjobs-sdk-script-dev;

# SDK image
FROM microsoft/dotnet:2.0-sdk-nanoserver-1709

COPY --from=installer-env ["azure-webjobs-sdk-script-dev", "C:\\azure-webjobs-sdk-script-dev"]

RUN cd azure-webjobs-sdk-script-dev && `
    dotnet restore WebJobs.Script.sln && `
    mkdir src\WebJobs.Script.Grpc\Messages\DotNet && `
    cd src\WebJobs.Script.Grpc && `
    "%UserProfile%\.nuget\packages\grpc.tools\1.4.1\tools\windows_x64\protoc.exe" Proto\FunctionRpc.proto --csharp_out Messages\DotNet --grpc_out=Messages\DotNet --plugin=protoc-gen-grpc="%UserProfile%\.nuget\packages\grpc.tools\1.4.1\tools\windows_x64\grpc_csharp_plugin.exe" --proto_path=Proto && `
    cd ..\.. && `
    dotnet build WebJobs.Script.sln && `
    dotnet publish src\WebJobs.Script.WebHost\WebJobs.Script.WebHost.csproj --output C:\runtime

ENV AzureWebJobsScriptRoot=C:\approot

CMD ["dotnet", "C:\\runtime\\Microsoft.Azure.WebJobs.Script.WebHost.dll"]

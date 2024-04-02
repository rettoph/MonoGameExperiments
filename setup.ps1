Set-Location $PSScriptRoot;

$MonoGameDirectory = "$($PSScriptRoot)/libraries/Guppy/libraries/MonoGame";
$MonoGameBuildVersion = "1.3.3.10";

git submodule update --init --recursive

# Build MonoGame
$WorkingDirectory = Get-Location;
Set-Location $MonoGameDirectory;

if((Test-Path "./build.cake") -eq $true)
{
    dotnet tool restore
    dotnet cake --build-version $MonoGameBuildVersion
}
else {
    dotnet run --project "$($MonoGameDirectory)/build/Build.cspcdroj" -- "--build-version" $MonoGameBuildVersion

    # Why isn't this included in Build.csproj tho?
    dotnet pack -o "$($MonoGameDirectory)/Artifacts/NuGet" "$($MonoGameDirectory)/Tools/MonoGame.Content.Builder.Editor.Windows/MonoGame.Content.Builder.Editor.Windows.csproj"
}

Set-Location $WorkingDirectory;

# Uninstall old tools (if any)
dotnet tool uninstall dotnet-mgcb
dotnet tool uninstall dotnet-mgcb-editor-windows
dotnet tool uninstall dotnet-mgcb-editor

# https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create
# https://learn.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use
dotnet tool install --version "$($MonoGameBuildVersion)-develop" --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb
dotnet tool install --version "$($MonoGameBuildVersion)-develop" --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb-editor-windows
dotnet tool install --version "$($MonoGameBuildVersion)-develop" --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb-editor
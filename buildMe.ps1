$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Set-Location $scriptPath

$solutionFiles = ("F1.PackageDelivery.Web.IN/F1.PackageDelivery.Web.Internal.sln")
$msBuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe /t:Build /p:Configuration=debug /p:Platform=""Any CPU"" ""{0}"""
foreach($file in $solutionFiles)
{    
    $command = [string]::Format($msBuild, $file)
    Invoke-Expression -Command:$command        
}
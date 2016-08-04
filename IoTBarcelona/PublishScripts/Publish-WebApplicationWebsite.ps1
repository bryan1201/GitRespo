#Requires -Version 3.0

<#
.SYNOPSIS
建立及部署 Visual Studio Web 專案的 Microsoft Azure 網站。
如需詳細文件，請移至: http://go.microsoft.com/fwlink/?LinkID=394471 

.EXAMPLE
PS C:\> .\Publish-WebApplicationWebSite.ps1 `
-Configuration .\Configurations\WebApplication1-WAWS-dev.json `
-WebDeployPackage ..\WebApplication1\WebApplication1.zip `
-Verbose

#>
[CmdletBinding(HelpUri = 'http://go.microsoft.com/fwlink/?LinkID=391696')]
param
(
    [Parameter(Mandatory = $true)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $Configuration,

    [Parameter(Mandatory = $false)]
    [String]
    $SubscriptionName,

    [Parameter(Mandatory = $false)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $WebDeployPackage,

    [Parameter(Mandatory = $false)]
    [ValidateScript({ !($_ | Where-Object { !$_.Contains('Name') -or !$_.Contains('Password')}) })]
    [Hashtable[]]
    $DatabaseServerPassword,

    [Parameter(Mandatory = $false)]
    [Switch]
    $SendHostMessagesToOutput = $false
)


function New-WebDeployPackage
{
    #撰寫函式以建置和封裝您的 Web 應用程式

    #若要建置您的 Web 應用程式，請使用 MsBuild.exe。如需說明，請參閱位於 http://go.microsoft.com/fwlink/?LinkId=391339 的＜MSBuild 命令列參考＞
}

function Test-WebApplication
{
    #編輯這個函式，以在您的 Web 應用程式上執行單元測試

    #若要撰寫函式，以便在您的 Web 應用程式上執行單元測試，請使用 VSTest.Console.exe。如需說明，請參閱位於 http://go.microsoft.com/fwlink/?LinkId=391340 的＜VSTest.Console 命令列參考＞
}

function New-AzureWebApplicationWebsiteEnvironment
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword
    )
       
    Add-AzureWebsite -Name $Config.name -Location $Config.location | Out-String | Write-HostWithTime
    # 建立 SQL 資料庫。連接字串可用於部署。
    $connectionString = New-Object -TypeName Hashtable
    
    if ($Config.Contains('databases'))
    {
        @($Config.databases) |
            Where-Object {$_.connectionStringName -ne ''} |
            Add-AzureSQLDatabases -DatabaseServerPassword $DatabaseServerPassword -CreateDatabase |
            ForEach-Object { $connectionString.Add($_.Name, $_.ConnectionString) }           
    }
    
    return @{ConnectionString = $connectionString}   
}

function Publish-AzureWebApplicationToWebsite
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage
    )

    if ($ConnectionString -and $ConnectionString.Count -gt 0)
    {
        Publish-AzureWebsiteProject `
            -Name $Config.name `
            -Package $WebDeployPackage `
            -ConnectionString $ConnectionString
    }
    else
    {
        Publish-AzureWebsiteProject `
            -Name $Config.name `
            -Package $WebDeployPackage
    }
}


# 指令碼主要常式
Set-StrictMode -Version 3
Import-Module Azure

try {
    $AzureToolsUserAgentString = New-Object -TypeName System.Net.Http.Headers.ProductInfoHeaderValue -ArgumentList 'VSAzureTools', '1.4'
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.UserAgents.Add($AzureToolsUserAgentString)
} catch {}

Remove-Module AzureWebSitePublishModule -ErrorAction SilentlyContinue
$scriptDirectory = Split-Path -Parent $PSCmdlet.MyInvocation.MyCommand.Definition
Import-Module ($scriptDirectory + '\AzureWebSitePublishModule.psm1') -Scope Local -Verbose:$false

New-Variable -Name VMWebDeployWaitTime -Value 30 -Option Constant -Scope Script 
New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
New-Variable -Name SendHostMessagesToOutput -Value $SendHostMessagesToOutput -Scope Global -Force

try
{
    $originalErrorActionPreference = $Global:ErrorActionPreference
    $originalVerbosePreference = $Global:VerbosePreference
    
    if ($PSBoundParameters['Verbose'])
    {
        $Global:VerbosePreference = 'Continue'
    }
    
    $scriptName = $MyInvocation.MyCommand.Name + ':'
    
    Write-VerboseWithTime ($scriptName + ' 開始')
    
    $Global:ErrorActionPreference = 'Stop'
    Write-VerboseWithTime ('{0} $ErrorActionPreference 設定為 {1}' -f $scriptName, $ErrorActionPreference)
    
    Write-Debug ('{0}: $PSCmdlet.ParameterSetName = {1}' -f $scriptName, $PSCmdlet.ParameterSetName)

    # 儲存目前訂用帳戶。它將於稍後在指令碼中還原為 Current 狀態
    Backup-Subscription -UserSpecifiedSubscription $SubscriptionName
    
    # 確認您具有 Azure 模組，版本 0.7.4 (含) 以後版本。
    if (-not (Test-AzureModule))
    {
         throw '您的 Microsoft Azure PowerShell 是舊版。若要安裝最新版本，請移至 http://go.microsoft.com/fwlink/?LinkID=320552。'
    }
    
    if ($SubscriptionName)
    {

        # 如果您提供訂用帳戶名稱，請確認訂用帳戶存在帳戶中。
        if (!(Get-AzureSubscription -SubscriptionName $SubscriptionName))
        {
            throw ("{0}: 找不到訂用帳戶名稱 $SubscriptionName" -f $scriptName)

        }

        # 將指定的訂用帳戶設定為目前帳戶。
        Select-AzureSubscription -SubscriptionName $SubscriptionName | Out-Null

        Write-VerboseWithTime ('{0}: 訂用帳戶設定為 {1}' -f $scriptName, $SubscriptionName)
    }

    $Config = Read-ConfigFile $Configuration 

    #建置和封裝您的 Web 應用程式
    New-WebDeployPackage

    #在您的 Web 應用程式上執行單元測試
    Test-WebApplication

    #建立 JSON 組態檔中所述的 Azure 環境
    $newEnvironmentResult = New-AzureWebApplicationWebsiteEnvironment -Configuration $Config -DatabaseServerPassword $DatabaseServerPassword

    #如果 $WebDeployPackage 是由使用者所指定，則部署 Web 應用程式封裝 
    if($WebDeployPackage)
    {
        Publish-AzureWebApplicationToWebsite `
            -Configuration $Config `
            -ConnectionString $newEnvironmentResult.ConnectionString `
            -WebDeployPackage $WebDeployPackage
    }
}
finally
{
    $Global:ErrorActionPreference = $originalErrorActionPreference
    $Global:VerbosePreference = $originalVerbosePreference

    # 將原始目前訂用帳戶還原為 Current 狀態
    Restore-Subscription

    Write-Output $Global:AzureWebAppPublishOutput    
    $Global:AzureWebAppPublishOutput = @()
}

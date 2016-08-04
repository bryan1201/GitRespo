#  AzureWebSitePublishModule.psm1 是一個 Windows PowerShell 指令碼模組。這個模組會匯出 Windows PowerShell 函式，將 Web 應用程式的生命週期管理自動化。您可以依原狀使用這些函式，也可以針對您的應用程式和發行環境自訂這些函式。

Set-StrictMode -Version 3

# 用來儲存原始訂用帳戶的變數。
$Script:originalCurrentSubscription = $null

# 用來儲存原始儲存體帳戶的變數。
$Script:originalCurrentStorageAccount = $null

# 用來儲存使用者指定之訂用帳戶的儲存體帳戶的變數。
$Script:originalStorageAccountOfUserSpecifiedSubscription = $null

# 用來儲存訂用帳戶名稱的變數。
$Script:userSpecifiedSubscription = $null


<#
.SYNOPSIS
在訊息前面加上日期和時間。

.DESCRIPTION
在訊息前面加上日期和時間。這個函式是專為寫入 Error 和 Verbose 資料流的訊息所設計。

.PARAMETER  Message
指定沒有日期的訊息。

.INPUTS
System.String

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Format-DevTestMessageWithTime -Message "將檔案 $filename 加入目錄"
2/5/2014 1:03:08 PM - 將檔案 $filename 加入目錄

.LINK
Write-VerboseWithTime

.LINK
Write-ErrorWithTime
#>
function Format-DevTestMessageWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    return ((Get-Date -Format G)  + ' - ' + $Message)
}


<#

.SYNOPSIS
撰寫前置字元為目前時間的錯誤訊息。

.DESCRIPTION
撰寫前置字元為目前時間的錯誤訊息。將訊息寫入 Error 資料流之前，這個函式會先呼叫 Format-DevTestMessageWithTime 函式，以便在訊息前面加上時間。

.PARAMETER  Message
指定錯誤訊息呼叫中的訊息。您可以將訊息字串輸送到函式。

.INPUTS
System.String

.OUTPUTS
無。函式會寫入 Error 資料流。

.EXAMPLE
PS C:> Write-ErrorWithTime -Message "Failed. Cannot find the file."

Write-Error: 2/6/2014 8:37:29 AM - Failed. Cannot find the file.
 + CategoryInfo     : NotSpecified: (:) [Write-Error], WriteErrorException
 + FullyQualifiedErrorId : Microsoft.PowerShell.Commands.WriteErrorException

.LINK
Write-Error

#>
function Write-ErrorWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Error
}


<#
.SYNOPSIS
撰寫前置字元為目前時間的詳細資訊訊息。

.DESCRIPTION
撰寫前置字元為目前時間的詳細資訊訊息。由於它會呼叫 Write-Verbose，因為訊息只會在指令碼以 Verbose 參數執行或 VerbosePreference 偏好設定設為 Continue 時顯示。

.PARAMETER  Message
指定詳細資訊訊息呼叫中的訊息。您可以將訊息字串輸送到函式。

.INPUTS
System.String

.OUTPUTS
無。函式會寫入 Verbose 資料流。

.EXAMPLE
PS C:> Write-VerboseWithTime -Message "The operation succeeded."
PS C:>
PS C:\> Write-VerboseWithTime -Message "The operation succeeded." -Verbose
VERBOSE: 1/27/2014 11:02:37 AM - The operation succeeded.

.EXAMPLE
PS C:\ps-test> "The operation succeeded." | Write-VerboseWithTime -Verbose
VERBOSE: 1/27/2014 11:01:38 AM - The operation succeeded.

.LINK
Write-Verbose
#>
function Write-VerboseWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Verbose
}


<#
.SYNOPSIS
撰寫前置字元為目前時間的主機訊息。

.DESCRIPTION
這個函式會將訊息寫入主機程式 (Write-Host) 且前置字元為目前時間。寫入主機程式的效用有所不同。大多數裝載 Windows PowerShell 的程式會將這些訊息寫入標準輸出。

.PARAMETER  Message
指定沒有日期的基底訊息。您可以將訊息字串輸送到函式。

.INPUTS
System.String

.OUTPUTS
無。函式會將訊息寫入主機程式。

.EXAMPLE
PS C:> Write-HostWithTime -Message "作業成功。"
1/27/2014 11:02:37 AM - 作業成功。

.LINK
Write-Host
#>
function Write-HostWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )
    
    if ((Get-Variable SendHostMessagesToOutput -Scope Global -ErrorAction SilentlyContinue) -and $Global:SendHostMessagesToOutput)
    {
        if (!(Get-Variable -Scope Global AzureWebAppPublishOutput -ErrorAction SilentlyContinue) -or !$Global:AzureWebAppPublishOutput)
        {
            New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
        }

        $Global:AzureWebAppPublishOutput += $Message | Format-DevTestMessageWithTime
    }
    else 
    {
        $Message | Format-DevTestMessageWithTime | Write-Host
    }
}


<#
.SYNOPSIS
如果屬性或方法是物件的成員，則傳回 $true。否則為 $false。

.DESCRIPTION
如果屬性或方法是物件的成員，則傳回 $true。這個函式會針對類別的靜態方法及檢視 (例如 PSBase 和 PSObject) 傳回 $false。

.PARAMETER  Object
指定測試中的物件。請輸入包含物件或會傳回物件之運算式的變數。您無法將 [DateTime] 或管道物件等類型指定到這個函式。

.PARAMETER  Member
指定測試中屬性或方法的名稱。指定方法時，請省略後面接著方法名稱的括號。

.INPUTS
無。這個函式不會接受來自管線的輸入。

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Test-Member -Object (Get-Date) -Member DayOfWeek
True

.EXAMPLE
PS C:\> $date = Get-Date
PS C:\> Test-Member -Object $date -Member AddDays
True

.EXAMPLE
PS C:\> [DateTime]::IsLeapYear((Get-Date).Year)
True
PS C:\> Test-Member -Object (Get-Date) -Member IsLeapYear
False

.LINK
Get-Member
#>
function Test-Member
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Object,

        [Parameter(Mandatory = $true)]
        [String]
        $Member
    )

    return $null -ne ($Object | Get-Member -Name $Member)
}


<#
.SYNOPSIS
如果 Azure 模組的版本為 0.7.4 (含) 以後版本，則傳回 $true。否則傳回 $false。

.DESCRIPTION
如果 Azure 模組的版本為 0.7.4 (含) 以後版本，則 Test-AzureModuleVersion 會傳回 $true。如果未安裝模組或模組是舊版，則會傳回 $false。這個函式沒有參數。

.INPUTS
無

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModuleVersion
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
0      7      4      -1

PS C:\> Test-AzureModuleVersion
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModuleVersion
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [System.Version]
        $Version
    )

    return ($Version.Major -gt 0) -or ($Version.Minor -gt 7) -or ($Version.Minor -eq 7 -and $Version.Build -ge 4)
}


<#
.SYNOPSIS
如果安裝的 Azure 模組版本為 0.7.4 (含) 以後版本，則傳回 $true。

.DESCRIPTION
如果安裝的 Azure 模組版本為 0.7.4 (含) 以後版本，則 Test-AzureModule 會傳回 $true。如果未安裝模組或模組是舊版，則會傳回 $false。這個函式沒有參數。

.INPUTS
無

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModule
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
    0      7      4      -1

PS C:\> Test-AzureModule
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModule
{
    [CmdletBinding()]

    $module = Get-Module -Name Azure

    if (!$module)
    {
        $module = Get-Module -Name Azure -ListAvailable

        if (!$module -or !(Test-AzureModuleVersion $module.Version))
        {
            return $false;
        }
        else
        {
            $ErrorActionPreference = 'Continue'
            Import-Module -Name Azure -Global -Verbose:$false
            $ErrorActionPreference = 'Stop'

            return $true
        }
    }
    else
    {
        return (Test-AzureModuleVersion $module.Version)
    }
}


<#
.SYNOPSIS
以指令碼範圍中的 $Script:originalSubscription 變數儲存目前的 Microsoft Azure 訂用帳戶。

.DESCRIPTION
Backup-Subscription 函式會將目前的 Microsoft Azure 訂用帳戶 (Get-AzureSubscription -Current) 及其儲存體帳戶，以及這個指令碼 ($UserSpecifiedSubscription) 所變更的訂用帳戶及其儲存體帳戶儲存在指令碼範圍中。透過儲存值，您就可以使用 Restore-Subscription 等函式，將原始的目前訂用帳戶和儲存體帳戶還原為目前狀態 (如果目前狀態已變更)。

.PARAMETER UserSpecifiedSubscription
指定新資源建立及發行所在之訂用帳戶的名稱。函式會將訂用帳戶及其儲存體帳戶的名稱儲存在指令碼範圍中。這個參數是必要項。

.INPUTS
無

.OUTPUTS
無

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso
PS C:\>

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso -Verbose
VERBOSE: Backup-Subscription: Start
VERBOSE: Backup-Subscription: Original subscription is Microsoft Azure MSDN - Visual Studio Ultimate
VERBOSE: Backup-Subscription: End
#>
function Backup-Subscription
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]
        $UserSpecifiedSubscription
    )

    Write-VerboseWithTime 'Backup-Subscription: 開始'

    $Script:originalCurrentSubscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue
    if ($Script:originalCurrentSubscription)
    {
        Write-VerboseWithTime ('Backup-Subscription: 原始訂用帳戶為 ' + $Script:originalCurrentSubscription.SubscriptionName)
        $Script:originalCurrentStorageAccount = $Script:originalCurrentSubscription.CurrentStorageAccountName
    }
    
    $Script:userSpecifiedSubscription = $UserSpecifiedSubscription
    if ($Script:userSpecifiedSubscription)
    {        
        $userSubscription = Get-AzureSubscription -SubscriptionName $Script:userSpecifiedSubscription -ErrorAction SilentlyContinue
        if ($userSubscription)
        {
            $Script:originalStorageAccountOfUserSpecifiedSubscription = $userSubscription.CurrentStorageAccountName
        }        
    }

    Write-VerboseWithTime 'Backup-Subscription: 結束'
}


<#
.SYNOPSIS
還原為 "current" 狀態的 Microsoft Azure 訂用帳戶，該帳戶是以指令碼範圍中的 $Script:originalSubscription 變數來儲存。

.DESCRIPTION
Restore-Subscription 函式會將以 $Script:originalSubscription 變數儲存的訂用帳戶設成目前的訂用帳戶 (再次)。如果原始訂用帳戶具有儲存體帳戶，則這個函式會針對目前訂用帳戶將該儲存體帳戶設成目前的帳戶。函式只會在環境中有非 null 的 $SubscriptionName 變數時才會還原訂用帳戶。否則，就會結束。如果填入 $SubscriptionName 但 $Script:originalSubscription 是 $null，則 Restore-Subscription 會使用 Select-AzureSubscription Cmdlet 來清除 Microsoft Azure PowerShell 中訂用帳戶的目前及預設設定。這個函式沒有參數，因此不會接受任何輸入，而且不會傳回任何項目 (void)。您可以使用 -Verbose 將訊息寫入 Verbose 資料流。

.INPUTS
無

.OUTPUTS
無

.EXAMPLE
PS C:\> Restore-Subscription
PS C:\>

.EXAMPLE
PS C:\> Restore-Subscription -Verbose
VERBOSE: Restore-Subscription: Start
VERBOSE: Restore-Subscription: End
#>
function Restore-Subscription
{
    [CmdletBinding()]
    param()

    Write-VerboseWithTime 'Restore-Subscription: 開始'

    if ($Script:originalCurrentSubscription)
    {
        if ($Script:originalCurrentStorageAccount)
        {
            Set-AzureSubscription `
                -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName `
                -CurrentStorageAccountName $Script:originalCurrentStorageAccount
        }

        Select-AzureSubscription -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName
    }
    else 
    {
        Select-AzureSubscription -NoCurrent
        Select-AzureSubscription -NoDefault
    }
    
    if ($Script:userSpecifiedSubscription -and $Script:originalStorageAccountOfUserSpecifiedSubscription)
    {
        Set-AzureSubscription `
            -SubscriptionName $Script:userSpecifiedSubscription `
            -CurrentStorageAccountName $Script:originalStorageAccountOfUserSpecifiedSubscription
    }

    Write-VerboseWithTime 'Restore-Subscription: 結束'
}


<#
.SYNOPSIS
驗證組態檔並傳回組態檔值的雜湊資料表。

.DESCRIPTION
Read-ConfigFile 函式會驗證 JSON 組態檔並傳回所選值的雜湊資料表。
-- 它一開始會將 JSON 檔案轉換成 PSCustomObject。網站雜湊表包含下列索引鍵:
-- Location: 網站位置
-- Databases: 網站 SQL 資料庫

.PARAMETER  ConfigurationFile
指定您 Web 專案之 JSON 組態檔的路徑和名稱。Visual Studio 會在您建立 Web 專案時自動產生 JSON 檔案，並將檔案儲存在您方案的 PublishScripts 資料夾中。

.PARAMETER HasWebDeployPackage
表示有 Web 應用程式的 Web Deploy 封裝 ZIP 檔案。若要指定 $true 值，請使用 -HasWebDeployPackage 或 HasWebDeployPackage:$true。若要指定 false 值，請使用 HasWebDeployPackage:$false。這是必要參數。

.INPUTS
無。您無法將輸入輸送到這個函式。

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> Read-ConfigFile -ConfigurationFile <path> -HasWebDeployPackage


Name                           Value                                                                                                                                                                     
----                           -----                                                                                                                                                                     
databases                      {@{connectionStringName=; databaseName=; serverName=; user=; password=}}                                                                                                  
website                        @{name="mysite"; location="West US";}                                                      
#>
function Read-ConfigFile
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $ConfigurationFile
    )

    Write-VerboseWithTime 'Read-ConfigFile: 開始'

    # 取得 JSON 檔案的內容 (-raw 會忽略分行符號) 並將它轉換為 PSCustomObject
    $config = Get-Content $ConfigurationFile -Raw | ConvertFrom-Json

    if (!$config)
    {
        throw ('Read-ConfigFile: ConvertFrom-Json 失敗: ' + $error[0])
    }

    # 判斷 environmentSettings 物件是否有 'webSite' 屬性 (不論屬性值為何)
    $hasWebsiteProperty =  Test-Member -Object $config.environmentSettings -Member 'webSite'

    if (!$hasWebsiteProperty)
    {
        throw 'Read-ConfigFile: 組態檔沒有 webSite 屬性。'
    }

    # 從 PSCustomObject 中的值建置雜湊資料表
    $returnObject = New-Object -TypeName Hashtable

    $returnObject.Add('name', $config.environmentSettings.webSite.name)
    $returnObject.Add('location', $config.environmentSettings.webSite.location)

    if (Test-Member -Object $config.environmentSettings -Member 'databases')
    {
        $returnObject.Add('databases', $config.environmentSettings.databases)
    }

    Write-VerboseWithTime 'Read-ConfigFile: 結束'

    return $returnObject
}


<#
.SYNOPSIS
建立 Microsoft Azure 網站。

.DESCRIPTION
使用特定名稱及位置建立 Microsoft Azure 網站。這個函式會呼叫 Azure 模組中的 New-AzureWebsite Cmdlet。若訂用帳戶還沒有使用指定名稱的網站，這個函式會建立網站，並傳回網站物件。否則將會傳回現有網站。

.PARAMETER  Name
指定新網站的名稱。此名稱在 Microsoft Azure 中不得重複。這個參數是必要項。

.PARAMETER  Location
指定網站的位置。有效值為 Microsoft Azure 位置，例如「美國西部」。這個參數是必要項。

.INPUTS
無。

.OUTPUTS
Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.Site

.EXAMPLE
Add-AzureWebsite -Name TestSite -Location "West US"

Name       : contoso
State      : Running
Host Names : contoso.azurewebsites.net

.LINK
New-AzureWebsite
#>
function Add-AzureWebsite
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $Name,

        [Parameter(Mandatory = $true)]
        [String]
        $Location
    )

    Write-VerboseWithTime 'Add-AzureWebsite: 開始'
    $website = Get-AzureWebsite -Name $Name -ErrorAction SilentlyContinue

    if ($website)
    {
        Write-HostWithTime ('Add-AzureWebsite: 現有網站 ' +
        $website.Name + ' 已找到')
    }
    else
    {
        if (Test-AzureName -Website -Name $Name)
        {
            Write-ErrorWithTime ('網站 {0} 已經存在' -f $Name)
        }
        else
        {
            $website = New-AzureWebsite -Name $Name -Location $Location
        }
    }

    $website | Out-String | Write-VerboseWithTime
    Write-VerboseWithTime 'Add-AzureWebsite: 結束'

    return $website
}

<#
.SYNOPSIS
當 URL 為絕對且其配置為 https 時，會傳回 $True。

.DESCRIPTION
Test-HttpsUrl 函式會將輸入 URL 轉換為 System.Uri 物件。當 URL 為絕對 (而非相對) 且其配置為 https 時，會傳回 $True。如果其中一項不符，或者輸入字串無法轉換為 URL，則函式會傳回 $false。

.PARAMETER Url
指定要測試的 URL。請輸入 URL 字串，

.INPUTS
無。

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\>$profile.publishUrl
waws-prod-bay-001.publish.azurewebsites.windows.net:443

PS C:\>Test-HttpsUrl -Url 'waws-prod-bay-001.publish.azurewebsites.windows.net:443'
False
#>
function Test-HttpsUrl
{

    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $Url
    )

    # 如果無法將 $uri 轉換為 System.Uri 物件，Test-HttpsUrl 會傳回 $false
    $uri = $Url -as [System.Uri]

    return $uri.IsAbsoluteUri -and $uri.Scheme -eq 'https'
}


<#
.SYNOPSIS
建立可讓您連接到 Microsoft Azure SQL 資料庫的字串。

.DESCRIPTION
Get-AzureSQLDatabaseConnectionString 函式會組譯連接字串，以連接到 Microsoft Azure SQL 資料庫。

.PARAMETER  DatabaseServerName
指定 Microsoft Azure 訂用帳戶中現有資料庫伺服器的名稱。所有 Microsoft Azure SQL 資料庫都必須與 SQL 資料庫伺服器產生關聯。若要取得伺服器名稱，請使用 Get-AzureSqlDatabaseServer Cmdlet (Azure 模組)。這個參數是必要項。

.PARAMETER  DatabaseName
指定 SQL 資料庫的名稱。這可以是現有 SQL 資料庫或新 SQL 資料庫所使用的名稱。這個參數是必要項。

.PARAMETER  Username
指定 SQL 資料庫系統管理員的名稱。使用者名稱會是 $Username@DatabaseServerName。這個參數是必要項。

.PARAMETER  Password
指定 SQL 資料庫系統管理員的密碼。請以純文字輸入密碼。不允許安全字串。這個參數是必要項。

.INPUTS
無。

.OUTPUTS
System.String

.EXAMPLE
PS C:\> $ServerName = (Get-AzureSqlDatabaseServer).ServerName[0]
PS C:\> Get-AzureSQLDatabaseConnectionString -DatabaseServerName $ServerName `
        -DatabaseName 'testdb' -UserName 'admin'  -Password 'password'

Server=tcp:testserver.database.windows.net,1433;Database=testdb;User ID=admin@testserver;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
#>
function Get-AzureSQLDatabaseConnectionString
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseServerName,

        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseName,

        [Parameter(Mandatory = $true)]
        [String]
        $UserName,

        [Parameter(Mandatory = $true)]
        [String]
        $Password
    )

    return ('Server=tcp:{0}.database.windows.net,1433;Database={1};' +
           'User ID={2}@{0};' +
           'Password={3};' +
           'Trusted_Connection=False;' +
           'Encrypt=True;' +
           'Connection Timeout=20;') `
           -f $DatabaseServerName, $DatabaseName, $UserName, $Password
}


<#
.SYNOPSIS
從 Visual Studio 所產生 JSON 組態檔中的值建立 Microsoft Azure SQL 資料庫。

.DESCRIPTION
Add-AzureSQLDatabases 函式會接受來自 JSON 檔案之資料庫區段的資訊。這個函式 Add-AzureSQLDatabases (複數) 會針對 JSON 檔案中的每個 SQL 資料庫呼叫 Add-AzureSQLDatabase (單數) 函式。Add-AzureSQLDatabase (單數) 會呼叫 New-AzureSqlDatabase Cmdlet (Azure 模組)，此 Cmdlet 會建立 SQL 資料庫。這個函式不會傳回資料庫物件。它會傳回以前用來建立資料庫之值的雜湊資料表。

.PARAMETER DatabaseConfig
 接受源自 Read-ConfigFile 函式在 JSON 檔案具有網站屬性時傳回之 JSON 檔案的 PSCustomObjects 物件的陣列。其中包括 environmentSettings.databases 屬性。您可以將清單輸送到這個函式。
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig
connectionStringName: Default Connection
databasename : TestDB1
edition   :
size     : 1
collation  : SQL_Latin1_General_CP1_CI_AS
servertype  : New SQL Database Server
servername  : r040tvt2gx
user     : dbuser
password   : Test.123
location   : West US

.PARAMETER  DatabaseServerPassword
指定 SQL Database 伺服器管理員的密碼。輸入內含 Name 和 Password 索引鍵的雜湊表。Name 的值是 SQL Database 伺服器的名稱。Password 的值是系統管理員的密碼。例如: @Name = "TestDB1"; Password = "password"。這個參數是選擇性的。如果省略，或是 SQL Database 伺服器名稱不符合 $DatabaseConfig 物件的 serverName 屬性值，則函式會使用連接字串中 SQL Database 的 $DatabaseConfig 物件的 Password 屬性。

.PARAMETER CreateDatabase
確認您要建立資料庫。這個參數是選擇性的。

.INPUTS
System.Collections.Hashtable[]

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig | Add-AzureSQLDatabases

Name                           Value
----                           -----
ConnectionString               Server=tcp:testdb1.database.windows.net,1433;Database=testdb;User ID=admin@testdb1;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
Name                           Default Connection
Type                           SQLAzure

.LINK
Get-AzureSQLDatabaseConnectionString

.LINK
Create-AzureSQLDatabase
#>
function Add-AzureSQLDatabases
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSCustomObject]
        $DatabaseConfig,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword,

        [Parameter(Mandatory = $false)]
        [Switch]
        $CreateDatabase = $false
    )

    begin
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases: 開始'
    }
    process
    {
        Write-VerboseWithTime ('Add-AzureSQLDatabases: 正在建立 ' + $DatabaseConfig.databaseName)

        if ($CreateDatabase)
        {
            # 建立具有 DatabaseConfig 值的新 SQL 資料庫 (除非已有資料庫存在)
            # 隱藏命令輸出。
            Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig | Out-Null
        }

        $serverPassword = $null
        if ($DatabaseServerPassword)
        {
            foreach ($credential in $DatabaseServerPassword)
            {
               if ($credential.Name -eq $DatabaseConfig.serverName)
               {
                   $serverPassword = $credential.password             
                   break
               }
            }               
        }

        if (!$serverPassword)
        {
            $serverPassword = $DatabaseConfig.password
        }

        return @{
            Name = $DatabaseConfig.connectionStringName;
            Type = 'SQLAzure';
            ConnectionString = Get-AzureSQLDatabaseConnectionString `
                -DatabaseServerName $DatabaseConfig.serverName `
                -DatabaseName $DatabaseConfig.databaseName `
                -UserName $DatabaseConfig.user `
                -Password $serverPassword }
    }
    end
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases: 結束'
    }
}


<#
.SYNOPSIS
建立新的 Microsoft Azure SQL 資料庫。

.DESCRIPTION
Add-AzureSQLDatabase 會從 Visual Studio 所產生 JSON 組態檔中的資料建立 Microsoft Azure SQL 資料庫，並且傳回新的資料庫。如果訂用帳戶在指定的 SQL 資料庫伺服器中已有具有指定之資料庫名稱的 SQL 資料庫，函式就會傳回現有資料庫。這個函式會呼叫 New-AzureSqlDatabase Cmdlet (Azure 模組)，實際上，此 Cmdlet 會建立 SQL 資料庫。

.PARAMETER DatabaseConfig
接受源自 Read-ConfigFile 函式在 JSON 組態檔具有網站屬性時傳回之 JSON 組態檔的 PSCustomObject。其中包括 environmentSettings.databases 屬性。您無法將物件輸送到這個函式。Visual Studio 會針對所有 Web 專案產生 JSON 組態檔，並將組態檔儲存在您方案的 PublishScripts 資料夾中。

.INPUTS
無。這個函式不會接受來自管線的輸入

.OUTPUTS
Microsoft.WindowsAzure.Commands.SqlDatabase.Services.Server.Database

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases | where connectionStringName
PS C:\> $DatabaseConfig

connectionStringName    : Default Connection
databasename : TestDB1
edition      :
size         : 1
collation    : SQL_Latin1_General_CP1_CI_AS
servertype   : New SQL Database Server
servername   : r040tvt2gx
user         : dbuser
password     : Test.123
location     : West US

PS C:\> Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig

.LINK
Add-AzureSQLDatabases

.LINK
New-AzureSQLDatabase
#>
function Add-AzureSQLDatabase
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [Object]
        $DatabaseConfig
    )

    Write-VerboseWithTime 'Add-AzureSQLDatabase: 開始'

    # 如果參數值沒有 serverName 屬性，則會失敗，或者不會填入 serverName 屬性值。
    if (-not (Test-Member $DatabaseConfig 'serverName') -or -not $DatabaseConfig.serverName)
    {
        throw 'Add-AzureSQLDatabase: DatabaseConfig 值中遺漏資料庫 serverName (必要項)。'
    }

    # 如果參數值沒有資料庫名稱屬性，則會失敗，或者不會填入資料庫名稱屬性值。
    if (-not (Test-Member $DatabaseConfig 'databaseName') -or -not $DatabaseConfig.databaseName)
    {
        throw 'Add-AzureSQLDatabase: DatabaseConfig 值中遺漏 databasename (必要項)。'
    }

    $DbServer = $null

    if (Test-HttpsUrl $DatabaseConfig.serverName)
    {
        $absoluteDbServer = $DatabaseConfig.serverName -as [System.Uri]
        $subscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue

        if ($subscription -and $subscription.ServiceEndpoint -and $subscription.SubscriptionId)
        {
            $absoluteDbServerRegex = 'https:\/\/{0}\/{1}\/services\/sqlservers\/servers\/(.+)\.database\.windows\.net\/databases' -f `
                                     $subscription.serviceEndpoint.Host, $subscription.SubscriptionId

            if ($absoluteDbServer -match $absoluteDbServerRegex -and $Matches.Count -eq 2)
            {
                 $DbServer = $Matches[1]
            }
        }
    }

    if (!$DbServer)
    {
        $DbServer = $DatabaseConfig.serverName
    }

    $db = Get-AzureSqlDatabase -ServerName $DbServer -DatabaseName $DatabaseConfig.databaseName -ErrorAction SilentlyContinue

    if ($db)
    {
        Write-HostWithTime ('Create-AzureSQLDatabase: 使用現有資料庫 ' + $db.Name)
        $db | Out-String | Write-VerboseWithTime
    }
    else
    {
        $param = New-Object -TypeName Hashtable
        $param.Add('serverName', $DbServer)
        $param.Add('databaseName', $DatabaseConfig.databaseName)

        if ((Test-Member $DatabaseConfig 'size') -and $DatabaseConfig.size)
        {
            $param.Add('MaxSizeGB', $DatabaseConfig.size)
        }
        else
        {
            $param.Add('MaxSizeGB', 1)
        }

        # 如果 $DatabaseConfig 物件具有定序屬性且不是 null 或空白
        if ((Test-Member $DatabaseConfig 'collation') -and $DatabaseConfig.collation)
        {
            $param.Add('Collation', $DatabaseConfig.collation)
        }

        # 如果 $DatabaseConfig 物件具有版本屬性且不是 null 或空白
        if ((Test-Member $DatabaseConfig 'edition') -and $DatabaseConfig.edition)
        {
            $param.Add('Edition', $DatabaseConfig.edition)
        }

        # 將雜湊資料表寫入詳細資訊資料流
        $param | Out-String | Write-VerboseWithTime
        # 呼叫已展開的 New-AzureSqlDatabase (隱藏輸出)
        $db = New-AzureSqlDatabase @param
    }

    Write-VerboseWithTime 'Add-AzureSQLDatabase: 結束'
    return $db
}

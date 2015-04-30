function Get-ScampAzureSubscription
{
[CmdletBinding()]
Param(
  )

$locs = Get-AzureLocation -ErrorAction SilentlyContinue
if (!$locs)
{
  Add-AzureAccount -ErrorAction Stop
}

$subs = Get-AzureSubscription -ErrorAction SilentlyContinue | Sort-Object DefaultAccount,SubscriptionName
if ($subs.Count -eq 1)
{
  return $subs[0]
}

Write-Host ("Found " + $subs.Count + " subscriptions, please select:")
for ($i = 0; $i -lt $subs.Count; $i++)
{
  Write-Host ([string]$i + ". " + $subs[$i].SubscriptionName + " (" + $subs[$i].DefaultAccount + ")" )
}

while ($true)
{
  $choice = [int]::Parse( (Read-Host -Prompt ("Choose(0-"+($subs.Count-1)+")")) )
  if ($choice -ge 0 -and $choice -lt $subs.Count)
  {
    Select-AzureSubscription -SubscriptionName $subs[$choice].SubscriptionName
    return $subs[$choice]
  }

  Write-Warning "Bad choice $choice, please try again"
}

}

$scampSub = Get-AzureSubscription -Current
if ($scampSub -eq $null) {
  Add-AzureAccount
  Get-ScampAzureSubscription | Select-AzureSubscription
  }

# 
# Microsoft Online Services Sign-in Asssistant
#     http://go.microsoft.com/fwlink/?LinkID=286152
# Microsoft Azure AD Module for PowerShell
#     http://go.microsoft.com/fwlink/p/?linkid=236297

$msolPath = $env:windir + '\system32\windowspowershell\v1.0\Modules\MSOnline'
if (Test-Path $msolPath) {
  $msolDll = get-item $msolPath\Microsoft.Online.Administration.Automation.PSModule.dll
  $msolVer = $msolDll.VersionInfo.FileVersion
  if ($msolVer -lt "1.0.8070.2") {
    Write-Warning "Please update your Azure AD PowerShell module"
    Start-Process 'http://go.microsoft.com/fwlink/?LinkID=286152'
    return
    }
  } else {
    Write-Warning "Please install the Sign-In Assistant and the Azure AD PowerShell module"
    Start-Process 'http://go.microsoft.com/fwlink/?LinkID=286152'
    Start-Process 'http://go.microsoft.com/fwlink/p/?linkid=236297'
    return
  }


if ($msolCred -eq $null) {
  $msolCred = Get-Credential -UserName (Get-AzureSubscription -Current).Account -Message "Login to your subscription account"
  }



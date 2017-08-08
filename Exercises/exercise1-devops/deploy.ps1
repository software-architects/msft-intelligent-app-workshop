# Check if user is already signed in
Try {
  Get-AzureRmContext | Out-Null
} Catch {
  if ($_ -like "*Login-AzureRmAccount to login*") {
    Login-AzureRmAccount
  }
}

# Select subscription where name contains `MSDN Subscription`
Get-AzureRmSubscription | where { $_.SubscriptionName -like "*MSDN Subscription*" } | Select-AzureRmSubscription

# Set some string constants
$rg = "intel-app-workshop2"
$location = "westeurope"
$dep = "Deployment-" + [guid]::NewGuid()
$path = "C:\Code\GitHub\msft-intelligent-app-workshop\Exercises\exercise1-devops\"
$vaultName = "wwikvdev01"
$dbAdminUser = "demo"

# Check if resource group already exists
$group = Get-AzureRmResourceGroup -Name $rg -ErrorAction SilentlyContinue
if (!$group) {
    New-AzureRmResourceGroup -Name $rg -Location $location

    # Ask for username and password and store it in KeyVault
    New-AzureRmKeyVault -VaultName $vaultName -ResourceGroupName $rg -Location "westeurope" | Out-Null
    Set-AzureKeyVaultSecret -VaultName $vaultName -Name "SqlAdminPassword" -SecretValue (Get-Credential).Password | Out-Null
}

# Get username and password from KeyVault
$password = Get-AzureKeyVaultSecret -VaultName $vaultName -Name "SqlAdminPassword"
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($dbAdminUser, $password.SecretValue)  

# Deploy ARM template
New-AzureRmResourceGroupDeployment -ResourceGroupName $rg -TemplateFile "$path\erp.json" `
    -Name $dep -dbAdminUser $dbAdminUser -dbAdminPassword $cred.Password

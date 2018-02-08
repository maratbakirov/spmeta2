﻿$ErrorActionPreference = "Stop"

$coreScript = "$($PSScriptRoot)/local.config.ps1"
if(!(Test-Path $coreScript)) { throw "Cannot find core script: $coreScript"}
. $coreScript

$api = "O365"
$configName = "Default.SharePointOnline"

Apply-M2RegressionSettings -config $spMeta2Config `
                -configName $configName `
                -api $api `
                -skipTestData $false `
                -skipSPWebApp $false `
                -skipSPPostCheck $false `
                -skipSPServicesFix $false `
                -skipEnvVariables $false `
                -deleteSPWebApp $false 
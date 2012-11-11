Param (
    [string]$filename = $(throw "filename is required")
)
exit;
$version_regex = "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+";

Write-Host "Parsing file"
$select_result = Select-String -Path $filename -Pattern $version_regex -AllMatches

Write-Host ("" + $select_result.Matches.Count + " matches");
Write-Host ("Parsed value: " + $select_result.matches[0].value);
$oldVersion = New-Object -TypeName System.Version -ArgumentList $select_result.matches[0].value;
write-host ("Old Version: " + $oldVersion.ToString())


$newVersion = New-Object -TypeName System.Version -ArgumentList ("" + $oldVersion.Major + "." + $oldVersion.Minor + "." + ($oldVersion.Build + 1) + "." + $oldVersion.Revision);
write-host "New Version: " $newVersion

Write-Host ("Updating file " + $filename)
(Get-Content $filename) -replace $version_regex, $newVersion.ToString() | Set-Content $filename
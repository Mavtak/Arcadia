Param (
    [string]$filename = $(throw "filename is required")
)
exit
[regex]$version_regex = "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+";

Write-Host ("Parsing " + $filename)
$matches = $version_regex.Matches((Get-Content $filename))

Write-Host ("" + $matches.Count + " matches");

if($matches.Count -eq 0)
{
    Write-Host "Error";
    Exit 1;
}

Write-Host ("Parsed value: " + $matches[0].value);
$oldVersion = New-Object -TypeName System.Version -ArgumentList $matches[0].value;
write-host ("Old Version: " + $oldVersion.ToString())

$now = [System.DateTime]::UtcNow

$new_major_number = 1
$new_minor_number = 3
$new_build_number = 0
$new_revision_number = 0

$new_version_string = "$new_major_number.$new_minor_number.$new_build_number.$new_revision_number"

Write-Host "Version: $new_version_string"
$newVersion = New-Object -TypeName System.Version -ArgumentList $new_version_string

if($newVersion -eq $null)
{
    Write-Host "Could not create new version"
    Exit 2
}
write-host "New Version: $newVersion"

Write-Host ("Updating file " + $filename)
(Get-Content $filename) -replace $version_regex, "$newVersion" | Set-Content $filename
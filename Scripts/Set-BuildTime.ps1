Param (
    [string]$filename = $(throw "filename is required")
)

[regex]$datetime_regex = 'var date = ".*";';

Write-Host ("Parsing " + $filename)
$matches = $datetime_regex.Matches((Get-Content $filename))

Write-Host ("" + $matches.Count + " matches");

if($matches.Count -eq 0)
{
    Write-Host "Error";
    Exit 1;
}

Write-Host ("Parsed value: " + $matches[0].value);

$new_value = ('var date = "' + [System.DateTime]::UtcNow.ToString("o") + '";')

Write-Host "New Value: $new_value"

Write-Host ("Updating file " + $filename)
(Get-Content $filename) -replace $datetime_regex, $new_value | Set-Content $filename
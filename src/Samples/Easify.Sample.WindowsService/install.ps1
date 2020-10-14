$executable = "<PathToExecutable>\Easify.Sample.WindowsService.Exe"
$user = "NT_AUTHORITY\LocalService"
$service = "EasifyWindowsService"
$serviceName = "Easify Windows Service"
$serviceDescription = "Easify Windows Service to showcase hosting as service"

$acl = Get-Acl $executable
$aclRuleArgs = $user, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl $executable


New-Service -Name $service -BinaryPathName $executable -Credential $user -Description $serviceDescription -DisplayName $serviceName -StartupType Automatic

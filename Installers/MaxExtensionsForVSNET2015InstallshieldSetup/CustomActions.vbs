''===================================================================================''
''  MAIN EVENT FUNCTIONS                                                             ''
''===================================================================================''

Function SetDatabaseDir()
	Session.Property("DATABASEDIR") = GetVSNetInstallationFolder("14.0") & "Extensions\"
	'Session.Property("VSNETEXTENSIONS") = GetVSNetInstallationFolder("14.0") & "Extensions\"
    'msgbox Session.Property("VSNETEXTENSIONS")
End Function

'Function PrepareInstallation()
'
'	Session.Property("VSNET2015INSTALLDIR") = GetVSNetInstallationFolder("14.0")
'	Session.Property("DATABASEDIR") = GetVSNetInstallationFolder("14.0") & "Extensions\Arebis2\MAX Mapping Test\1.0\"
'    WriteToRegistry "HKCU\Software\Arebis\MAX Extensions for VS.NET 2015\InstallDir", Session.Property("INSTALLDIR")
'
'	Session.Property("NEW_FOLDER_1") = GetVSNetInstallationFolder("14.0") & "Extensions"
'	
'	'MsgBox GetVSNetInstallationFolder("14.0"), VBOK, "PrepareInstallation/1"
'	'MsgBox Session.Property("VSNET2015INSTALLDIR"), VBOK, "PrepareInstallation/2"
'	'MsgBox Session.Property("INSTALLDIR"), VBOK, "PrepareInstallation/3"
'    'MsgBox ReadFromRegistry("HKCU\Software\Arebis\MAX Extensions for VS.NET 2015\InstallDir", ""), VBOK, "PrepareInstallation/4"
'	MsgBox Session.Property("NEW_FOLDER_1"), VBOK, "PrepareInstallation/5"
'	
'End Function
'
'Function CopyExtensionToVSNET()
'	
'    'If IsElevated() Then
'    '    MsgBox "Is Elevated"
'    'Else
'    '    MsgBox "Is NOT Elevated"
'    'End IF
'
'	CopyExtensionToVSNETVersion("14.0")
'	
'End Function
'
'Function RemoveExtensionFromVSNET()
'
'    RemoveExtensionFromVSNETVersion("14.0")
'
'End Function
'
'''===================================================================================''
'''  UTILITY FUNCTIONS                                                                ''
'''===================================================================================''
'
'Function CopyExtensionToVSNETVersion(version)
'	Dim fso, sourceFolder, targetFolder, changefile
'	
'	On Error Resume Next
'
'	MsgBox "Starting...", vbOK, "CopyExtensionToVSNETVersion"
'
'    Set fso = CreateObject("Scripting.FileSystemObject")
'	If Err.Number <> 0 Then MsgBox "Failed to create object: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'   	sourceFolder = ReadFromRegistry("HKCU\Software\Arebis\MAX Extensions for VS.NET 2015\InstallDir", "")
'	If Err.Number <> 0 Then MsgBox "Failed to read from registry: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'	targetFolder = GetVSNetInstallationFolder(version)
'	If Err.Number <> 0 Then MsgBox "Failed to get installation folder: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'
'	MsgBox "SourceFolder: " & sourceFolder, vbOK, "CopyExtensionToVSNETVersion"
'	MsgBox "TargetFolder: " & targetFolder, vbOK, "CopyExtensionToVSNETVersion"
'
'    MsgBox Session.Property("INSTALLDIR"), VBOK, "CopyExtensionToVSNETVersion(session.property.INSTALLDIR)"
'
'	
'	If sourceFolder <> "" And targetFolder <> "" Then
'        If fso.FolderExists(targetFolder & "Extensions") Then
'
''MsgBox "Extensions folder exists"
'            '' Create extension folder:
'            If Not fso.FolderExists(targetFolder & "Extensions\Arebis") Then
'MsgBox "Creating folder: " & targetFolder & "Extensions\Arebis"
'                fso.CreateFolder targetFolder & "Extensions\Arebis"
'				If Err.Number <> 0 Then MsgBox "Failed to create folder """ & targetFolder & "Extensions\Arebis" & """: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'MsgBox "Created folder: " & targetFolder & "Extensions\Arebis"
'            End If
'            If Not fso.FolderExists(targetFolder & "Extensions\Arebis") Then
'MsgBox "Creating folder: " & targetFolder & "Extensions\Arebis"
'                fso.CreateFolder targetFolder & "Extensions\Arebis"
'				If Err.Number <> 0 Then MsgBox "Failed to create folder """ & targetFolder & "Extensions\Arebis" & """: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'MsgBox "Created folder: " & targetFolder & "Extensions\Arebis"
'            End If
'            If Not fso.FolderExists(targetFolder & "Extensions\Arebis") Then
'MsgBox "Creating folder: " & targetFolder & "Extensions\Arebis"
'                fso.CreateFolder targetFolder & "Extensions\Arebis"
'				If Err.Number <> 0 Then MsgBox "Failed to create folder """ & targetFolder & "Extensions\Arebis" & """: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'MsgBox "Created folder: " & targetFolder & "Extensions\Arebis"
'            End If
'            If Not fso.FolderExists(targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions") Then
'                fso.CreateFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions"
'				If Err.Number <> 0 Then MsgBox "Failed to create folder """ & targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions" & """: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'            End If
'            '' Copy files:
'		    fso.CopyFolder sourceFolder & "Extension\*.*", targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions\", True
'			If Err.Number <> 0 Then MsgBox "Failed to copy MAX extensions to VS.NET installation folder: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'            '' 'Touch' the "extensions.configurationchanged" file:
'            Set changefile = fso.OpenTextFile(targetFolder & "Extensions\extensions.configurationchanged", 8, True, 0)
'            changefile.Write Now
'            changefile.WriteLine " : Installed Arebis Max extensions"
'            changefile.Close
'			If Err.Number <> 0 Then MsgBox "Failed to touch the ""extensions.configurationchanged"" file: " & err.Message, VBOK, "CopyExtensionToVSNETVersion"
'        Else
'            MsgBox "No folder """ & targetFolder & "Extensions" & """ found."
'        End If
'	End If
'	
'	On Error Goto 0
'	
'End Function
'
'Function RemoveExtensionFromVSNETVersion(version)
'    Dim fso, targetFolder, changefile
'
'    On Error Resume Next
'    Set fso = CreateObject("Scripting.FileSystemObject")
'	targetFolder = GetVSNetInstallationFolder(version)
'
'    '' Delete extensions folder:
'    fso.DeleteFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions", true
'
'    '' 'Retouch' the "extensions.configurationchanged" file:
'    Set changefile = fso.OpenTextFile(targetFolder & "Extensions\extensions.configurationchanged", 8, True, 0)
'    changefile.Write Now
'    changefile.WriteLine " : Uninstalled Arebis Max extensions"
'    changefile.Close
'
'End Function

Function GetVSNetInstallationFolder(version)
    GetVSNetInstallationFolder = ReadFromRegistry("HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\" & version & "_Config\InstallDir", "")
End Function

Function ReadFromRegistry (strRegistryKey, strDefault)
    Dim WSHShell, value

    On Error Resume Next
    Set WSHShell = CreateObject("WScript.Shell")
    value = WSHShell.RegRead(strRegistryKey)

    If Err.Number <> 0 then
        ReadFromRegistry = strDefault
    Else
        ReadFromRegistry = value
    End If

    Set WSHShell = nothing

End Function
'
'Function WriteToRegistry(strRegistryKey, strValue)
'    Dim WSHShell, value
'
'    Set WSHShell = CreateObject("WScript.Shell")
'    WSHShell.RegWrite strRegistryKey, strValue, "REG_SZ"
'
'End Function
'
''Checks if the script is running elevated (UAC)
'Function IsElevated
'  Set shell = CreateObject("WScript.Shell")
'  Set whoami = shell.Exec("whoami /groups")
'  Set whoamiOutput = whoami.StdOut
'  strWhoamiOutput = whoamiOutput.ReadAll
' 
'  If InStr(1, strWhoamiOutput, "S-1-16-12288", vbTextCompare) Then 
'    IsElevated = True
'  Else
'      IsElevated = False
'  End If
'End Function
'

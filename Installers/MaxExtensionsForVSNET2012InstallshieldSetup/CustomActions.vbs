''===================================================================================''
''  MAIN EVENT FUNCTIONS                                                             ''
''===================================================================================''

Function PrepareInstallation()

	Session.Property("VSNET2012INSTALLDIR") = GetVSNetInstallationFolder("11.0")
    WriteToRegistry "HKCU\Software\Arebis\MAX Extensions for VS.NET\InstallDir", Session.Property("INSTALLDIR")
	
End Function

Function CopyExtensionToVSNET()
	
	CopyExtensionToVSNETVersion("11.0")
	
End Function

Function RemoveExtensionFromVSNET()

    RemoveExtensionFromVSNETVersion("11.0")

End Function

''===================================================================================''
''  UTILITY FUNCTIONS                                                                ''
''===================================================================================''

Function CopyExtensionToVSNETVersion(version)
	Dim fso, sourceFolder, targetFolder

    Set fso = CreateObject("Scripting.FileSystemObject")
   	sourceFolder = ReadFromRegistry("HKCU\Software\Arebis\MAX Extensions for VS.NET\InstallDir", "")
	targetFolder = GetVSNetInstallationFolder(version)

	If sourceFolder <> "" And targetFolder <> "" Then
        If fso.FolderExists(targetFolder & "Extensions") Then
            If Not fso.FolderExists(targetFolder & "Extensions\Arebis") Then
                fso.CreateFolder targetFolder & "Extensions\Arebis"
            End If
            If Not fso.FolderExists(targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions") Then
                fso.CreateFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions"
            End If
		    fso.CopyFolder sourceFolder & "Extension\*.*", targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions\", True
        Else
            MsgBox "No folder """ & targetFolder & "Extensions" & """ found."
        End If
	End If
	
End Function

Function RemoveExtensionFromVSNETVersion(version)
    Dim fso, targetFolder

    On Error Resume Next
    Set fso = CreateObject("Scripting.FileSystemObject")
	targetFolder = GetVSNetInstallationFolder(version)
    fso.DeleteFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions", true

End Function

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

Function WriteToRegistry(strRegistryKey, strValue)
    Dim WSHShell, value

    Set WSHShell = CreateObject("WScript.Shell")
    WSHShell.RegWrite strRegistryKey, strValue, "REG_SZ"

End Function

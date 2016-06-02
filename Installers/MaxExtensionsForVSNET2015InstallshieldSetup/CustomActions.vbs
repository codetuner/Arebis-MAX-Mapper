''===================================================================================''
''  MAIN EVENT FUNCTIONS                                                             ''
''===================================================================================''

Function PrepareInstallation()

	Session.Property("VSNET2013INSTALLDIR") = GetVSNetInstallationFolder("12.0")
    WriteToRegistry "HKCU\Software\Arebis\MAX Extensions for VS.NET 2013\InstallDir", Session.Property("INSTALLDIR")
	
End Function

Function CopyExtensionToVSNET()
	
	CopyExtensionToVSNETVersion("12.0")
	
End Function

Function RemoveExtensionFromVSNET()

    RemoveExtensionFromVSNETVersion("12.0")

End Function

''===================================================================================''
''  UTILITY FUNCTIONS                                                                ''
''===================================================================================''

Function CopyExtensionToVSNETVersion(version)
	Dim fso, sourceFolder, targetFolder, changefile

    Set fso = CreateObject("Scripting.FileSystemObject")
   	sourceFolder = ReadFromRegistry("HKCU\Software\Arebis\MAX Extensions for VS.NET 2013\InstallDir", "")
	targetFolder = GetVSNetInstallationFolder(version)

	If sourceFolder <> "" And targetFolder <> "" Then
        If fso.FolderExists(targetFolder & "Extensions") Then
            '' Create extension folder:
            If Not fso.FolderExists(targetFolder & "Extensions\Arebis") Then
                fso.CreateFolder targetFolder & "Extensions\Arebis"
            End If
            If Not fso.FolderExists(targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions") Then
                fso.CreateFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions"
            End If
            '' Copy files:
		    fso.CopyFolder sourceFolder & "Extension\*.*", targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions\", True
            '' 'Touch' the "extensions.configurationchanged" file:
            Set changefile = fso.OpenTextFile(targetFolder & "Extensions\extensions.configurationchanged", 8, True, 0)
            changefile.Write Now
            changefile.WriteLine " : Installed Arebis Max extensions"
            changefile.Close
        Else
            MsgBox "No folder """ & targetFolder & "Extensions" & """ found."
        End If
	End If
	
End Function

Function RemoveExtensionFromVSNETVersion(version)
    Dim fso, targetFolder, changefile

    On Error Resume Next
    Set fso = CreateObject("Scripting.FileSystemObject")
	targetFolder = GetVSNetInstallationFolder(version)

    '' Delete extensions folder:
    fso.DeleteFolder targetFolder & "Extensions\Arebis\Max Visual Studio.NET Extensions", true

    '' 'Retouch' the "extensions.configurationchanged" file:
    Set changefile = fso.OpenTextFile(targetFolder & "Extensions\extensions.configurationchanged", 8, True, 0)
    changefile.Write Now
    changefile.WriteLine " : Uninstalled Arebis Max extensions"
    changefile.Close

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

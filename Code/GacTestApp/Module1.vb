Imports System.Reflection

Module Module1

    Sub Main()

        Dim t As Type
        Dim tname As String

        If (Environment.GetCommandLineArgs().Length > 1) Then
            tname = Environment.GetCommandLineArgs()(1)
        Else
            tname = "Max.Tools.DomainGenerator.DomainGeneratorSession, Max.Tools.DomainGenerator"
        End If

        Console.WriteLine("Getting " & tname)

        t = Type.GetType(tname)

        If (t Is Nothing) Then
            Console.WriteLine("Failed to load type.")
        Else
            Console.WriteLine(t.FullName)
            Console.WriteLine(t.Assembly.CodeBase)
        End If

    End Sub

End Module

Imports System.Globalization
Imports System.IO
Imports System.Reflection

Class Application
    <STAThreadAttribute>
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf OnResolveAssembly
    End Sub

    Private Shared Function OnResolveAssembly(ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly
        Dim executingAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim assemblyName As AssemblyName = New AssemblyName(args.Name)
        Dim path = assemblyName.Name & ".dll"
        If assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) = False Then path = String.Format("{0}\{1}", assemblyName.CultureInfo, path)
        Using stream As Stream = executingAssembly.GetManifestResourceStream(path)
            If stream Is Nothing Then Return Nothing
            Dim assemblyRawBytes = New Byte(stream.Length - 1) {}
            stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length)
            Return Assembly.Load(assemblyRawBytes)
        End Using
    End Function

End Class

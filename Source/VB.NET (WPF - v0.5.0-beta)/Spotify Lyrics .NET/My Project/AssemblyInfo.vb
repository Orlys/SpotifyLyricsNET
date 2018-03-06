Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows

' Le informazioni generali relative a un assembly sono controllate dal seguente 
' set di attributi. Modificare i valori di questi attributi per modificare le informazioni
' associate a un assembly.

' Controllare i valori degli attributi degli assembly

<Assembly: AssemblyTitle("Spotify Lyrics .NET")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("Jakub Stęplowski")>
<Assembly: AssemblyProduct("Spotify Lyrics .NET")>
<Assembly: AssemblyCopyright("Copyright © 2018 Jakub Stęplowski")>
<Assembly: AssemblyTrademark("")>
<Assembly: ComVisible(false)>

'Per iniziare a creare applicazioni localizzabili, impostare
'<UICulture>CultureYouAreCodingWith</UICulture> nel file VBPROJ
'all'interno di un <PropertyGroup>.  Ad esempio, se si utilizza l'inglese (Stati Uniti)
'nei file di origine, impostare <UICulture> su "en-US".  Rimuovere quindi il commento
'dall'attributo NeutralResourceLanguage seguente.  Aggiornare "en-US" nella riga
'seguente in modo che corrisponda all'impostazione di UICulture nel file di progetto.

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)>


'L'attributo ThemeInfo indica la possibile posizione dei dizionari risorse generici e specifici del tema.
'Primo parametro: posizione dei dizionari risorse specifici del tema
'(da usare se nella pagina non viene trovata una risorsa,
' oppure nei dizionari delle risorse dell'applicazione)

'Parametro 2: posizione del dizionario risorse generico
'(da usare se nella pagina non viene trovata una risorsa,
'un'applicazione e alcun dizionario risorse specifico del tema)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'Se il progetto viene esposto a COM, il GUID seguente verrà utilizzato come ID della libreria dei tipi
<Assembly: Guid("aa6d4a2c-60b5-4171-9c48-74d4b857c317")>

' Le informazioni sulla versione di un assembly sono costituite dai seguenti quattro valori:
'
'      Versione principale
'      Versione secondaria
'      Numero di build
'      Revisione
'
' È possibile specificare tutti i valori oppure impostare valori predefiniti per i numeri relativi alla revisione e alla build
' usando l'asterisco '*' come illustrato di seguito:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("0.5.0.0")>
<Assembly: AssemblyFileVersion("0.5.0.0")>

![](/Screenshots/SpotifyLyricsNET-logo-v1.png)
# Spotify Lyrics .NET
![](https://img.shields.io/badge/build-passing-brightgreen.svg?style=flat) ![](https://img.shields.io/badge/VB.NET_source-v0.6.0-blue.svg?style=flat) ![](https://img.shields.io/badge/CSharp_source-v1.0.0--alpha-red.svg?style=flat)
> Get the lyrics of the song you're listening to on Spotify

![](/Screenshots/SpotifyLyricsNET-v1.0.0-alpha.png)

# Requirements
## To use the software:
- .NET Framework 4.6.1 or newer
- Spotify
- Internet Connection

> The software has been most recently tested with **Spotify 1.0.96.181.gf6bc1b6b**.<br>I can't guarantee that it works with other versions.

Download the latest version from the ![Releases](https://github.com/JakubSteplowski/SpotifyLyricsNET/releases) section.

## To use the source:
- Visual Studio 2017 (or newer)
- .NET Framework 4.6.1 or newer
- NuGet HtmlAgilityPack 1.8.11 package

# Changelog

>v0.6.0 (08/05/2018):
>- Improved UI
>- Improved search of lyrics
>- Fixed a few bugs and glitches
>- Added automerging of .dll with the WPF assembly<br>
>*(the source of this version is available only in VB.NET)*

>v0.3.0 (05/03/2018):
>- New improved UI
>- Project converted to WPF<br>
>*(the source of this version is available only in VB.NET)*

>v0.2.3 (02/03/2018):
>- Added support for themes: light, dark
>- Improved and fixed UI
>- The window position and size are now saved and restored at launch
>- The "Always on Top" status is now saved and restored at launch
>- Fixed lyrics formatting
>- Fixed song title parser

>v0.1.0 (01/03/2018):
>- First public version
>- Find the song title and author of the current playing song on Spotify
>- Search and get the lyrics from Musixmatch
>- Change lyrics if the found one are wrong (move through search results)
>- Source available in two languages: C# and VB.NET

## Planned features
- Support for other lyrics sources
- Improved UI and fixed loading glithces
- Improved performance
- Improved code (create separate classes)

# Used resources

- HtmlAgilityPack (![GitHub](https://github.com/zzzprojects/html-agility-pack), ![Website](http://html-agility-pack.net/), ![NuGet](https://www.nuget.org/packages/HtmlAgilityPack/))
- Costura (![GitHub](https://github.com/Fody/Costura))
- Icons made by ![Icons8](icons8.com)

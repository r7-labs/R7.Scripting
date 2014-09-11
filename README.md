# About R7.Scripting

R7.Scripting is the .NET / Mono library to use in C# scripts with Nautilus and Nemo filemanagers support. It provides simple command execution,
easy access to the environment variables, parsing of Nautilus / Nemo environment variables and some useful utilities.

Mono provides C# interactive console through '''csharp''' command, so you can create executable scripts on C# 
just by adding '''#!/usr/bin/csharp''' as first line of text file. Placing '''R7.Scripting.dll''' to the '''~/.config/csharp'''
and adding '''using R7.Scripting;''' directive to the script allows you to use R7.Scripting library features. 

# About R7.Scripting

*R7.Scripting* is the .NET / Mono library to use in C# scripts with *Nautilus*, *Nemo* and *Caja* filemanagers support. 
It provides simple command execution, easy access to the environment variables, error logging, environment variables 
parsing and some useful utilities.

Mono provides C# interactive console through `csharp` command, so you can create executable scripts on C# 
just by adding `#!/usr/bin/csharp` as first line of text file. Placing `R7.Scripting.dll` to the `~/.config/csharp`
and adding `using R7.Scripting;` directive to the script will allow you to use *R7.Scripting* library features. 

# Warning: unstable API

Library is WIP and on early stages of development, so don't expect any kind of stable API.

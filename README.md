# About R7.Scripting

*R7.Scripting* is .NET/Mono library designed to simplify the development of C# scripts.

## Features

- Support for *Nautilus*, *Nemo* and *Caja* filemanagers.
- Desktop notifications for scripts (via notify-sharp).
- Easy access to environment variables.
- Simple external commands execution.
- Error logging (to a file).

## License

[![GPLv3](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.html)

The *R7.Scripting* is free software: you can redistribute it and/or modify it under the terms of 
the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

## Options for C# script runners

Mono provides C# interactive console (REPL) through `csharp` command, so you can create executable scripts with C# 
just by adding `#!/usr/bin/csharp` as first line of source file. [More info...](http://www.mono-project.com/docs/tools+libraries/tools/repl/)

If you need more from your C# scripts (i.e. pass command-line arguments to them, know script source location, run script in a terminal window) - 
you could run then using *csexec*. Get it here: https://github.com/roman-yagodin/csexec, install, 
then add `#!/usr/bin/csexec -r:R7.Scripting.dll` as a first line. [More info...](https://github.com/roman-yagodin/csexec/blob/master/README.md)

## Install library

- Place `R7.Scripting.dll` and `notify-sharp.dll` at `~/.config/csharp`.

## Simple script

```C#
#!/usr/bin/csexec -r:R7.Scripting.dll

using System;
using System.IO;
using R7.Scripting;

public static class Program
{
    public static int Main (string [] args)
    {
        var script = new Script (args, () => { 
            // TODO: place your code here
        });

        return script.Run ();
    }
}
```

## Warning: in development!

The library's code is in process of constant change, so do not expect any kind of stable API.

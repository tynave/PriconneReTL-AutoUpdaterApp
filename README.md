# PriconneReTL-AutoUpdaterApp
A modified version of the [PriconneReTL-Installer](https://github.com/tynave/PriconneReTL-Installer) for use with the [PriconneReTL-AutoUpdater](https://github.com/tynave/PriconneReTL-AutoUpdater) patcher to perform auto-updating of the [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch.

## Installation
### Manual
Extract the files found in the release archive to the `priconner` folder (keep the folder structure in the archive!)

### Automated
Coming soon!

## Requirements / Dependencies
- [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch.  
(It can only update existing patch installations, it cannot do a fresh install automatically.)
- [PriconneReTLAutoUpdater](https://github.com/tynave/PriconneReTL-AutoUpdater)  
This application is called/run by the preload patcher plugin.

## Limitations / Caveats
As this updater is called/run by the preload patcher plugin which requires the BepInEx framework, the framework files cannot be accessed and modified by it.  
Thus the following files/folders are excluded from the auto-update process:
- .\dotnet
- .\BepInEx\core
- .\winhttp.dll

If any of these require updating, manual installation is required (either fully manually or through one of the available updater apps/scripts)

## Disclaimer
Use at your own risk.  
Although the application has been thoroughly tested, bugs, errors or undesired operation may happen.  
The author takes no responsibility for any eventual damage, data loss or any other negative effect cause by the above listed occurences or any misuse of the application.

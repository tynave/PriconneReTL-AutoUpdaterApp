# PriconneReTL-AutoUpdaterApp
A modified version of the [PriconneReTL-Installer](https://github.com/tynave/PriconneReTL-Installer) for use with the [PriconneReTL-AutoUpdater](https://github.com/tynave/PriconneReTL-AutoUpdater) patcher to perform auto-updating of the [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch.

## Installation
### Manual
Extract the files found in the release archive to the `priconner` folder (keep the folder structure in the archive!)  
(The goal is to have the files from this application, and the dll file of the PriconneReTLAutoUpdater together inside the BepInEx\patchers folder.
They can be directly in the root of the patchers folder, or in any subfolder, just have them "beside" each other. But if you just extract the release archive as-is into the priconner folder, you should be good.)

### Automated
Coming soon!

## Requirements / Dependencies
- [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch.  
The autoupdater can only update existing patch installations, it cannot do a fresh install automatically.
- [PriconneReTLAutoUpdater](https://github.com/tynave/PriconneReTL-AutoUpdater)  
This application is called/run by the preload patcher plugin.

## Limitations / Caveats / Exclusions
As this updater is called/run by the preload patcher plugin which requires the BepInEx framework, the framework files cannot be accessed and modified by it.  
Thus the following files/folders are excluded from the auto-update process:
- .\dotnet
- .\BepInEx\core
- .\winhttp.dll

If any of these require updating, manual installation is required (either fully manually or through one of the available updater apps/scripts)


Also, the same as the PriconneReTL-Installer, this autoupdater file does not touch the following user-important files:  
  - XUnityAutoTranslator specific files (from here on referred as "ignored files"):
      - _AutoGeneratedTranslations.txt
      - _Preprocessors.txt
      - _Postprocessors.txt
      - _Substitions.txt
  - Configuration files:
      - AutoTranslatorConfig.ini
      - BepInEx.cfg
  - User **created** files in the patch folders  
(Please note that due to the logic of the operations, any **user edited** files that originally are part of the translation patch **DO NOT** persist.
    So if you would like to make personal alterations of the patch files and want to keep those across the versions, it is advised to have those on seperate files.)

## Disclaimer
Use at your own risk.  
Although the application has been thoroughly tested, bugs, errors or undesired operation may happen.  
The author takes no responsibility for any eventual damage, data loss or any other negative effect cause by the above listed occurences or any misuse of the application.

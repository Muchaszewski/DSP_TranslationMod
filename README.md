[Support my work on Patreon](https://www.patreon.com/muchaszewski?fan_landing=true)
# Dyson sphere translation plugin!

[Licence: Project is under CC Attribution 4.0](https://raw.githubusercontent.com/Muchaszewski/DSP_TranslationMod/main/LICENSE)

# BREAKING CHANGE
New translation folder location for translation: `{Game Directory}\DSPGAME_Data\Translation\{LanguageName}\...`

# Features
 - Adds possibility to add custom languages
 - Adds (currently hidden) French language
 - Adds the possibility to change Font in game (WORK IN PROGRESS)
 - Use custom logo for custom translation
 
# Roadmap
 - Refactor code and add documentation
 - Add support for adjusting content size of in game UI to fit new text

## Installation via Mod manager 

[Download mod manager](https://dsp.thunderstore.io/package/ebkr/r2modman_dsp/)

1. Press Install with Mod Manager at https://dsp.thunderstore.io/package/Muchaszewski/DSPTranslationPlugin/
2. Add translations to
   `{Game Directory}\DSPGAME_Data\Translation\{LanguageName}\translation_DysonSphereProgram.json`.
   You can find [translations at Crowdin](https://crowdin.com/translate/dyson-sphere-program)
6. Select new translation in Menu of the Game
7. Enjoy
   (Note: Restart is not required for full effect ;) )
   
## Installation Manual
1. Download and unpack [BepInEx](https://github.com/BepInEx/BepInEx/releases) into game root directory
2. Download [mod files](https://github.com/Muchaszewski/DSP_TranslationMod/releases)
3. Extract zip file
4. Paste DLLs under `Dyson Sphere Program\BepInEx\plugins\DSPTranslationPlugin`
5. Add translations to
`{Game Directory}\DSPGAME_Data\Translation\{LanguageName}\translation_DysonSphereProgram.json`.
You can find [translations at Crowdin](https://crowdin.com/translate/dyson-sphere-program)

6. Select new translation in Menu of the Game
7. Enjoy
(Note: Restart is not required for full effect ;) )

![InGameTranslation](https://raw.githubusercontent.com/Muchaszewski/DSP_TranslationMod/main/.readme/InGameTranslation.png)

## How to add new translations
1. Create folder under `Dyson Sphere Program\BepInEx\plugins\DSPTranslationPlugin\Translation` with the name of your translation eg: `Polish`

![TranslationFolder](https://raw.githubusercontent.com/Muchaszewski/DSP_TranslationMod/main/.readme/TranslationFolder.png)

2. Run game once - New file settings and translations files will be created.
3. Translate

### Translation file structure:
```
{ #CROWDIN
  "点击鼠标建造": "Click to build",
  "无法在此建造": "Cannot build here",
  "{NAME}: "{TRANSLATION}",
  (...)
}
```


```
{ #LEGACY
  "TranslationTable": [
    {
      "IsValid": true,                          # Does translation exists in the game
      "Name": "点击鼠标建造",                    # Name used by the game for translation (READ ONLY)
      "Original": "Click to build",             # Translation in English
      "Translation": "Kliknij, aby zbudować"    # Your translation here
    },
    (...)
    ]
}
```

### Settings file structure:
```
{
  "Version": "0.1.0.0",                             # Plugin version
  "GameVersion": "0.6.15.5678",                     # Game version
  "OriginalLanguage": "ENUS",                       # Language in which empty new translation files will be generated, possible values: "ENUS", "FRFR", "ZHCN"
  "LanguageDisplayName": "Polish",                  # Language display name in the game
  "ImportFromLegacy": false,                        # Generate Legacy json format (defualt is Crowdin format)
  "CreateAndUpdateFromPlainTextDumpUnsafe": true    # Should create and import dump file (more below)
}
```

### Custom images:
Currently in the game there are 2 images that can be changed, they look like a path - `UI/Textures/dsp-logo-en`. You can use a valid path from resources or valid URL.
URL needs to be a direct png file. Eg:
```
   Internet URI -- "ImageLogo0": "https://wiki.factorio.com/images/thumb/Factorio-logo.png/461px-Factorio-logo.png",
   Local file URI -- "ImageLogo0": "file://C:/Users/Muchaszewski/Documents/Icon.png"
```

#### Specific images description:

 - `ImageLogo0` and `ImageLogo1` needs to have aspect ratio that corresponds to 800x300 pixels, otherwise they will be stretched

# Special Thanks to
[BepInEx](https://github.com/BepInEx/BepInEx/releases) - for mod support

Modified hard fork of [SimpleJSON](https://github.com/Bunny83/SimpleJSON) - for simple json parser

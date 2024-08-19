# CRI ADX LE for C#
ADX LE is the lite edition of “CRI ADX”, a widely used audio middleware in game development studios worldwide that can be used for free*.
- https://game.criware.jp/products/adx-le/

Please read and agree to [“CRI ADX LE User License Agreement”](CRI_ADX_LE_SDK_License_Agreement_en.txt) before downloading and using the SDK.

*Conditions applies.

## Conditions for the distribution of contents using “ADX LE” (including free-to-play and commercial games)
Contents using ADX LE can only be distributed without licensing fees if ALL of the following conditions are met:
- As a company, organization, or individual, you had less then 10 million yen in revenue during the previous fiscal year.
- You own the distribution rights of the game.
- The revenue of the game is less than 10 million yen. * When exceeding 10 million yen, you need to switch to the licensing with “ADX”.

When distributing games with ADX LE, the copyright notice must be included.
The copyright notice must not be modified under any circumstances. Please follow these instructions: 
- https://game.criware.jp/products/adx-le/ (under "Copyright Notice" section)

## How to use

This section will walkthrough how to import CRI ADX LE. 
For further instructions and informations, please refer to ["Manual/API Reference for CRI ADX for C#"](https://game.criware.jp/manual/csharp_plugin/latest/index.html).

### Prepare resource files for ADX

To play sounds with ADX, data built in a proprietary format for ADX via CRI Atom Craft is needed.
Our tool package including CRI Atom Craft can be obtained via the download page of ADX LE:
- https://game.criware.jp/products/adx-le/

For how to create and build data for ADX, please refer to our Native SDK Manual:
- https://game.criware.jp/manual/native/adx2_en/latest/criatom_qstart_createdata.html

### Project Settings

Application projects needed to be set up in order for the SDK to function.

#### dotnet

Add `CriWare.CriAtomLE` to the project's package references.
```
dotnet add package CriWare.CriAtomLE
```

#### Unity

Add the following Git URL in Unity Package Manager:
```
https://github.com/cri-middleware/cri_adx_le_for_csharp.git?path=CriWare.CriAtomLE
```

Further instructions about Unity Package Manager can be found here:
- https://docs.unity3d.com/Manual/upm-ui-giturl.html

## Supports/FAQ

We do not provide individual support for ADX LE users. For bug reports or technical support, please refer to this link instead.

- [Frequent asked questions concerning ADX LE](https://game.criware.jp/products/adx2-le/le-faq/)
- [ADX User Help Center (Facebook)](https://www.facebook.com/groups/adx2userj/): A forum where ADX users can engage within the CRIWARE community.
- [Game Sound Production Community (Discord)](https://discordapp.com/invite/hJn9Cyc): A forum where users can engage in a wide range of discussions about game sound production. Users can ask questions about ADX LE to the Technical Support team of CRIWARE.
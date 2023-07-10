🌏[한국어](README.kr.md) | [English](README.en.md)


[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fkillkimno%2FMORT&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
[![GitHub downloads](https://img.shields.io/github/downloads/killkimno/MORT/total.svg?logo=github)](https://github.com/killkimno/MORT/releases)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y6DIUR2)

<img src="https://github.com/killkimno/MORT/blob/main/MORT_LOGO.png" width="90%"></img>


[![Video Label](https://img.youtube.com/vi/LHTErVnsaws/0.jpg)](https://youtu.be/LHTErVnsaws)

https://youtu.be/LHTErVnsaws

Sample video

# MORT #

MORT is a program that extracts dialogs from the screen in real time using OCR and outputs a translations using DB or machine translation.

Currently, English and Japanese translation/extraction can be extracted by default, and it can also be translated by linking with a hooking program using the save to clipboard function.

[Latest version download and release notes - https://blog.naver.com/killkimno/70179867557]

## Features ##
* Realtime translate
* OCR - TesseractOCR , Windows OCR, NHOcr
* Machine translation - Naver Papago, Google Web, Google Sheet, ezTrans, DeepL
* Language Patch with using DB
* Multiple OCR areas
* Img adjust

## System Requirement ##
* Windows 10 or higer
* 64bit os
* .NET 7 or higer
  - https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/runtime-desktop-7.0.8-windows-x64-installer
* Visual Studio 2022 Visual C++ (x64) - vcredist_x64.exe
  - https://aka.ms/vs/17/release/vc_redist.x64.exe


## How to use ##
#### Basic usage ####

1. After setting in the quick settings, press Trnaselate on the remote control to start translation
2. Or Preferences tab -> Set the OCR language according to the language of the game to be translated
3. Remote control -> Click Search and select the area where the lines appear
4. Click Apply on MORT main form
5. Remote control -> Real-time translation by pressing Translate

#### User Manual ####
* https://blog.naver.com/killkimno/221904784013

#### FAQ ####
<b>Can i use FullScreen Mode?</b>
- No you can't use it in fullscreen games, please use windowed mode, borderless windowed mode instead

<b>I'm using 32-bit Windows. Can I use MORT?</b>
- Use 32bit version MORT
- https://blog.naver.com/killkimno/222936631523

<b>I'm using 64-bit Windows. But Can't run with this error 0x8007045A.</b>
- CPU must Support AVX2. If Your CPU not support AVX2, Use 32bit version MORT instead
- https://blog.naver.com/killkimno/222936631523


## Develop the project
### Development environment ###

* Visaul Studio 2019 or higer
* Tesseract OCR 5.2.0 
* NHocr 0.21

### Create a build and run environment ###
1. Set the project to Release mode. Set the target CPU to X64.
2. Build the project first
3. Can't run after build. The reason is that there are no essential files required for run
4. Unzip the latest build files into the release folder to get the required files 
- https://drive.google.com/drive/folders/0BxO-Nrmd-kR7dVp5TWpMQ09jMFU?resourcekey=0-bx6_8OEv3WAGzz9Au9fxNg
5. MORT_CORE.DLL , nhocr.DLL To modify This dll, please refer to the related projects below

### Related Project ###

* MORT Core - MORT_CORE_DLL
  - https://github.com/killkimno/MORT_CORE
  
* MORT NHocr - nhocr.DLL
  - https://github.com/killkimno/MORT_NHOCR

## ETC ##
### Development Plans Trello ###

- https://trello.com/b/gPa1EL5x/mort

### Discord ###
- [Discord](https://discord.com/invite/ha5yNy9) ![Discord Badge](https://discord.com/api/guilds/742743719958151298/widget.png?style=shield)

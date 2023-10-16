🌏[한국어](README.kr.md) | [English](README.en.md)


[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fkillkimno%2FMORT&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
[![GitHub downloads](https://img.shields.io/github/downloads/killkimno/MORT/total.svg?logo=github)](https://github.com/killkimno/MORT/releases)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y6DIUR2)

<img src="https://github.com/killkimno/MORT/blob/main/MORT_LOGO.png" width="90%"></img>


[![Video Label](https://img.youtube.com/vi/LHTErVnsaws/0.jpg)](https://youtu.be/LHTErVnsaws)

https://youtu.be/LHTErVnsaws

Sample video

# MORT #

MORT는 OCR을 이용해 화면상에 나온 대사를 추출 , DB나 기계번역을 이용해 번역본을 출력해주는 프로그램입니다.

현재 영어와 일본어 번역/추출을 기본적으로 추출 가능하며, 클립보드에 저장 기능을 이용해 후킹 프로그램과 연동해 번역할 수도 있습니다.

[최신 버전 다운로드 및 릴리즈 노트 - https://blog.naver.com/killkimno/70179867557]

## 지원 기능 ##
* 실시간 번역
* OCR - TesseractOCR , Windows OCR, NHOcr
* 번역기 - 네이버 파파고, 구글 웹, 구글 시트, 이지트랜스, 딥플
* DB를 이용한 준한글화
* 다중 OCR 영역
* 이미지 보정
* 실시간 번역

## 요구 사항 ##
* 윈도우 10 이상
* 64비트 OS
* .NET 7 이상
  - https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/runtime-desktop-7.0.8-windows-x64-installer
* 필요 재배포 패키지 Visual Studio 2022 Visual C++ (x64) - vcredist_x64.exe
  - https://aka.ms/vs/17/release/vc_redist.x64.exe
## 사용법 ##
#### 기본 사용법 ####
1. 빠른설정에서 설정을 한 후 리모컨에서 Trnaslate를 눌러 번역 시작
2. 또는 기본설정 탭 -> OCR 언어를 번역할 게임의 언어에 맞춰 설정
3. 리모컨 -> Search 클릭 후 대사가 나오는 영역 선택
4. MORT 메인 폼에서 적용 클릭
5. 리모컨 -> Translate를 눌러 실시간 번역

#### 사용자 메뉴얼 링크 ####
* https://blog.naver.com/killkimno/221904784013

## 커스텀 사용법 ##
#### 번역 결과 언어 코드 추가 ####
UserData/UserTransCode.txt 파일에서 구글 번역기 언어 코드를 추가할 수 있습니다​

```
code, 이름 
(예 : it, 이탈리아어)
```

​언어 코드 리스트는 https://cloud.google.com/translate/docs/languages?hl=en 여기서 확인할 수 있습니다
#### 커스텀 API 사용법 ####
1. HTTP 기반으로 만들어진 커스텀 API를 사용할 수 있습니다
2. 번역 방법 -> 커스텀 API
3. 고급 설정 -> 커스텀 API URL 설정
```
POST 규칙
name - string
text - string - 번역할 문장
target - string - 결과 언어 코드
source - string - 소스 언어 코드

ex
{
"name" : "test",
"text" : "tank divsion" ,
"target" : "ko",
"source" : "en"
...
}


응답 규칙
result - string - 번역 결과
errorCode - string - 에러 코드
errorMessage - string - 에러 메세지

ex
{
"result" : "탱크 사단",
"errorMessage" : "",
"errorCode" : "0"
}
```
#### FAQ ####
<b>전체화면 게임에서 사용할 수 있나요?</b>
- 아뇨 전체화면 게임에서 사용할 수 없습니다. 대신 윈도우 모드, 보더리스 윈도우 모드로 해주세요

<b>32bit 윈도우를 사용하고 있습니다. MORT를 사용할 수 없을까요?</b>
- 32비트 버전 MORT를 이용해 주세요
- https://blog.naver.com/killkimno/222936631523

<b>64bit 윈도우를 사용하는데 0x8007045A 가 발생했습니다. </b>
- CPU가 AVX2를 지원하지 않아 생기는 문제입니다. 32비트 MORT를 이용해 주세요
- https://blog.naver.com/killkimno/222936631523

## 프로젝트 개발하기 ##
### 개발 환경 ###
* Visaul Studio 2019 or higer
* Tesseract OCR 5.2.0 
* NHocr 0.21

### 빌드 및 실행 환경 만들기 ###
1. 프로젝트를 Release 모드로 설정합니다. 타겟 CPU는 X64로 합니다
2. 빌드합니다
3. 실행하면 에러가 뜹니다. 이유는 실행에 필요한 필수 파일이 없기 때문입니다
4. 필수 파일을 받기 위해 최신 빌드 파일을 릴리즈 폴더에 압축을 풉니다 
- https://drive.google.com/drive/folders/0BxO-Nrmd-kR7dVp5TWpMQ09jMFU?resourcekey=0-bx6_8OEv3WAGzz9Au9fxNg
5. DLL 폴더의 MORT_CORE.DLL , nhocr.DLL 을 수정할려면 아래 관련된 프로젝트를 참고하시기 바랍니다

### 관련 프로젝트 ###

* MORT Core - MORT_CORE_DLL
  - https://github.com/killkimno/MORT_CORE
  
* MORT NHocr - nhocr.DLL
  - https://github.com/killkimno/MORT_NHOCR

## 그 외 ##
### 개발 상황 트렐로 ###

- https://trello.com/b/gPa1EL5x/mort

### 문의 디스코드 ###
- [Discord](https://discord.com/invite/ha5yNy9) ![Discord Badge](https://discord.com/api/guilds/742743719958151298/widget.png?style=shield)

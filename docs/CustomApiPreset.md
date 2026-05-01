================================================================
[MORT 1.310] 커스텀 API 프리셋 - API 문서로부터 프리셋 만드는 법
================================================================

이 문서는 외부 번역 API의 공식 문서를 읽고 MORT의 커스텀 API 프리셋으로
옮길 때, Headers / Request / Response를 어떻게 만드는지 설명합니다.


■ 핵심 아이디어

  외부 API 문서는 보통 아래 4가지 정보를 알려줍니다.
    (1) 엔드포인트 URL
    (2) 필요한 헤더 (인증, Content-Type 등)
    (3) 요청 본문 예시 (보통 JSON)
    (4) 응답 본문 예시 (보통 JSON)

  MORT 프리셋의 4칸은 이 4가지를 그대로 옮긴 것입니다.

    API 문서                   →  MORT 프리셋
    ─────────────────────────     ─────────────────────────
    (1) Endpoint URL           →  Url
    (2) Required Headers       →  Headers
    (3) Request body sample    →  Request  + 토큰 치환
    (4) Response body sample   →  Response + {RESULT_TEXT} 표시


■ 패치노트 (1.310)

  - 커스텀 API 프리셋 기능 추가
  - Request 템플릿에서 헤더 형식 추가, 배열 형식 지원
  - Gemini 모델 'gemini-3.1-flash-lite-preview' 추가
  - 윈도우 검색에서 앱 실행 시 파일 경로를 못 찾는 문제 수정


================================================================
1. URL 정하기
================================================================

API 문서에 있는 엔드포인트 주소를 그대로 Url 칸에 입력합니다.

  예) "POST https://api.example.com/v1/translate"
       → Url : https://api.example.com/v1/translate

같은 API라도 Free / Pro 등으로 도메인이 갈리는 경우가 있으니
어떤 플랜의 키를 쓰는지 확인해야 합니다.

  예) DeepL Free : https://api-free.deepl.com/v2/translate
      DeepL Pro  : https://api.deepl.com/v2/translate


================================================================
2. Headers 만들기
================================================================

API 문서의 "Required Headers" 또는 "Authentication" 섹션을 보고
그대로 옮깁니다. 한 줄에 하나씩, "Key: Value" 형식입니다.

  ┌─────────────────────────────────────────────────────────┐
  │ MORT가 자동으로 넣는 헤더                              │
  │   · Content-Type: application/json                     │
  │   · cache-control: no-cache  (구 방식에서)             │
  │ → 위 두 개는 직접 적지 않아도 됩니다.                  │
  └─────────────────────────────────────────────────────────┘

대표 패턴별 변환 예시 :

  [Bearer 토큰]
    API 문서 :  Authorization: Bearer <YOUR_TOKEN>
    MORT     :  Authorization: Bearer YOUR_TOKEN

  [API Key 방식 - DeepL]
    API 문서 :  Authorization: DeepL-Auth-Key <KEY>
    MORT     :  Authorization: DeepL-Auth-Key YOUR_API_KEY:fx

  [API Key 방식 - Yandex 등]
    API 문서 :  Authorization: Api-Key <KEY>
    MORT     :  Authorization: Api-Key YOUR_API_KEY

  [커스텀 헤더]
    API 문서 :  X-Custom-Tenant: my-tenant
    MORT     :  X-Custom-Tenant: my-tenant

주의사항 :
  · 콜론(:) 누락 시 헤더가 무시되고 로그에 경고가 남습니다.
  · 따옴표는 붙이지 마세요 (Authorization: "Bearer ..."  ← X)


================================================================
3. Request 템플릿 만들기
================================================================

API 문서의 요청 본문 예시(보통 JSON)를 그대로 가져와서,
"고정값 자리"를 MORT 토큰으로 바꾸면 됩니다.

▶ 사용 가능한 토큰

  {OCR_TEXT}     : OCR로 추출된 원문 (자동 JSON 이스케이프됨)
  {SOURCE_CODE}  : 소스 언어 코드 (예: en, ja)
  {RESULT_CODE}  : 결과 언어 코드 (예: ko, en)


▶ 변환 절차

  Step 1. API 문서의 요청 예시를 그대로 복사
  Step 2. 번역할 텍스트 자리를 {OCR_TEXT} 로 교체
  Step 3. 소스 언어 자리를 {SOURCE_CODE} 로 교체
  Step 4. 결과 언어 자리를 {RESULT_CODE} 로 교체
  Step 5. 그 외 고정값(모델명, 옵션 등)은 그대로 둠
  Step 6. 가장 바깥의 { } 는 떼고 안쪽 키-값 줄만 입력칸에 붙여넣음
          (MORT가 자동으로 { } 로 감싸줍니다)


▶ 입력칸에는 "안쪽 줄"만 적는다 — 매우 중요

  MORT 의 Request 입력칸에는 가장 바깥의 { } 를 적지 않아도 됩니다.
  안쪽의 키-값 줄만 적으면 MORT가 자동으로 { } 로 감싸줍니다.

  예) API 문서가 아래와 같다면

      {
        "text": ["Hello"],
        "target_lang": "KO"
      }

    Request 입력칸에는 이렇게만 적으면 됩니다 ↓

      "text": ["{OCR_TEXT}"],
      "target_lang": "{RESULT_CODE}"

    물론 { } 를 같이 적어도 동작은 같습니다. 어느 쪽이든 OK.


▶ 예시 - 일반 번역 API

  [API 문서 예시]
    {
      "text": "Hello world",
      "source": "en",
      "target": "ko"
    }

  [MORT Request 입력칸에 적는 값]
    "text": "{OCR_TEXT}",
    "source": "{SOURCE_CODE}",
    "target": "{RESULT_CODE}"


▶ 예시 - 배열로 받는 API (DeepL 형태)

  [API 문서 예시]
    {
      "text": ["Hello world"],
      "target_lang": "KO"
    }

  [MORT Request 입력칸에 적는 값]
    "text": ["{OCR_TEXT}"],
    "target_lang": "{RESULT_CODE}"


▶ 예시 - LLM 프롬프트형 API (Ollama 형태)

  [API 문서 예시]
    {
      "model": "translategemma",
      "prompt": "Translate to Korean: Hello world",
      "stream": false
    }

  [MORT Request 입력칸에 적는 값]
    "model": "translategemma",
    "prompt": "Translate to {RESULT_CODE}: {OCR_TEXT}",
    "stream": false


▶ 문법 규칙

  · JSON 표준 문법 ( "key": "value" ) 과
    C# 객체 초기화 문법 ( key = "value" ) 둘 다 지원합니다.
    내부 파서가 C# 스타일을 자동으로 JSON으로 변환합니다.
  · 가장 바깥의 { } 는 자동으로 감싸주므로 생략 가능합니다.
  · 값 자동 인식 :
      문자열       → 자동으로 큰따옴표
      숫자         → 그대로 숫자
      true / false → 그대로 불리언
      null         → JSON null
      [ ... ]      → 배열로 처리, 내부 요소도 같은 규칙


▶ 자주 막히는 부분

  (1) 언어 코드 대소문자
      어떤 API는 소문자(ko), 어떤 API는 대문자(KO)를 요구합니다.
      MORT가 넘기는 {RESULT_CODE} 는 보통 소문자입니다.
      대문자만 받는 API라면 토큰 대신 직접 적거나, 사전에 변환된
      코드를 쓰도록 하세요.
        예) DeepL : "target_lang": "KO"  ← 직접 대문자

  (2) source 언어 자동 감지
      많은 API가 source 를 생략하면 자동 감지합니다.
      필요 없으면 {SOURCE_CODE} 부분을 통째로 빼도 됩니다.

  (3) 따옴표 누락
      문자열인데 따옴표가 빠지면 JSON 변환에 실패합니다.
      ("text": {OCR_TEXT}  ← X,  "text": "{OCR_TEXT}"  ← O)
      단, 이미 JSON 이스케이프된 토큰을 사용하므로 토큰 자체에
      따옴표를 넣지 마세요.


================================================================
4. Response 템플릿 만들기
================================================================

API 문서의 응답 예시를 가져와서, "번역 결과"가 들어있는 자리에
{RESULT_TEXT} 라고 적어주면 됩니다.


▶ 변환 절차

  Step 1. API 문서의 응답 예시를 그대로 복사
  Step 2. 실제 번역 결과 문자열이 들어있는 위치를 {RESULT_TEXT} 로 교체
  Step 3. 그 외 필드는 그대로 둬도 되고 지워도 됨
          (MORT는 키 이름만 보고 재귀 탐색합니다)
  Step 4. 가장 바깥의 { } 는 떼고 안쪽만 입력칸에 붙여넣음
          (MORT가 자동으로 { } 로 감싸줍니다. 같이 적어도 OK)


▶ 예시 1 - 단순 키 응답 (Ollama)

  [API 응답 예시]
    {
      "response": "안녕하세요",
      "done": true,
      "model": "translategemma"
    }

  [MORT Response 입력칸에 적는 값]
    "response": {RESULT_TEXT}

  → MORT가 "response" 키 아래의 값을 결과로 사용합니다.


▶ 예시 2 - 배열 + 중첩 (DeepL)

  [API 응답 예시]
    {
      "translations": [
        {
          "detected_source_language": "EN",
          "text": "안녕하세요"
        }
      ]
    }

  [MORT Response 입력칸에 적는 값]
    "translations": [
      {
        "text": {RESULT_TEXT}
      }
    ]

  → MORT는 "text" 키를 응답 JSON 안에서 재귀적으로 찾습니다.
    중첩 깊이는 자유입니다.


▶ 예시 3 - 일반 번역 API (result 키)

  [API 응답 예시]
    {
      "errorCode": "0",
      "errorMessage": "",
      "result": "안녕하세요"
    }

  [MORT Response 입력칸에 적는 값]
    "result": {RESULT_TEXT}


▶ 동작 원리 (꼭 알아둘 것)

  MORT는 Response 템플릿에서 {RESULT_TEXT} 바로 앞의 키 이름을
  찾아낸 후, 실제 응답 JSON에서 그 키 이름을 가진 값을 재귀적으로
  탐색합니다.

  → 따라서 키 이름만 정확하면 응답 구조의 중첩 깊이는 무관합니다.
  → 단, 같은 키 이름이 여러 군데 있으면 가장 먼저 발견되는
     값이 반환됩니다. 의도와 다르면 응답 구조를 더 명시적으로
     적어 의도한 위치만 매칭되도록 하세요.


▶ {RESULT_TEXT} 를 적을 때 따옴표는?

  넣어도 되고 빼도 됩니다. 둘 다 인식합니다.
    "text": {RESULT_TEXT}     ← OK
    "text": "{RESULT_TEXT}"   ← OK


================================================================
5. 전체 워크플로우 예시
================================================================

가상의 "MyTranslate API" 문서가 다음과 같다고 가정합니다.

  [엔드포인트]
    POST https://api.mytrans.com/v1/translate

  [헤더]
    Authorization: Bearer <YOUR_TOKEN>
    Content-Type: application/json

  [요청 본문]
    {
      "q": "Hello",
      "from": "en",
      "to": "ko",
      "format": "text"
    }

  [응답 본문]
    {
      "code": 0,
      "data": {
        "translatedText": "안녕"
      }
    }


이를 MORT 프리셋으로 옮기면 :

  Name    : MyTranslate
  Url     : https://api.mytrans.com/v1/translate
  Headers : Authorization: Bearer YOUR_TOKEN
            (Content-Type 은 자동 추가되므로 생략)

  Request 입력칸 :
    "q": "{OCR_TEXT}",
    "from": "{SOURCE_CODE}",
    "to": "{RESULT_CODE}",
    "format": "text"

  Response 입력칸 :
    "data": {
      "translatedText": {RESULT_TEXT}
    }


================================================================
6. 실전 예시 - Ollama (로컬 LLM)
================================================================

  Name    : Ollama
  Url     : http://localhost:11434/api/generate
  Headers : (없음)

  Request 입력칸 :
    model = "translategemma",
    prompt = "You are a professional {SOURCE_CODE} to {RESULT_CODE} translator. Output ONLY the translation result. Translate the following {SOURCE_CODE} text into {RESULT_CODE}:\r\n\r\n{OCR_TEXT}",
    stream = false

  Response 입력칸 :
    response = {RESULT_TEXT}

설명 :
  · Ollama 는 사전에 로컬에 설치/실행되어 있어야 합니다.
      ollama serve
      ollama pull translategemma
  · stream 은 반드시 false. (스트리밍 응답은 미지원)
  · 다른 모델로 바꾸려면 model 값만 교체.
      예) "qwen2.5:7b", "llama3.1"


================================================================
7. 실전 예시 - DeepL (공식 API)
================================================================

  Name    : DeepL
  Url     : https://api-free.deepl.com/v2/translate
  Headers : Authorization: DeepL-Auth-Key YOUR_API_KEY:fx

  Request 입력칸 :
    "text": ["{OCR_TEXT}"],
    "target_lang": "{RESULT_CODE}"

  Response 입력칸 :
    "translations": [
      {
        "text": {RESULT_TEXT}
      }
    ]

설명 :
  · DeepL 은 text 를 "배열" 로 받습니다. 단일 문장도 [] 안에 넣어야 합니다.
  · source_lang 은 생략 시 자동 감지됩니다.
  · 언어 코드는 대문자(EN, JA, KO, ZH 등). 소문자로 거부될 경우
    "target_lang": "KO" 처럼 직접 대문자로 적습니다.
  · Free 플랜은 키 끝에 ":fx" 가 붙습니다.
  · Pro 플랜으로 옮길 때
      Url    : https://api.deepl.com/v2/translate
      Header : 키 끝의 ":fx" 제거


================================================================
8. 문제 해결 체크리스트
================================================================

번역이 안 되거나 에러가 날 때 순서대로 점검하세요.

  □ 1. 로그에 [CustomAPI] 혹은 "JSON 변환 실패" 메시지가 있는지
  □ 2. Request 템플릿이 올바른 JSON인지
        - 따옴표 짝, 콤마 누락, 토큰 자리 확인
        - 문자열 값에 큰따옴표가 있는지
  □ 3. Headers 형식이 "Key: Value" 인지 (콜론 누락 X)
  □ 4. 인증 키가 만료되거나 플랜이 다른 엔드포인트와 묶여있지 않은지
  □ 5. Response 템플릿에 {RESULT_TEXT} 가 키-값 짝으로 있는지
        예) "키": {RESULT_TEXT}
  □ 6. 같은 요청을 curl / Postman으로 보내 정상 응답이 오는지
  □ 7. 언어 코드 대소문자가 API 요구 사항과 맞는지

================================================================

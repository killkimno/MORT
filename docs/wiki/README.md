# MORT 구현 위키

`index.html`은 브라우저에서 바로 열 수 있는 단일 파일 위키다.

- 사람이 관리하는 공통 내용: `wiki-content.json`
- 기능별 상세 구현·작동 설명: `feature-content.json`
- 파일별 수동 상세 설명: `file-overrides.json`
- 코드에서 자동 수집하는 내용: 모든 C# 파일의 타입/메서드/의존 네임스페이스, DI 등록, TODO 계열 메모, 파일 크기
- 생성 명령: `powershell -NoProfile -ExecutionPolicy Bypass -File tools/update-wiki.ps1`
- 최신 상태 검사: 위 명령에 `-Check` 추가

코드를 바꿔 구현 의도, 작동 방식, 예시 또는 열린 질문의 답이 달라지면
`wiki-content.json`도 같은 작업에서 수정한다. `index.html`을 직접 수정하면 다음 생성 때
덮어써진다.

위키는 MORT 프로젝트 빌드 직전, 저장소의 pre-commit hook, 이 저장소에서 작업하는
에이전트의 작업 완료 절차에서 갱신된다.

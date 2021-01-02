using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// 어셈블리에 대한 일반 정보는 다음 특성 집합을 통해 
// 제어됩니다. 어셈블리와 관련된 정보를 수정하려면
// 이러한 특성 값을 변경하세요.
[assembly: AssemblyTitle("ScreenCapture")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ScreenCapture")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 표시되지 않습니다. COM에서 이 어셈블리의 형식에 액세스하려면
// 해당 형식에 대해 ComVisible 특성을 true로 설정하세요.
[assembly: ComVisible(false)]

//지역화 가능 애플리케이션 빌드를 시작하려면 다음을 설정하세요.
//.csproj 파일에서 <PropertyGroup> 내에 <UICulture>CultureYouAreCodingWith</UICulture>를
//설정하십시오. 예를 들어 소스 파일에서 영어(미국)를
//사용하는 경우 <UICulture>를 en-US로 설정합니다. 그런 다음 아래
//NeutralResourceLanguage 특성의 주석 처리를 제거합니다. 아래 줄의 "en-US"를 업데이트하여
//프로젝트 파일의 UICulture 설정과 일치시킵니다.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //테마별 리소스 사전의 위치
                                     //(페이지 또는 응용 프로그램 리소스 사진에
                                     // 리소스가 없는 경우에 사용됨)
    ResourceDictionaryLocation.SourceAssembly //제네릭 리소스 사전의 위치
                                              //(페이지 또는 응용 프로그램 리소스 사진에
                                              // 리소스가 없는 경우에 사용됨)
)]


// 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다.
//
//      주 버전
//      부 버전 
//      빌드 번호
//      수정 버전
//
// 모든 값을 지정하거나 아래와 같이 '*'를 사용하여 빌드 번호 및 수정 번호를
// 기본값으로 할 수 있습니다.
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

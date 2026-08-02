namespace MORT.Model
{
    internal sealed record TranslationProcessInitializationResult(
        bool CanStart,
        bool IsOnce,
        bool UseGoogleOcr,
        bool RequireOriginalScreen);
}

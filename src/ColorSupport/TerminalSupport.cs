using System.Linq;
using System.Text.RegularExpressions;
using ColorSupport.Internals;

namespace ColorSupport {
    public class TerminalSupport: ITerminalSupport {
        private readonly IEnvironment _env;
        internal TerminalSupport (IEnvironment environment) {
            _env = environment;
        }
        public TerminalSupport () {
            _env = new Environment ();
        }
        public SupportLevel Level => GetSupportLevel ();

        private SupportLevel GetSupportLevel () {
            SupportLevel? level = GetLevelFromEnvAndFlags ();

            if (level == SupportLevel.None) {
                return SupportLevel.None;
            }

            var minLevel = (SupportLevel) (level != null ? level.Value : 0);

            if (HasTrueColorFlag) {
                return SupportLevel.TrueColor;
            }

            if (Has256ColorFlag) {
                return SupportLevel.Color256;
            }

            if (_env.ConsoleIsRedirected () && level == null) {
                return SupportLevel.None;
            }

            if (TermIsDumb) {
                return minLevel;
            }

            if (_env.IsWindows ()) {
                return GetWindowsLevel ();
            }

            if (_env.HasEnvironmentVariable ("CI")) {
                return GetCILevel ();
            }

            if (_env.HasEnvironmentVariable ("TEAMCITY_VERSION")) {
                return GetTeamCityLevel ();
            }

            if (ColorTermIsTrueColor) {
                return SupportLevel.TrueColor;
            }

            if (_env.HasEnvironmentVariable ("TERM_PROGRAM")) {
                return GetAppleTerminalLevel ();
            }

            if (new Regex ("-256(color)$").IsMatch (_env.GetEnvironmentVariable ("TERM"))) {
                return SupportLevel.Color256;
            }

            if (new Regex ("(^screen|^xterm|^vt100|^vt220|^rxvt|color|ansi|cygwin|linux)").IsMatch (_env.GetEnvironmentVariable ("TERM"))) {
                return SupportLevel.Basic;
            }

            if (_env.HasEnvironmentVariable ("COLORTERM")) {
                return SupportLevel.Basic;
            }

            return minLevel;
        }

        private SupportLevel? GetLevelFromEnvAndFlags () {
            SupportLevel? level = null;

            if (DisableColorsFlags) {
                level = SupportLevel.None;
            }
            if (EnableColorFlags) {
                level = SupportLevel.Basic;
            }

            if (HasColorForceEnvironmentVariable) {
                level = LevelFromForceColor ();
            }

            return level;
        }

        private bool DisableColorsFlags => _env.HasFlag ("no-color") ||
            _env.HasFlag ("no-colors") ||
            _env.HasFlag ("color=false") ||
            _env.HasFlag ("color=never");

        private bool EnableColorFlags => _env.HasFlag ("color") ||
            _env.HasFlag ("colors") ||
            _env.HasFlag ("color=true") ||
            _env.HasFlag ("color=always");

        private bool HasColorForceEnvironmentVariable => _env.HasEnvironmentVariable ("FORCE_COLOR");

        private bool HasTrueColorFlag =>
            _env.HasFlag ("color=16m") ||
            _env.HasFlag ("color=full") ||
            _env.HasFlag ("color=truecolor");

        private bool ColorTermIsTrueColor => _env.GetEnvironmentVariable ("COLORTERM") == "truecolor";

        private bool TermIsDumb => _env.GetEnvironmentVariable ("TERM") == "dumb";

        private bool Has256ColorFlag => _env.HasFlag ("--color=256");
        private SupportLevel LevelFromForceColor () {
            var forceColor = _env.GetEnvironmentVariable ("FORCE_COLOR");

            if (forceColor == "true") {
                return SupportLevel.Basic;
            }

            if (forceColor == "false") {
                return SupportLevel.None;
            }

            if (int.TryParse (forceColor, out var level)) {
                return (SupportLevel) System.Math.Min (level, 3);
            }

            return SupportLevel.None;
        }

        private SupportLevel GetWindowsLevel () {
            var windowsVersion = _env.GetOsVersion ();
            if (windowsVersion.Major >= 10 && windowsVersion.Build >= 10586) {
                return windowsVersion.Build >= 14931 ? SupportLevel.TrueColor : SupportLevel.Color256;
            }

            return SupportLevel.Basic;
        }
        private SupportLevel GetCILevel () {
            var ciProviders = new [] { "TRAVIS", "CIRCLECI", "APPVEYOR", "GITLAB_CI", "GITHUB_ACTIONS", "BUILDKITE" };
            if (ciProviders.Any (p => _env.HasEnvironmentVariable (p)) || _env.GetEnvironmentVariable ("CI_NAME") == "codeship") {
                return SupportLevel.Basic;
            }
            return SupportLevel.None;
        }

        private SupportLevel GetTeamCityLevel () {
            const string versionStringAbove9Point1Regex = @"(^(9.([1 - 9]))|^([1-9][0-9].))";
            return new Regex (versionStringAbove9Point1Regex)
                .IsMatch (_env.GetEnvironmentVariable ("TEAMCITY_VERSION")) ? SupportLevel.Basic : SupportLevel.None;
        }

        private SupportLevel GetAppleTerminalLevel () {
            var version = int.Parse ((_env.HasEnvironmentVariable ("TERM_PROGRAM_VERSION") ? _env.GetEnvironmentVariable ("TERM_PROGRAM_VERSION") : string.Empty).Split ('.') [0]);

            var termProgram = _env.GetEnvironmentVariable ("TERM_PROGRAM");

            if (termProgram == "iTerm.app") {
                return version >= 3 ? SupportLevel.TrueColor : SupportLevel.Color256;
            }

            if (termProgram == "Apple_Terminal") {
                return SupportLevel.Color256;
            }

            return SupportLevel.Basic;
        }
    }
}
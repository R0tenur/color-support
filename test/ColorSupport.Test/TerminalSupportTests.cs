using ColorSupport.Internals;
using FakeItEasy;
using Shouldly;
using Xunit;

namespace ColorSupport.Test
{
    public class TerminalSupportTests
    {
        private readonly TerminalSupport _consoleColor;
        private readonly IEnvironment _env;
        public TerminalSupportTests()
        {
            _env = A.Fake<IEnvironment>();
            _consoleColor = new TerminalSupport(_env);
        }
        [Fact]
        public void GetSupportLevel_Returns_Basic_When_FORCE_COLOR_Is_true_In_Env()
        {
            SetEnvironmentVar("FORCE_COLOR", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_FORCE_COLOR_Is_1_In_Env()
        {
            SetEnvironmentVar("FORCE_COLOR", "1");

            _consoleColor.Level.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_FORCE_COLOR_Is_true_In_Env_And_256_Flag()
        {
            SetEnvironmentVar("FORCE_COLOR", "true");
            SetFlag("--color=256");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_FORCE_COLOR_Is_1_In_Env_And_256_Flag()
        {
            SetEnvironmentVar("FORCE_COLOR", "1");
            SetFlag("--color=256");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_Not_TTY()
        {
            A.CallTo(() => _env.ConsoleIsRedirected()).Returns(true);

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_nocolor_flag_is_used()
        {
            SetEnvironmentVar("TERM", "term-256color");
            SetFlag("no-color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_color_flag_is_used()
        {
            SetFlag("color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_colors_flag_is_used()
        {
            SetFlag("colors");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_color_flag_is_true()
        {
            SetFlag("color=true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_COLORTERM_is_in_env()
        {
            SetEnvironmentVar("COLORTERM", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_COLORTERM_is_true_in_env()
        {
            SetEnvironmentVar("COLORTERM", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_COLORTERM_is_truecolor_in_env()
        {
            SetEnvironmentVar("COLORTERM", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_color_is_in_flags()
        {
            SetFlag("color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_colors_is_in_flags()
        {
            SetFlag("colors");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_color_is_true_in_flags()
        {
            SetFlag("color=true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_color_is_false_in_flags()
        {
            SetFlag("color=false");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_color_is_256_in_flag()
        {
            SetFlag("--color=256");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_color_is_16m_in_flag()
        {
            SetFlag("color=16m");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.TrueColor);
        }

        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_color_is_full_in_flag()
        {
            SetFlag("color=full");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.TrueColor);
        }

        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_color_is_truecolor_in_flag()
        {
            SetFlag("color=truecolor");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.TrueColor);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_CI_In_Env()
        {
            SetEnvironmentVar("CI", "DummyProvider");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_and_Travis_Is_In_Env()
        {
            SetEnvironmentVar("CI", "TRAVIS");
            SetEnvironmentVar("TRAVIS", "1");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_and_CIRCLECI_Is_In_Env()
        {
            SetEnvironmentVar("CI", "CIRCLECI");
            SetEnvironmentVar("CIRCLECI", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_and_APPVEYOR_Is_In_Env()
        {
            SetEnvironmentVar("CI", "true");
            SetEnvironmentVar("APPVEYOR", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_and_GITLAB_CI_Is_In_Env()
        {
            SetEnvironmentVar("CI", "true");
            SetEnvironmentVar("GITLAB_CI", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_and_BUILDKITE_Is_In_Env()
        {
            SetEnvironmentVar("CI", "true");
            SetEnvironmentVar("BUILDKITE", "true");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_CI_Is_In_Env_And_CI_name_Is_codeship()
        {
            SetEnvironmentVar("CI", "true");
            SetEnvironmentVar("CI_NAME", "codeship");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_TEAMCITY_VERSION_In_Env_And_Version_Is_Less_Than_9_1()
        {
            SetEnvironmentVar("TEAMCITY_VERSION", "9.0.5 (build 32523)");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }


        [Fact]
        public void GetSupportLevel_Returns_Basic_When_TEAMCITY_VERSION_In_Env_And_Version_Is_Bigger_Or_equal_To_9_1()
        {
            SetEnvironmentVar("TEAMCITY_VERSION", "9.1.0 (build 32523)");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_Basic_When_TERM_IS_rxvt_In_env()
        {
            SetEnvironmentVar("TERM", "rxvt");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_TERM_IS_xterm_256color_And_COLORTERM_IS_1_In_env()
        {
            SetEnvironmentVar("COLORTERM", "1");
            SetEnvironmentVar("TERM", "xterm-256color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_TERM_IS_screen_256color_In_env()
        {
            SetEnvironmentVar("TERM", "screen-256color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_TERM_IS_putty_256color_In_env()
        {
            SetEnvironmentVar("TERM", "putty-256color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_Using_iTerm3()
        {
            SetEnvironmentVar("TERM_PROGRAM", "iTerm.app");
            SetEnvironmentVar("TERM_PROGRAM_VERSION", "3.0.10");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.TrueColor);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_Using_iTerm_Below_Version_3()
        {
            SetEnvironmentVar("TERM_PROGRAM", "iTerm.app");
            SetEnvironmentVar("TERM_PROGRAM_VERSION", "2.9.3");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }


        [Fact]
        public void GetSupportLevel_Returns_Basic_When_Using_Older_Windows_Than_10_build_10586()
        {
            A.CallTo(() => _env.IsWindows()).Returns(true);
            A.CallTo(() => _env.GetOsVersion()).Returns(new System.Version(10, 0, 10585));

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Basic);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_Using_Newer_Windows_Than_10_build_10586_And_Older_Than_14931()
        {
            A.CallTo(() => _env.IsWindows()).Returns(true);
            A.CallTo(() => _env.GetOsVersion()).Returns(new System.Version(10, 0, 10586));

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }
        [Fact]
        public void GetSupportLevel_Returns_TrueColor_When_Using_Newer_Windows_Than_10_build_14931()
        {
            A.CallTo(() => _env.IsWindows()).Returns(true);
            A.CallTo(() => _env.GetOsVersion()).Returns(new System.Version(10, 0, 14931));

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.TrueColor);
        }

        [Fact]
        public void GetSupportLevel_Returns_256_When_Not_TTY_In_xterm256()
        {
            A.CallTo(() => _env.ConsoleIsRedirected()).Returns(true);
            SetEnvironmentVar("FORCE_COLOR", "true");
            SetEnvironmentVar("TERM", "xterm-256color");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.Color256);
        }

        [Fact]
        public void GetSupportLevel_Returns_None_When_TERIM_Is_dumb_in_env()
        {
            A.CallTo(() => _env.ConsoleIsRedirected()).Returns(true);
            SetEnvironmentVar("TERM", "dumb");

            var result = _consoleColor.Level;

            result.ShouldBe(SupportLevel.None);
        }

        private void HasEnvironmentVarReturns(string var, bool returns)
        {
            A.CallTo(() => _env.HasEnvironmentVariable(var)).Returns(returns);
        }
        private void SetEnvironmentVar(string key, string value)
        {
            A.CallTo(() => _env.HasEnvironmentVariable(key)).Returns(true);

            A.CallTo(() => _env.GetEnvironmentVariable(key)).Returns(value);
        }

        private void SetFlag(string key)
        {
            A.CallTo(() => _env.HasFlag(key)).Returns(true);
        }
    }
}

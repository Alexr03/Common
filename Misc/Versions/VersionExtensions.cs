namespace Alexr03.Common.Misc.Version
{
    public static class VersionExtension
    {
        public static System.Version IncrementRevision(this System.Version version)
        {
            return AddVersion(version, 0, 0, 0, 1);
        }

        public static System.Version IncrementBuild(this System.Version version)
        {
            return IncrementBuild(version, true);
        }

        public static System.Version IncrementBuild(this System.Version version, bool resetLowerNumbers)
        {
            return AddVersion(version, 0, 0, 1, resetLowerNumbers ? -version.Revision : 0);
        }

        public static System.Version IncrementMinor(this System.Version version)
        {
            return IncrementMinor(version, true);
        }

        public static System.Version IncrementMinor(this System.Version version, bool resetLowerNumbers)
        {
            return AddVersion(version, 0, 1, resetLowerNumbers ? -version.Build : 0,
                resetLowerNumbers ? -version.Revision : 0);
        }

        public static System.Version IncrementMajor(this System.Version version)
        {
            return IncrementMajor(version, true);
        }

        public static System.Version IncrementMajor(this System.Version version, bool resetLowerNumbers)
        {
            return AddVersion(version, 1, resetLowerNumbers ? -version.Minor : 0,
                resetLowerNumbers ? -version.Build : 0, resetLowerNumbers ? -version.Revision : 0);
        }

        public static System.Version AddVersion(this System.Version version, string addVersion)
        {
            return AddVersion(version, new System.Version(addVersion));
        }

        public static System.Version AddVersion(this System.Version version, System.Version addVersion)
        {
            return AddVersion(version, addVersion.Major, addVersion.Minor, addVersion.Build, addVersion.Revision);
        }

        public static System.Version AddVersion(this System.Version version, int major, int minor, int build, int revision)
        {
            return SetVersion(version, version.Major + major, version.Minor + minor, version.Build + build,
                version.Revision + revision);
        }

        public static System.Version SetVersion(this System.Version version, int major, int minor, int build,
            int revision)
        {
            return new System.Version(major, minor, build, revision);
        }

        /*
        //one day we may even be able to do something like this...
        //https://github.com/dotnet/csharplang/issues/192
        public static Version operator +(Version version, int revision) {
            return AddVersion(version, 0, 0, 0, revision);
        }
        public static Version operator ++(Version version) {
            return IncrementVersion(version);
        }   
        */
    }
}
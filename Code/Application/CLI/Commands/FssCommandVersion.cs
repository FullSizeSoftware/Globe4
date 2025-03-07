using System.Collections.Generic;

// FssCommandVersion

public class FssCommandVersion : FssCommand
{
    public FssCommandVersion()
    {
        Signature.Add("version");
    }

    public override string Execute(List<string> parameters)
    {
        FssCentralLog.AddEntry("FssCommandVersion.Execute: " + FssFssbals.VersionString);
        return FssFssbals.VersionString;
    }

}

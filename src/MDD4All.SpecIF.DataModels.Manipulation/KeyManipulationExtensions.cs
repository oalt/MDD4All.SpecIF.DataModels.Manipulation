namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class KeyManipulationExtensions
    {
        public static void InitailizeFromKeyString(this Key key, string keyString)
        {
            if(keyString.Contains("_R_"))
            {
                int splitIndex = keyString.IndexOf("_R_");

                if(splitIndex >= 0)
                {
                    string id = keyString.Substring(0, splitIndex);

                    string revision = keyString.Substring(splitIndex + 3);

                    key.ID = id;
                    key.Revision = revision;
                }
            }
        }
    }
}

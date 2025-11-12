namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class SpecIfManipulationExtensions
    {
        public static bool ContainsData(this SpecIF specIF)
        {
            bool result = false;

            if(specIF.Resources != null && specIF.Resources.Count > 0)
            {
                result = true;
            }

            if (specIF.Statements != null && specIF.Statements.Count > 0)
            {
                result = true;
            }

            if (specIF.Hierarchies != null && specIF.Hierarchies.Count > 0)
            {
                result = true;
            }

            if (specIF.Files != null && specIF.Files.Count > 0)
            {
                result = true;
            }

            return result;
        }
    }
}

using psn.PH;

namespace psn.PH
{
    public class BuildNumberChecker
    {
        public static void Main(string[] args)
        {
            Stripe_Ext me = new Stripe_Ext();
            Console.WriteLine(me.GetBuildInfo_Ext());
        }
    }
}
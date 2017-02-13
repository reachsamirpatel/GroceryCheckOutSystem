using BusinessProcess;

namespace GroceryCheckOutSystem.Console
{
    public class MarketingConsole
    {
        public static void Start(UserBP userBp)
        {
            System.Console.WriteLine("You have access to Settings only.");
            while (true)
            {
                System.Console.Write("Select application mode ([E]xit,[S]ettings)");

                string mode = System.Console.ReadLine()?.ToLowerInvariant();
                switch (mode)
                {
                    case "e":
                        return;
                        break;
                    case "s":
                        System.Console.WriteLine("Entering settings...");
                        SettingsBP settings = new SettingsBP(userBp.CurrentUser);
                        settings.Start();
                        break;
                    default:
                        System.Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }
    }
}
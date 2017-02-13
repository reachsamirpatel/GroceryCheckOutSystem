using BusinessProcess;

namespace GroceryCheckOutSystem.Console
{
    class AdminConsole
    {
        public static void Start(UserBP userBp)
        {
            while (true)
            {
                System.Console.Write("Select application mode ([C]heckout,[S]ettings)");

                string mode = System.Console.ReadLine()?.ToLowerInvariant();
                System.Console.WriteLine("You have selected {0}", mode);
                switch (mode)
                {
                    case "c":
                        System.Console.WriteLine("Entering checkout mode...");
                        CheckOutBP checkOutBp = new CheckOutBP(userBp.CurrentUser);
                        checkOutBp.Start();
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
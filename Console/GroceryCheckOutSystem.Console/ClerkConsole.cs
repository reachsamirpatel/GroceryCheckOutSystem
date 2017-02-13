using BusinessProcess;

namespace GroceryCheckOutSystem.Console
{
    //Console app to perform actions Clerk Usertype.
    //Only checkout is performed by the user.
    public class ClerkConsole
    {
        public static void Start(UserBP userBp)
        {
            System.Console.WriteLine("You have access to checkout only.");
            while (true)
            {
                System.Console.Write("Select application mode ([E]xit,[C]heckout)");

                string mode = System.Console.ReadLine()?.ToLowerInvariant();
                switch (mode)
                {
                    case "e":
                        return;
                        break;
                    case "c":
                        System.Console.WriteLine("Entering checkout mode...");
                        CheckOutBP checkOutBp = new CheckOutBP(userBp.CurrentUser);
                        checkOutBp.Start();
                        break;
                    default:
                        System.Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }
    }
}
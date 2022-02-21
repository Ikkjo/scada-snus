using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class Program
    {
        private static string token = null;
        private static DBManagerServiceReference.DBManagerServiceClient proxy = new DBManagerServiceReference.DBManagerServiceClient();

        static void Main(string[] args)
        {
            while (true)
            {

                while (token == null || token == "invalid")
                {
                    token = LogIn();

                    CheckIfTokenValid();
                }

                while (token != null)
                {
                    PrintMainMenu();

                    string userChoice = GetUserChoice();

                    switch (userChoice)
                    {
                        case "1":
                            AddTag();
                            break;

                        case "2":
                            RemoveTag();
                            break;

                        case "3":
                            ChangeOutputTagValue();
                            break;

                        case "4":
                            GetOutputTagValue();
                            break;

                        case "5":
                            EnableScan();
                            break;

                        case "6":
                            DisableScan();
                            break;

                        case "7":
                            RegisterUser();
                            break;

                        case "8":
                            LogOut();
                            token = null;
                            break;

                        default:
                            break;
                    }

                }
            }

        }

        private static string LogIn()
        {
            LoginCredentials lc = getLoginCredentials();
            string token = proxy.LogIn(lc.Username, lc.Password);
            return token;
        }

        private static void AddTag()
        {
            PrintTagTypeMenu();
            string tagType = GetUserChoice();
            
            DBManagerServiceReference.Tag newTag = null;

            newTag = GetTagFromUserInput(tagType);

            bool successful = proxy.AddTag(newTag);
        }

        private static void RemoveTag()
        {

        }

        private static void ChangeOutputTagValue()
        {

        }

        private static void GetOutputTagValue()
        {

        }

        private static void EnableScan()
        {

        }

        private static void DisableScan()
        {

        }

        private static void RegisterUser()
        {

        }

        private static void LogOut()
        {
            proxy.LogOut(token);
        }

        private static LoginCredentials getLoginCredentials()
        {
            PrintLoginMenu();
            return new LoginCredentials(getUsername(), getPassword());
        }

        private static string GetUserChoice()
        {
            string userChoice = "-1";
            Console.Write("Odaberite željenu opciju: ");
            userChoice = Console.ReadLine();
            return userChoice;
        }

        private static void PrintMainMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("\n=====================\n=====GLAVNI MENI=====\n=====================\n");
            System.Console.WriteLine("  1. Dodaj tag\n");
            System.Console.WriteLine("  2. Ukloni tag\n");
            System.Console.WriteLine("  3. Promeni vrednost izlaznog taga\n");
            System.Console.WriteLine("  4. Dobavi vrednost izlaznog taga\n");
            System.Console.WriteLine("  5. Uključi skeniranje za ulazni tag\n");
            System.Console.WriteLine("  6. Isključi skeniranje za ulazni tag\n");
            System.Console.WriteLine("  7. Registruj novog korisnika\n");
            System.Console.WriteLine("  8. Izloguj se\n");
            System.Console.WriteLine("\n=====================\n=====================\n=====================\n");
        }

        private static void PrintLoginMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("\n=====================\n========LOGIN========\n===(esc za izlaz)====\n");
        }

        private static void PrintTagTypeMenu()
        {
            System.Console.WriteLine("\n=====================\n=====================\n=====================\n");
            System.Console.WriteLine("Odaberi vrstu taga:\n\n");
            System.Console.WriteLine("  1. Analog input tag\n");
            System.Console.WriteLine("  2. Digital input tag tag\n");
            System.Console.WriteLine("  3. Analog output tag\n");
            System.Console.WriteLine("  4. Digital output tag\n");
            System.Console.WriteLine("\n=====================\n=====================\n=====================\n");
        }

        private static DBManagerServiceReference.Tag GetTagFromUserInput(string tagType)
        {
            switch (tagType)
            {
                case "1":
                    return GetAITagFromUserInput();

                case "2":
                    return GetDITagFromUserInput();

                case "3":
                    return GetAOTagFromUserInput();

                case "4":
                    return GetDOTagFromUserInput();

                default:
                    return null;
            }


            return null;
        }

        private static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }

        private static string getUsername()
        {
            string username = "";
            System.Console.Write("Upiši korisničko ime: ");
            username = System.Console.ReadLine();
            return username;
        }

        private static string getPassword()
        {
            string password = "";
            Console.Write("Upiši lozinku: ");

            ConsoleKeyInfo nextKey = Console.ReadKey(true);

            while (nextKey.Key != ConsoleKey.Enter)
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        // erase the last * as well
                        Console.Write(nextKey.KeyChar);
                        Console.Write(" ");
                        Console.Write(nextKey.KeyChar);
                    }
                }
                else
                {
                    password += nextKey.KeyChar;
                    Console.Write("*");
                }
                nextKey = Console.ReadKey(true);
            }
            return password;
        }

        public static void CheckIfTokenValid()
        {
            if (token == "invalid")
            {
                System.Console.WriteLine("\nLogin neuspešan, pokušajte ponovo...");
                System.Console.ReadKey();
            }
        }

    }

    enum TagType {
        AI,
        DI,
        AO,
        DO
    }

    struct LoginCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; } 

        public LoginCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

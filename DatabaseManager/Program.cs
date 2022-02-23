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
            return proxy.LogIn(lc.Username, lc.Password);
        }

        // refactor posle ovoga

        private static void AddTag()
        {
            PrintTagTypeMenu();
            string tagType = GetUserChoice();
            
            DBManagerServiceReference.Tag newTag = null;

            newTag = GetTagFromUserInput(tagType);

            bool successful = proxy.AddTag(newTag);

            PrintResultMessage("Dodavanje novog taga", successful);
        }

        private static void RemoveTag()
        {
            System.Console.Clear();
            System.Console.WriteLine("Brisanje taga\n\n");
            string tagName = GetTagNameFromUser();
            bool succ = proxy.RemoveTag(tagName);
            PrintResultMessage("Uklanjanje taga", succ);
        }

        private static void ChangeOutputTagValue()
        {
            System.Console.Clear();
            System.Console.WriteLine("Promena vrednosti izlaznog taga\n\n");
            string tagName = GetTagNameFromUser();
            float? currVal = proxy.GetOutputValue(tagName);

            if (currVal == null)
            {
                System.Console.WriteLine($"\n\nTag {tagName} ne postoji! (ili je izabran tag koji nije output tag)...");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine($"\nTrenutna vrednost izlaznog taga je: {currVal}");
            System.Console.Write("Unesite novu vrednost taga: ");
            float newVal = float.Parse(System.Console.ReadLine());
            bool succ = proxy.ChangeOutputValue(tagName, newVal);
            PrintResultMessage($"Promena vrednosti izlaznog taga {tagName}", succ);
        }

        private static void GetOutputTagValue()
        {
            System.Console.Clear();
            System.Console.WriteLine("Ispis vrednosti izlaznog taga\n\n");
            string tagName = GetTagNameFromUser();
            float? result = proxy.GetOutputValue(tagName);
            if (result != null)
            {
                System.Console.WriteLine($"Vrednost traženog izlaznog taga je: {result}");
            } else
            {
                System.Console.WriteLine("Greška. Traženi izlazni tag nije pronađen");
            }
            System.Console.ReadKey();
        }

        private static void EnableScan()
        {
            System.Console.Clear();
            System.Console.WriteLine("Uključenje skeniranja ulaznog taga\n\n");
            string tagName = GetTagNameFromUser();
            bool succ = proxy.SetScan(tagName, true);
            PrintResultMessage("Uključenje skeniranje taga", succ);
        }

        private static void DisableScan()
        {
            System.Console.Clear();
            System.Console.WriteLine("Isključenje skeniranja ulaznog taga\n\n");
            string tagName = GetTagNameFromUser();
            bool succ = proxy.SetScan(tagName, false);
            PrintResultMessage("Isključenje skeniranje taga", succ);
        }

        private static void RegisterUser()
        {
            System.Console.Clear();
            System.Console.WriteLine("Registrovanje novog korisnika\n\n");
            string username = GetUsernameFromUser();
            string password = GetPasswordFromUser();
            bool succ = proxy.RegisterUser(username, password);
            PrintResultMessage("Registrovanje novog korisnika", succ);
        }

        private static void LogOut()
        {
            proxy.LogOut(token);
        }

        private static void PrintResultMessage(string message, bool success)
        {
            string succ = success ? "uspešno" : "neuspešno";
            System.Console.WriteLine($"\n\n{message}: {succ}");
            System.Console.WriteLine("Pritisnite taster za nastavak...");
            System.Console.ReadKey();

        }

        private static LoginCredentials getLoginCredentials()
        {
            PrintLoginMenu();
            return new LoginCredentials(GetUsernameFromUser(), GetPasswordFromUser());
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
            System.Console.Clear();
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
            System.Console.Clear();

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
        }

        private static DBManagerServiceReference.Tag GetAITagFromUserInput()
        {
            DBManagerServiceReference.AnalogInput newTag = new DBManagerServiceReference.AnalogInput();
            newTag.TagName = GetTagNameFromUser();
            newTag.Description = GetTagDataFromUser("opis taga");
            newTag.IOAddress = GetTagDataFromUser("I/O adresa");
            newTag.Driver = GetInputDriverFromUser();
            newTag.ScanTime = int.Parse(GetTagDataFromUser("vreme skeniranja"));
            newTag.LowLimit = float.Parse(GetTagDataFromUser("donja granica"));
            newTag.HighLimit = float.Parse(GetTagDataFromUser("gornja granica"));
            newTag.Units = GetTagDataFromUser("merna jedinica");
            newTag.ScanActive = false;
            newTag.Alarms = new List<DBManagerServiceReference.Alarm>().ToArray();
            return newTag;
        }

        private static DBManagerServiceReference.Tag GetDITagFromUserInput()
        {

            DBManagerServiceReference.DigitalInput newTag = new DBManagerServiceReference.DigitalInput();
            newTag.TagName = GetTagNameFromUser();
            newTag.Description = GetTagDataFromUser("opis taga");
            newTag.IOAddress = GetTagDataFromUser("I/O adresa");
            newTag.Driver = GetTagDataFromUser("Driver (SimulationDriver/RealTimeDriver");
            newTag.ScanTime = int.Parse(GetTagDataFromUser("vreme skeniranja"));
            newTag.ScanActive = false;
            return newTag;
        }

        private static DBManagerServiceReference.Tag GetAOTagFromUserInput()
        {

            DBManagerServiceReference.AnalogOutput newTag = new DBManagerServiceReference.AnalogOutput();
            newTag.TagName = GetTagNameFromUser();
            newTag.Description = GetTagDataFromUser("opis taga");
            newTag.IOAddress = GetTagDataFromUser("I/O adresa");
            newTag.InitialValue = float.Parse(GetTagDataFromUser("inicijalna vrednost"));
            newTag.Value = newTag.InitialValue;
            newTag.LowLimit = float.Parse(GetTagDataFromUser("donja granica"));
            newTag.HighLimit = float.Parse(GetTagDataFromUser("gornja granica"));
            newTag.Units = GetTagDataFromUser("merna jedinica");
            return newTag;
        }

        private static DBManagerServiceReference.Tag GetDOTagFromUserInput()
        {
            DBManagerServiceReference.DigitalOutput newTag = new DBManagerServiceReference.DigitalOutput();
            newTag.TagName = GetTagNameFromUser();
            newTag.Description = GetTagDataFromUser("opis taga");
            newTag.IOAddress = GetTagDataFromUser("I/O adresa");
            newTag.InitialValue = float.Parse(GetTagDataFromUser("inicijalna vrednost"));
            newTag.Value = newTag.InitialValue;
            return newTag;
        }

        private static string GetTagNameFromUser()
        {
            System.Console.WriteLine("Unesi ime taga:");
            return System.Console.ReadLine();
        }

        private static string GetTagDataFromUser(string description)
        {
            System.Console.WriteLine($"Unesi {description}:");
            string data = System.Console.ReadLine();

            return data == "" ? null : data;
        }

        private static string GetTagDescriptionFromUser()
        {
            System.Console.WriteLine("Unesi opis taga:");
            return System.Console.ReadLine();
        }

        private static DBManagerServiceReference.InputDriver GetInputDriverFromUser()
        {

            return new DBManagerServiceReference.InputDriver();
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

        private static string GetUsernameFromUser()
        {
            string username = "";
            System.Console.Write("Upiši korisničko ime: ");
            username = System.Console.ReadLine();
            return username;
        }

        private static string GetPasswordFromUser()
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

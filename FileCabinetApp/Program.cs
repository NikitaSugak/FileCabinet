namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Sugak Nikita";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static FileCabinetService fileCabinetService = new FileCabinetService();
        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "ptints the stat", "The 'stat' command prints the stat." },
            new string[] { "create", "create new write", "The 'crete' command crete new write." },
            new string[] { "list", "list all write", "The 'list' command list all write." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            if (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("First name don't be null or empty");
            }

            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            if (string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Last name don't be null or empty");
            }

            Console.Write("Date of birth: ");
            var stringateOfBirth = Console.ReadLine();

            DateTime dateOfBirth;
            if (string.IsNullOrEmpty(stringateOfBirth))
            {
                Console.WriteLine("Date of birth don't be null or empty");
            }

            if (!DateTime.TryParse(stringateOfBirth, out dateOfBirth))
            {
                Console.WriteLine("Date of birth be day/month/year");
            }

            fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth);

            Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] fileCabinetRecords = fileCabinetService.GetRecords();

            for (int i = 0; i < fileCabinetRecords.Length; i++)
            {
                Console.WriteLine($"#{fileCabinetRecords[i].Id}, {fileCabinetRecords[i].FirstName}, {fileCabinetRecords[i].LastName}, {fileCabinetRecords[i].DateOfBirth.ToString("yyyy-MMM-dd", new System.Globalization.CultureInfo("en-US"))}");
            }
        }
    }
}
    // See https://aka.ms/new-console-template for more information

    void ShowMenu()
    {
        Console.WriteLine(" Menu Choices:");
        Console.WriteLine(" =========================");
        Console.WriteLine(" 1. Import Colores");
        Console.WriteLine(" 2. Map Colores ");
        Console.WriteLine(" 3. Exit");
        Console.WriteLine(" =========================");
        Console.Write(" Select an option (1-3): ");
    }

    void HandleChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                Console.Clear();
                Console.WriteLine(" You selected 1. Import Colores");
                Console.Write(" Press any key to continue...");
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                Console.WriteLine(" You selected 2. Map Colores");
                Console.Write(" Press any key to continue...");
                Console.ReadKey();
                break;
            case 3:
                Console.WriteLine(" Exiting application. Press Any Key");
                Console.ReadKey();
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                Console.Write(" Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }

    while (true)
    {
        Console.Clear();
        ShowMenu();
        var input = Console.ReadLine();
        if (int.TryParse(input, out int choice))
        {
            HandleChoice(choice);
            if (choice == 3)
                break;
        }
        else
        {
            Console.WriteLine("Invalid input.");
            Console.Write(" Press any key to continue...");
            Console.ReadKey();
        }
    }

using System;
using System.Collections.Generic;

class Item
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

class Order
{
    public Item SelectedItem { get; set; }
    public decimal TotalPrice => SelectedItem.Price;
}

class Menu
{
    public List<Item> Items { get; set; } = new List<Item>();

    public void DisplayMenu()
    {
        Console.WriteLine("Menu:");
        foreach (var item in Items)
        {
            Console.WriteLine($"{item.Name} - {item.Price:C} - ilość: {item.Quantity}");
        }
    }
}

class VendingMachine
{
    private Menu menu = new Menu();
    private List<Order> orderHistory = new List<Order>();
    private decimal currentAmount = 0;

    public void Initialize()
    {
        // Dodaj przedmioty do automatu
        menu.Items.Add(new Item { Name = "Czekolada", Price = 2.5m, Quantity = 1 });
        menu.Items.Add(new Item { Name = "Chipsy", Price = 1.8m, Quantity = 10 });
        menu.Items.Add(new Item { Name = "Cola", Price = 1.0m, Quantity = 10 });
    }

    public void Run()
    {
        Initialize();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Witaj w automacie sprzedającym!");
            Console.WriteLine("1. Wybierz pozycję");
            Console.WriteLine("2. Sprawdź historię zamówień");
            Console.WriteLine("3. Sprawdź spis asortymentu");
            Console.WriteLine("0. Zakończ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    OrderItem();
                    break;

                case "2":
                    DisplayOrderHistory();
                    break;

                case "3":
                    menu.DisplayMenu();
                    Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                    Console.ReadLine();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void OrderItem()
    {
        Console.Clear();
        menu.DisplayMenu();

        Console.WriteLine($"Wrzuć monetę od 1 zł do 5 zł (0 - aby wrócić): ");
        string coinInput = Console.ReadLine();

        if (coinInput == "0")
            return;

        if (decimal.TryParse(coinInput, out decimal coin) && coin >= 1.0m && coin <= 5.0m)
        {
            currentAmount += coin;
            Console.WriteLine($"Aktualna kwota: {currentAmount:C}");
        }
        else
        {
            Console.WriteLine("Nieprawidłowa moneta. Wrzuć monetę od 1 zł do 5 zł.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Wybierz pozycję (1, 2, 3, ..., 0 - aby wrócić): ");
        string choice = Console.ReadLine();

        if (choice == "0")
            return;

        if (int.TryParse(choice, out int selectedOption) && selectedOption > 0 && selectedOption <= menu.Items.Count)
        {
            Item selectedItem = menu.Items[selectedOption - 1];
            if (selectedItem.Quantity == 0)
            {
                Console.WriteLine("Brak produktu.");
            }
            else if (currentAmount >= selectedItem.Price)
            {
                Order order = new Order { SelectedItem = selectedItem };
                orderHistory.Add(order);
                currentAmount -= selectedItem.Price;
                selectedItem.Quantity -= 1;

                Console.WriteLine($"Zamówienie złożone: {selectedItem.Name} - Łączna cena: {order.TotalPrice:C}");
                Console.WriteLine($"Reszta: {currentAmount:C}");
                currentAmount = 0;
            }
            else
            {
                Console.WriteLine("Niewystarczająca kwota. Dodaj więcej monet.");
            }

            Console.WriteLine("Naciśnij Enter, aby kontynuować...");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować...");
            Console.ReadLine();
        }
    }

    private void DisplayOrderHistory()
    {
        Console.Clear();
        Console.WriteLine("Historia zamówień:");

        foreach (var order in orderHistory)
        {
            Console.WriteLine($"{order.SelectedItem.Name}");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
        Console.ReadLine();
    }
}

class Program
{
    static void Main()
    {
        VendingMachine vendingMachine = new VendingMachine();
        vendingMachine.Run();
    }
}

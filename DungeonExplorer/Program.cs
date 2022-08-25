Game game = new Game();

while (true)
{
    if (!game.Running)
        game.Generate(5);

    // zde vypis vseho
    Room room = game.GetRoom();
    Console.WriteLine(room.image);
    Console.WriteLine();
    Console.WriteLine(room.message);
    Console.WriteLine();
    for (int i = 0; i < room.choices.Length; i++)
    {
        Choice c = room.choices[i];
        Console.WriteLine($"[{i + 1}] {c.title}");
    }
    Console.WriteLine();
    Console.Write("> ");
    string input = Console.ReadLine();
    if (int.TryParse(input, out int index))
    {
        game.Choose(index - 1);
    }

    // zde ovladani hry

    Console.Clear();
}
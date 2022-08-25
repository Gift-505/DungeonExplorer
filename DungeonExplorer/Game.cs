using Newtonsoft.Json;

class Game
{
    private Room[] _prefabs;
    private Random _rand = new Random();

    public List<Room> Rooms = new List<Room>();

    public bool Running { get; private set; }
    public int Room { get; private set; }
    public int Health { get; private set; }

    public Game()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        Directory.CreateDirectory("rooms");
        Directory.CreateDirectory("images");

        var files = Directory.GetFiles("rooms");
        var list = new List<Room>();

        foreach (var file in files)
        {
            try
            {
                var data = File.ReadAllText(file);
                var room = JsonConvert.DeserializeObject<Room>(data);

                if (room == null)
                    continue;

                var image = "";

                try
                {
                    image = File.ReadAllText($"images/{room.image}.txt");
                }
                catch { }

                room.image = image;

                list.Add(room);
            }
            catch
            {

            }
        }

        _prefabs = list.ToArray();
    }

    public void Generate(int rooms = 10)
    {
        Rooms.Clear();

        Room = 0;
        Health = 100;

        var last = new List<Room>(_prefabs);

        for (int i = 0; i < rooms; i++)
        {
            var index = _rand.Next(last.Count);
            var room = last[index];

            if (_prefabs.Length > 1)
            {
                last = new List<Room>(_prefabs);
                last.Remove(room);
            }

            Rooms.Add(room);
        }

        Running = true;
    }

    public Room GetRoom()
    {
        if (Room < 0 || Room >= Rooms.Count)
            return null;

        return Rooms[Room];
    }

    public void Choose(int index)
    {
        var room = GetRoom();
        if (room == null)
            return;

        if (index < 0 || index >= room.choices.Length)
            return;

        var choice = room.choices[index];
        if (!string.IsNullOrEmpty(choice.message))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"\n{choice.message}");
            Console.ForegroundColor = ConsoleColor.White;

            Thread.Sleep(1500);
        }

        if (choice.chance > 0 && _rand.NextDouble() * 100 <= choice.chance)
        {
            Health = Math.Max(0, Math.Min(100, Health + choice.chance_heal));

            var hasMsg = !string.IsNullOrEmpty(choice.chance_message);

            if (hasMsg)
            {
                if (choice.chance_heal < 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"\n{choice.chance_message}");
            }

            if (choice.chance_heal != 0)
            {
                if (choice.chance_heal > 0)
                    Console.WriteLine($"(+{choice.chance_heal} HP)");
                else
                    Console.WriteLine($"({choice.chance_heal} HP)");
            }

            Console.ForegroundColor = ConsoleColor.White;

            Thread.Sleep(3000);
        }

        if (Health <= 0 || Room == Rooms.Count - 1)
        {
            Running = false;

            return;
        }

        Room += 1;
    }
}
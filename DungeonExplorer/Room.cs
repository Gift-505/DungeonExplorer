using Newtonsoft.Json;

[JsonObject]
class Room
{
    public string message;
    public string image;
    public Choice[] choices;
}
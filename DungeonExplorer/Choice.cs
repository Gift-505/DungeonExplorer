using Newtonsoft.Json;

[JsonObject]
class Choice
{
    public string title;
    public string message;
    public string chance_message;

    public int chance_heal;
    public int chance;
}
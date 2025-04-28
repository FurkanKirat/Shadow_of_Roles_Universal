using Newtonsoft.Json;

namespace game.models.Settings
{
    [System.Serializable]
    public class UserSettings
    {
        
        public static readonly UserSettings DefaultSettings = new (Language.English, "Player");
        public Language Language{get; set;}
        public string Username{get; set;}

        [JsonConstructor]
        public UserSettings(Language language, string username) {
            Language = language;
            Username = username;
        }


        public override string ToString()
        {
            return $"Username: {Username}, Language: {Language}";
        }
    }
}
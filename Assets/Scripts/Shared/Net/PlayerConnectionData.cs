namespace Balloondle.Shared.Game
{
    public class PlayerConnectionData
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ServerPassword { get; set; }

        public PlayerConnectionData()
        {

        }

        public PlayerConnectionData(string name, string code, string serverPassword)
        {
            Name = name;
            Code = code;
            ServerPassword = serverPassword;
        }
    }
}

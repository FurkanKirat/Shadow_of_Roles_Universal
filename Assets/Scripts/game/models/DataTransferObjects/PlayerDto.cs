using game.models.roles.Templates;

namespace game.models.DataTransferObjects
{
    [System.Serializable]
    public class PlayerDto
    {
        public int SenderPlayerNumber { get; }
        public int ChosenPlayerNumber { get; }
        public RoleTemplate SenderRole { get; }

        public PlayerDto(int senderPlayerNumber, int chosenPlayerNumber, RoleTemplate senderRole)
        {
            SenderPlayerNumber = senderPlayerNumber;
            ChosenPlayerNumber = chosenPlayerNumber;
            SenderRole = senderRole;
        }
    }

}
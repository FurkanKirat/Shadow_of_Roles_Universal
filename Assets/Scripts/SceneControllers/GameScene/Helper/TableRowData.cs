namespace SceneControllers.GameScene.Helper
{
    public class TableRowData
    {
        public string NumberColumn {get; set;}
        public string NameColumn {get; set;}
        public string RoleColumn {get; set;}
        public string WinStatusColumn {get; set;}
        public string AliveStatusColumn {get; set;}
        public string CausesOfDeathColumn {get; set;}

        public TableRowData(string numberColumn, string nameColumn, string roleColumn, string winStatusColumn, string aliveStatusColumn, string causesOfDeathColumn)
        {
            NumberColumn = numberColumn;
            NameColumn = nameColumn;
            RoleColumn = roleColumn;
            WinStatusColumn = winStatusColumn;
            AliveStatusColumn = aliveStatusColumn;
            CausesOfDeathColumn = causesOfDeathColumn;
        }
    }
}
using System.Collections.Generic;
using game.Constants;
using Managers;
using SceneControllers.GameScene.Helper;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.GameEnd
{
    public class GameEndTable : MonoBehaviour
    {
        
        [SerializeField] private GameObject rowPrefab;
        [SerializeField] private Transform contentParent;
        
        public void PopulateTable(List<TableRowData> dataList)
        {
            foreach (Transform child in contentParent)
                Destroy(child.gameObject);

            var headers = new TableRowData(
                TextCategory.GameEnd.GetTranslation("number_column"),
                TextCategory.GameEnd.GetTranslation("name_column"),
                TextCategory.GameEnd.GetTranslation("role_column"),
                TextCategory.GameEnd.GetTranslation("win_loss_column"),
                TextCategory.GameEnd.GetTranslation("alive_dead_column"),
                TextCategory.GameEnd.GetTranslation( "causes_of_death_column")
            );
            CreateRow(headers, true);
            
            foreach (var row in dataList)
            {
               CreateRow(row);
            }
        }

        private void CreateRow(TableRowData row, bool isHeader = false)
        {
            CreateCell(row.NumberColumn, isHeader);
            CreateCell(row.NameColumn, isHeader);
            CreateCell(row.RoleColumn, isHeader);
            CreateCell(row.WinStatusColumn, isHeader);
            CreateCell(row.AliveStatusColumn, isHeader);
            CreateCell(row.CausesOfDeathColumn, isHeader);
        }
        private void CreateCell(string textContent, bool isHeader = false)
        {
            var cell = Instantiate(rowPrefab, contentParent);
            var text = cell.GetComponentInChildren<TextMeshProUGUI>();
            text.text = textContent;

            if (!isHeader) return;
            
            text.color = Color.yellow;
            var image = cell.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = new Color(0.2f, 0.2f, 0.2f);
            }
            
        }

    }
}
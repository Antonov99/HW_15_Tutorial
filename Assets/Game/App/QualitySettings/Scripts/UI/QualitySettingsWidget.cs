using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.App
{
    public sealed class QualitySettingsWidget : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown dropdown;

        private void Awake()
        {
            InitDropdown();
        }

        private void OnEnable()
        {
            dropdown.onValueChanged.AddListener(OnLevelChanged);
        }

        private void OnDisable()
        {
            dropdown.onValueChanged.RemoveListener(OnLevelChanged);
        }

        private void OnLevelChanged(int level)
        {
            QualitySettingsManager.SetLevel(level);
        }

        private void InitDropdown()
        {
            var options = QualitySettingsManager
                .GetLevelNames()
                .Select(it => it.ToUpper())
                .ToList();
            
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = QualitySettingsManager.GetLevel();
        }
    }
}
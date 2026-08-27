// TransferOperation.cs
using System.Collections.Generic;

namespace stroevkaI.Models
{
    /// <summary>
    /// Операция перевода техники между состояниями
    /// </summary>
    public class TransferOperation
    {
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
    }

    /// <summary>
    /// Доступные операции перевода
    /// </summary>
    public static class TransferOperations
    {
        public static List<TransferOperation> GetOperations()
        {
            return new List<TransferOperation>
            {
                new TransferOperation { FromState = "br", ToState = "rezerv", DisplayName = "Бр → Резерв", Icon = "→" },
                new TransferOperation { FromState = "rezerv", ToState = "br", DisplayName = "Резерв → Бр", Icon = "←" },
                new TransferOperation { FromState = "br", ToState = "remont", DisplayName = "Бр → Ремонт", Icon = "→" },
                new TransferOperation { FromState = "remont", ToState = "br", DisplayName = "Ремонт → Бр", Icon = "←" },
                new TransferOperation { FromState = "br", ToState = "tofirst", DisplayName = "Бр → ТО-1", Icon = "→" },
                new TransferOperation { FromState = "tofirst", ToState = "br", DisplayName = "ТО-1 → Бр", Icon = "←" },
                new TransferOperation { FromState = "br", ToState = "totow", DisplayName = "Бр → ТО-2", Icon = "→" },
                new TransferOperation { FromState = "totow", ToState = "br", DisplayName = "ТО-2 → Бр", Icon = "←" },
                new TransferOperation { FromState = "rezerv", ToState = "remont", DisplayName = "Резерв → Ремонт", Icon = "→" },
                new TransferOperation { FromState = "remont", ToState = "rezerv", DisplayName = "Ремонт → Резерв", Icon = "←" },
                new TransferOperation { FromState = "rezerv", ToState = "tofirst", DisplayName = "Резерв → ТО-1", Icon = "→" },
                new TransferOperation { FromState = "tofirst", ToState = "rezerv", DisplayName = "ТО-1 → Резерв", Icon = "←" },
                new TransferOperation { FromState = "rezerv", ToState = "totow", DisplayName = "Резерв → ТО-2", Icon = "→" },
                new TransferOperation { FromState = "totow", ToState = "rezerv", DisplayName = "ТО-2 → Резерв", Icon = "←" },
                new TransferOperation { FromState = "remont", ToState = "tofirst", DisplayName = "Ремонт → ТО-1", Icon = "→" },
                new TransferOperation { FromState = "tofirst", ToState = "remont", DisplayName = "ТО-1 → Ремонт", Icon = "←" },
                new TransferOperation { FromState = "remont", ToState = "totow", DisplayName = "Ремонт → ТО-2", Icon = "→" },
                new TransferOperation { FromState = "totow", ToState = "remont", DisplayName = "ТО-2 → Ремонт", Icon = "←" },
                new TransferOperation { FromState = "tofirst", ToState = "totow", DisplayName = "ТО-1 → ТО-2", Icon = "→" },
                new TransferOperation { FromState = "totow", ToState = "tofirst", DisplayName = "ТО-2 → ТО-1", Icon = "←" },
            };
        }

        /// <summary>
        /// Получить операции, доступные для состояния
        /// </summary>
        public static List<TransferOperation> GetOperationsForState(string state)
        {
            if (string.IsNullOrEmpty(state)) return new List<TransferOperation>();

            return GetOperations().FindAll(op => op.FromState == state);
        }

        /// <summary>
        /// Получить название состояния
        /// </summary>
        public static string GetStateDisplayName(string state)
        {
            return state switch
            {
                "br" => "Боевой расчет",
                "rezerv" => "Резерв",
                "remont" => "Ремонт",
                "tofirst" => "ТО-1",
                "totow" => "ТО-2",
                _ => state
            };
        }

        /// <summary>
        /// Получить цвет для состояния
        /// </summary>
        public static Color GetStateColor(string state)
        {
            return state switch
            {
                "br" => Color.Green,
                "rezerv" => Color.Blue,
                "remont" => Color.Red,
                "tofirst" => Color.Orange,
                "totow" => Color.Purple,
                _ => Color.Gray
            };
        }
    }
}

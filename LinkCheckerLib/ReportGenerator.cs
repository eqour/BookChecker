using System;
using System.Text;

namespace LinkCheckerLib
{
    public static class ReportGenerator
    {
        public static string GoodMsgPrefix { get; set; } = "ОК";
        public static string DoubtfulMsgPrefix { get; set; } = "Найдены совпадения по фразам: ";
        public static string BadMsgPrefix { get; set; } = "Ошибка. Код состояния: ";

        /// <summary>
        /// Генерирует отчёт изходя из информации о ссылке
        /// </summary>
        /// <param name="linkInfo">Данные о ссылке и её проверке</param>
        /// <returns>Строка с отчётом</returns>
        public static string GenerateReportBasedOnLinkInfo(LinkInfo linkInfo)
        {
            if (linkInfo == null || linkInfo.Link == null)
                throw new ArgumentNullException();

            StringBuilder reportBuilder = new StringBuilder();

            if (linkInfo.ResponseCode == 200)
            {
                if (linkInfo.FoundErrorMessageMatches.Length == 0)
                    reportBuilder.Append(GoodMsgPrefix);
                else
                    reportBuilder.Append($"{DoubtfulMsgPrefix}{string.Join(", ", linkInfo.FoundErrorMessageMatches)}");
            }
            else
                reportBuilder.Append($"{BadMsgPrefix}{linkInfo.ResponseCode}");

            return reportBuilder.ToString();
        }

        /// <summary>
        /// Генерирует отчёт изходя из информации о ссылке
        /// </summary>
        /// <param name="linkInfo">Данные о ссылке и её проверке</param>
        /// <returns>Строка с отчётом</returns>
        public static string GenerateReportBasedOnLinkInfo(LinkInfo[] linkInfo)
        {
            if (linkInfo == null)
                throw new ArgumentNullException();

            StringBuilder reportBuilder = new StringBuilder();

            for (int i = 0; i < linkInfo.Length; i++)
            {
                if (i < linkInfo.Length && i != 0)
                    reportBuilder.Append('\n');
                reportBuilder.Append(GenerateReportBasedOnLinkInfo(linkInfo[i]));
            }

            return reportBuilder.ToString();
        }
    }
}

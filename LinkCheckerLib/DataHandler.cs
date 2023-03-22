using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace LinkCheckerLib
{
    public class DataHandler
    {
        public static string OutputFilenamePrefix { get; set; } = "out_";

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private HandleLinksInfo _handlingParameters;

        public class StartWorkingEventArgs : EventArgs
        {
            public int NumberOfBooks { get; set; }
        }

        public class StopWorkingEventArgs : EventArgs { }

        public class StartCheckingWorkbookEventArgs : EventArgs
        {
            public string Filename { get; set; }
            public int NumberOfWorksheets { get; set; }
        }

        public class StartCheckingWorksheetEventArgs : EventArgs
        {
            public int NumberOfProcessedItems { get; set; }
        }

        public class ProgressEventArgs : EventArgs
        {
            public int NumberOfRemainingItems { get; set; }
        }

        public class LoadErrorEventArgs : EventArgs
        {
            public bool? ConfirmRestartLoad { get; set; } = null;
            public string PathToLoadFile { get; set; }
        }

        public class SaveErrorEventArgs : EventArgs
        {
            public bool? ConfirmRestartSave { get; set; } = null;
            public string PathToSaveFile { get; set; }
        }

        public event EventHandler<StartWorkingEventArgs> OnStartWorking;
        public event EventHandler<StopWorkingEventArgs> OnStopWorking;
        public event EventHandler<StartCheckingWorkbookEventArgs> OnStartCheckingWorkbook;
        public event EventHandler<StartCheckingWorksheetEventArgs> OnStartCheckingWorksheet;
        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler<LoadErrorEventArgs> OnLoadError;
        public event EventHandler<SaveErrorEventArgs> OnSaveError;
        public event EventHandler<EventArgs> OnLoadFileStart;
        public event EventHandler<EventArgs> OnLoadFileStop;
        public event EventHandler<EventArgs> OnSaveFileStart;
        public event EventHandler<EventArgs> OnSaveFileStop;

        public DataHandler(HandleLinksInfo handlingParameters)
        {
            _handlingParameters = handlingParameters;
        }

        /// <summary>
        /// Выбирает все ссылки из строк текста и помещает их в словарь
        /// </summary>
        /// <param name="textLines">Строки текста, из которых будут выбранны ссылки</param>
        /// <returns>Словарь ссылок (номер строки, в которой были найдены сслыки (ключ) - массив найденных ссылок (значение))</returns>
        public static Dictionary<int, string[]> SelectLinksFromText(string[] textLines)
        {
            if (textLines == null)
                throw new ArgumentNullException();

            Dictionary<int, string[]> preparedLinks = new Dictionary<int, string[]>();

            for (int i = 0; i < textLines.Length; i++)
            {
                string[] links = TextParser.LinkFinder.FindLinks(textLines[i]);

                if (links != null)
                    preparedLinks.Add(i, links);
            }

            return preparedLinks;
        }

        /// <summary>
        /// Обрабатывает ссылки в словаре - получает информацию о доступности ресурса в сети
        /// </summary>
        /// <param name="preparedLinks">Словарь ссылок, которые нужно проверить</param>
        /// <returns>Словарь проверенных ссылок с данными об их обработке</returns>
        public static Dictionary<int, LinkInfo[]> HandleLinks(Dictionary<int, string[]> preparedLinks)
        {
            if (preparedLinks == null)
                throw new ArgumentNullException();

            Dictionary<int, LinkInfo[]> handledLinks = new Dictionary<int, LinkInfo[]>(preparedLinks.Count);

            foreach (var preparedLink in preparedLinks)
                handledLinks[preparedLink.Key] = HandleLinkInfos(preparedLink.Value);

            return handledLinks;
        }

        /// <summary>
        /// Обрабатывает массив ссылок - получает информацию о доступности ресурса в сети
        /// </summary>
        /// <param name="preparedLinks">Массив обработанных ссылок (без лишнего текста)</param>
        /// <returns>Массив проверенных ссылок с данными об их обработке</returns>
        private static LinkInfo[] HandleLinkInfos(string[] preparedLinks)
        {
            LinkInfo[] handledLinkInfos = new LinkInfo[preparedLinks.Length];

            for (int i = 0; i < preparedLinks.Length; i++)
                handledLinkInfos[i] = LinkChecker.CheckLink(preparedLinks[i]);

            return handledLinkInfos;
        }

        private void TasksProcessHandleLinks(object obj)
        {
            (Dictionary<int, LinkInfo[]> handledLinks, Dictionary<int, string[]> preparedLinks, Queue<int> unhandledLinkKeys) couple = ((Dictionary<int, LinkInfo[]> handledLinks, Dictionary<int, string[]> preparedLinks, Queue<int> unhandledLinkKeys))obj;

            bool isEnd = false;
            int currentKey = 0;
            Dictionary<int, LinkInfo[]> handledLinks = couple.handledLinks;
            Dictionary<int, string[]> preparedLinks = couple.preparedLinks;
            Queue<int> unhandledLinkKeys = couple.unhandledLinkKeys;

            if (_cancellationTokenSource.Token.IsCancellationRequested)
                return;

            lock (unhandledLinkKeys)
            {
                if (unhandledLinkKeys.Count == 0)
                    isEnd = true;
                else
                {
                    currentKey = unhandledLinkKeys.Dequeue();
                    OnProgress?.Invoke(this, new ProgressEventArgs() { NumberOfRemainingItems = unhandledLinkKeys.Count });
                }
            }

            if (!isEnd)
            {

                string[] currentPreparedLinks = preparedLinks[currentKey];
                LinkInfo[] currentLinkInfos = HandleLinkInfos(currentPreparedLinks);

                lock (handledLinks)
                {
                    handledLinks[currentKey] = currentLinkInfos;
                }
            }
        }

        private Dictionary<int, LinkInfo[]> TasksAsyncHandleLinks(Dictionary<int, string[]> preparedLinks)
        {
            if (preparedLinks == null)
                throw new ArgumentNullException();

            OnStartCheckingWorksheet?.Invoke(this, new StartCheckingWorksheetEventArgs() { NumberOfProcessedItems = preparedLinks.Count });

            Dictionary<int, LinkInfo[]> handledLinks = new Dictionary<int, LinkInfo[]>(preparedLinks.Count);
            Queue<int> unhandledLinkKeys = new Queue<int>(preparedLinks.Keys);

            (Dictionary<int, LinkInfo[]> handledLinks, Dictionary<int, string[]> preparedLinks, Queue<int> unhandledLinkKeys) couple = (handledLinks, preparedLinks, unhandledLinkKeys);

            Parallel.For(0, unhandledLinkKeys.Count, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, i => TasksProcessHandleLinks(couple));

            return handledLinks;
        }

        private List<string[,]> SafelyLoadData(string path, int iRow, int iCol, int width, int height)
        {
            bool fileIsLoaded;
            List<string[,]> worksheetsInputData;

            OnLoadFileStart?.Invoke(this, new EventArgs());

            do
            {
                worksheetsInputData = ExcelWorker.ReadDataInRangeFromWorkbook(path, iRow, iCol, width, height);
                fileIsLoaded = worksheetsInputData != null;

                if (!fileIsLoaded)
                {
                    LoadErrorEventArgs args = new LoadErrorEventArgs() { PathToLoadFile = path };
                    OnLoadError?.Invoke(this, args);

                    while (args.ConfirmRestartLoad == null)
                        Thread.Sleep(1000);

                    fileIsLoaded = !(bool)args.ConfirmRestartLoad;
                }

            } while (!fileIsLoaded);

            OnLoadFileStop?.Invoke(this, new EventArgs());

            return worksheetsInputData;
        }

        private void SafelySaveData(string path, List<string[,]> worksheetsOutputData, int oRow, int oCol)
        {
            bool fileIsSaved;
            string pathToSaveAs = path.Insert(path.LastIndexOf('\\') + 1, OutputFilenamePrefix);

            OnSaveFileStart?.Invoke(this, new EventArgs());

            do
            {
                fileIsSaved = ExcelWorker.WriteDataInRangeFromWorkbook(path, worksheetsOutputData, oRow, oCol, pathToSaveAs);

                if (!fileIsSaved)
                {
                    SaveErrorEventArgs args = new SaveErrorEventArgs() { PathToSaveFile = pathToSaveAs };
                    OnSaveError?.Invoke(this, args);

                    while (args.ConfirmRestartSave == null)
                        Thread.Sleep(1000);

                    fileIsSaved = !(bool)args.ConfirmRestartSave;
                }

            } while (!fileIsSaved);

            OnSaveFileStop?.Invoke(this, new EventArgs());
        }

        public void HandleLinksInWorkbook(string path, int iRow, int iCol, int width, int height, int oRow, int oCol)
        {
            List<string[,]> worksheetsInputData = SafelyLoadData(path, iRow, iCol, width, height);

            if (worksheetsInputData == null)
                return;

            List<string[,]> worksheetsOutputData = new List<string[,]>();

            OnStartCheckingWorkbook?.Invoke(this, new StartCheckingWorkbookEventArgs { Filename = path, NumberOfWorksheets = worksheetsInputData.Count });

            foreach (string[,] inputWorksheetData in worksheetsInputData)
            {
                string[] inputData = new string[inputWorksheetData.GetLength(0)];
                for (int i = 0; i < inputWorksheetData.GetLength(0); i++)
                    inputData[i] = inputWorksheetData[i, 0];

                var selectedLinks = SelectLinksFromText(inputData);
                var handledLinks = TasksAsyncHandleLinks(selectedLinks);

                string[,] outputWorksheetData = new string[inputWorksheetData.GetLength(0), 1];
                foreach (var outputData in handledLinks)
                    outputWorksheetData[outputData.Key, 0] = ReportGenerator.GenerateReportBasedOnLinkInfo(outputData.Value);

                worksheetsOutputData.Add(outputWorksheetData);

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;
            }

            SafelySaveData(path, worksheetsOutputData, oRow, oCol);
        }

        public void HandleLinksInWorkbook(string path)
        {
            HandleLinksInWorkbook(path,
                                  _handlingParameters.InputTableRow,
                                  _handlingParameters.InputTableColumn,
                                  _handlingParameters.Width,
                                  _handlingParameters.Height,
                                  _handlingParameters.OutputTableRow,
                                  _handlingParameters.OutputTableColumn);
        }

        public void HandleLinksInWorkbooks(string[] pathes, int iRow, int iCol, int width, int height, int oRow, int oCol)
        {
            OnStartWorking?.Invoke(this, new StartWorkingEventArgs() { NumberOfBooks = pathes.Length });

            foreach (string path in pathes)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                HandleLinksInWorkbook(path, iRow, iCol, width, height, oRow, oCol);
            }

            OnStopWorking?.Invoke(this, new StopWorkingEventArgs());
        }

        public void HandleLinksInWorkbooks(string[] pathes)
        {
            if (_handlingParameters.ErrorPatterns != null)
                TextParser.MatchFinder.SearchPatterns = _handlingParameters.ErrorPatterns;

            HandleLinksInWorkbooks(pathes,
                                   _handlingParameters.InputTableRow,
                                   _handlingParameters.InputTableColumn,
                                   _handlingParameters.Width,
                                   _handlingParameters.Height,
                                   _handlingParameters.OutputTableRow,
                                   _handlingParameters.OutputTableColumn);
        }

        public void CancelHandleLinks()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}

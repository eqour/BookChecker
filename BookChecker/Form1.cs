using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkCheckerLib;
using System.Windows.Forms;

namespace WFInterface
{
    public partial class Form1 : Form
    {
        private DataHandler _dataHandler = new DataHandler(Program.HandleLinksInfo);

        private int _amountFiles, _amountSheets, _amountItems;
        private int _fileNumber, _sheetNumber, _itemNumber;
        private string _currentFileName = "";
        private bool _checkingCompleted = true, _doFinishWork = false;

        public Form1()
        {
            InitializeComponent();

            _dataHandler.OnStartWorking += DataHandler_StartWorking;
            _dataHandler.OnStopWorking += DataHandler_StopWorking;
            _dataHandler.OnStartCheckingWorkbook += DataHandler_StartCheckingWorkbook;
            _dataHandler.OnStartCheckingWorksheet += DataHandler_StartCheckingWorksheet;
            _dataHandler.OnProgress += DataHandler_Progress;
            _dataHandler.OnLoadError += DataHandler_LoadError;
            _dataHandler.OnSaveError += DataHandler_SaveError;
            _dataHandler.OnLoadFileStart += DataHandler_LoadFileStart;
            _dataHandler.OnLoadFileStop += DataHandler_LoadFileStop;
            _dataHandler.OnSaveFileStart += DataHandler_SaveFileStart;
            _dataHandler.OnSaveFileStop += DataHandler_SaveFileStop;
        }

        private void StartCheckButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.Filter = "Excel files (*.xslx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            DialogResult result = openDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ((Button)sender).Enabled = false;

                Task task = new Task(() => { _dataHandler.HandleLinksInWorkbooks(openDialog.FileNames); });
                task.Start();
            }
        }


        private void DataHandler_StartWorking(object sender, DataHandler.StartWorkingEventArgs e)
        {
            _amountFiles = e.NumberOfBooks;
            _fileNumber = 0;
            _checkingCompleted = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_checkingCompleted == false)
            {
                if (_doFinishWork == true)
                {
                    MessageBox.Show(this, "Ожидайте завершения работы процессов программы.\nПрограмма закроется автоматически.", "Завершение работы", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult result = MessageBox.Show(this, "Вы действительно хотите завершить работу программы?\nПроцесс обработки данных будет завершён, а обработанные данные - сохранены.", "Завершение работы", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        label5.Text = "Ожидание завершения процессов программы";
                        label5.Visible = true;
                        LinksCheckingProgressBar.Style = ProgressBarStyle.Marquee;

                        if (_checkingCompleted == false)
                        {
                            _doFinishWork = true;
                            _dataHandler.CancelHandleLinks();
                        }
                        else
                        {
                            e.Cancel = false;
                            return;
                        }
                    }
                }

                e.Cancel = true;
            }
        }

        private void DataHandler_StopWorking(object sender, DataHandler.StopWorkingEventArgs e)
        {
            _checkingCompleted = true;

            Invoke(new Action(() =>
            {
                StartCheckButton.Enabled = true;
                LinksCheckingProgressBar.Value = 0;
                label1.Text = "Обработка файлов:";
                label2.Text = "Обработка листов:";
                label3.Text = "Обработка элементов:";
                label4.Text = "Текущий файл:";
            }));

            if (_doFinishWork == true)
                Invoke(new Action(() =>
                {
                    Application.Exit();
                }));
        }

        private void DataHandler_StartCheckingWorkbook(object sender, DataHandler.StartCheckingWorkbookEventArgs e)
        {
            _currentFileName = e.Filename;
            _amountSheets = e.NumberOfWorksheets;
            _sheetNumber = 0;
            _fileNumber++;

            Invoke(new Action(() =>
            {
                label1.Text = $"Обработка файлов: {_fileNumber} из {_amountFiles}";
                label4.Text = $"Текущий файл: {_currentFileName}";
            }));
        }

        private void DataHandler_StartCheckingWorksheet(object sender, DataHandler.StartCheckingWorksheetEventArgs e)
        {
            _amountItems = e.NumberOfProcessedItems;
            _itemNumber = 0;
            _sheetNumber++;

            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Value = 0;
                LinksCheckingProgressBar.Minimum = 0;
                LinksCheckingProgressBar.Maximum = e.NumberOfProcessedItems == 0 ? 100 : e.NumberOfProcessedItems;

                label2.Text = $"Обработка листов: {_sheetNumber} из {_amountSheets}";
            }));
        }

        private void DataHandler_Progress(object sender, DataHandler.ProgressEventArgs e)
        {
            _itemNumber = _amountItems - e.NumberOfRemainingItems;

            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Value = _itemNumber;

                label3.Text = $"Обработка элементов: {_itemNumber} из {_amountItems}";
            }));
        }

        private void DataHandler_LoadError(object sender, DataHandler.LoadErrorEventArgs e)
        {
            Invoke(new Action(() =>
            {
                DialogResult result = MessageBox.Show(this, $"Не удалось загрузить данные из файла {e.PathToLoadFile}.\nВозможно, он был удалён или перенесён во время работы программы.\nПовторить открытие файла?", "Ошибка загрузки данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    e.ConfirmRestartLoad = true;
                }
                else
                {
                    e.ConfirmRestartLoad = false;
                }
            }));
        }

        private void DataHandler_SaveError(object sender, DataHandler.SaveErrorEventArgs e)
        {

            Invoke(new Action(() =>
            {
                DialogResult result = MessageBox.Show(this, $"Не удалось сохранить данные в файл {e.PathToSaveFile}, так как он используется другой программой.\nСохранить файл повторно?", "Ошибка сохранения данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    e.ConfirmRestartSave = true;
                }
                else
                {
                    e.ConfirmRestartSave = false;
                }
            }));
        }

        private void DataHandler_LoadFileStart(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Style = ProgressBarStyle.Marquee;
                label5.Text = "Загрузка информации из файла";
                label5.Visible = true;
            }));
        }

        private void DataHandler_LoadFileStop(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Style = ProgressBarStyle.Blocks;
                label5.Visible = false;
            }));
        }

        private void DataHandler_SaveFileStart(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Style = ProgressBarStyle.Marquee;
                label5.Text = "Сохранение информации в файл";
                label5.Visible = true;
            }));
        }

        private void DataHandler_SaveFileStop(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                LinksCheckingProgressBar.Style = ProgressBarStyle.Blocks;
                label5.Visible = false;
            }));
        }
    }
}

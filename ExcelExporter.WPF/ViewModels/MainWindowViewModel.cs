using System.Windows.Input;
using DataLoaderLibrary.Services;
using ExportToExcelLibrary.Services;
using WpfHelper.Binding;
using WpfHelper.Commands;

namespace ExcelExporter.WPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

        /* Показывает осуществляется ли в данный момент выгрузка в Excel */
        private bool _isLoad { get; set; }
        public bool IsLoad
        {
            get => _isLoad;
            set { _isLoad = value; RaiseOnPropertyChanged(); }
        }

        /* Показывает осуществляется ли в данный момент подключение к БД */
        private bool _isConnection { get; set; }
        public bool IsConnection
        {
            get => _isConnection;
            set { _isConnection = value; RaiseOnPropertyChanged(); }
        }

        /* SQL запрос, вводимый пользователем */
        private string _sqlQuery { get; set; }
        public string SqlQuery
        {
            get => _sqlQuery;
            set => _sqlQuery = value;
        }

        private ICommand _loadDataCommand { get; set; }
        public ICommand LoadDataCommand
        {
            get => _loadDataCommand;
            set => _loadDataCommand = value;
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            LoadDataCommand = new DelegateCommand(param =>
            {
                LoadData();
            });
        }

        #endregion

        #region Methods

        private void LoadData()
        {

        }

        #endregion
    }
}

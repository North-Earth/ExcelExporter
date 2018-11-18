using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DataLoaderLibrary.Services;
using ExcelExporter.WPF.Models;
using WpfHelper.Binding;
using WpfHelper.Commands;

namespace ExcelExporter.WPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Threads
        private Thread ConnectToServer;

        #endregion

        #region Fields

        /* Показывает осуществляется ли в данный момент выгрузка в Excel */
        private bool _isLoad { get; set; }
        public bool IsLoad
        {
            get => _isLoad;
            set { _isLoad = value; RaiseOnPropertyChanged(); }
        }

        /* Показывает осуществляется ли в данный момент подключение к SQL серверу */
        private bool _isConnection { get; set; }
        public bool IsConnection
        {
            get => _isConnection;
            set { _isConnection = value; RaiseOnPropertyChanged(); }
        }

        /* Показывает есть ли в данный момент подключение к SQL серверу */
        private bool _isConnected { get; set; }
        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; RaiseOnPropertyChanged(); }
        }

        /* Хранит коллекцию серверов */
        private List<ServerModel> _serversCollection { get; set; }
        public List<ServerModel> ServersCollection
        {
            get => _serversCollection;
            set { _serversCollection = value; RaiseOnPropertyChanged(); }
        }

        /* Хранит текущую выбранную модель сервера */
        private ServerModel _selectedServerModel { get; set; }
        public ServerModel SelectedServerModel
        {
            get => _selectedServerModel;
            set
            {
                _selectedServerModel = value;
                RaiseOnPropertyChanged();

                if (ConnectToServer != null)
                {
                    ConnectToServer.Abort();
                    ConnectToServer.Join(500);
                }

                ConnectToServer = new Thread(CheckServer);
                ConnectToServer.Start();
            }
        }

        /* Хранит список с названиями баз данных */
        private List<string> _basesCollection { get; set; }
        public List<string> BasesCollection
        {
            get => _basesCollection;
            set { _basesCollection = value; RaiseOnPropertyChanged(); }
        }

        /* Хранит текущее выбранное название БД */
        private string _selectedBase { get; set; }
        public string SelectedBase
        {
            get => _selectedBase;
            set { _selectedBase = value; RaiseOnPropertyChanged(); }
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
            LoadDataCommand = new DelegateCommand(param => { LoadData(); }, param => IsConnected);

            ServersCollection = new List<ServerModel>
            {
                new ServerModel { Name = "EARTH", Value = @"EARTH\EARTH" },
            };
        }

        #endregion

        #region Methods

        private void CheckServer()
        {
            bool isSuccessfully = false;

            try
            {
                IsConnection = true;
                var loader = new LoaderService<string>(serverName: SelectedServerModel.Value) as ILoaderService<string>;

                BasesCollection = loader
                    .GetQueryResultsForType("SELECT sdb.name FROM master.dbo.sysdatabases AS sdb")
                    .ToList();
                IsConnected = true;
                isSuccessfully = true;
            }
            catch (ThreadAbortException ex) { }
            catch (SqlException ex) { MessageBox.Show(ex.Message, "Ошибка подключения к серверу"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                /* Если не выполнено с успехом - отмена изменений */
                if (!isSuccessfully)
                {
                    BasesCollection = null;
                    IsConnected = false;
                }

                IsConnection = false;
            }
        }

        private void LoadData()
        {
            MessageBox.Show($"{SelectedServerModel.Name} | {SelectedServerModel.Value} | {SelectedBase}");
        }

        #endregion
    }
}

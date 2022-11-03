using GalaSoft.MvvmLight;
using SuperCom.Config;
using SuperCom.Entity;
using SuperCom.Log;
using SuperUtils.Common;
using SuperUtils.Framework.ORM.Mapper;
using SuperUtils.WPF.VisualTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace SuperCom.ViewModel
{

    public class VieModel_AdvancedSend : ViewModelBase
    {

        private static SqliteMapper<AdvancedSend> mapper { get; set; }

        private ObservableCollection<AdvancedSend> _Projects;
        public ObservableCollection<AdvancedSend> Projects
        {
            get { return _Projects; }
            set { _Projects = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<SendCommand> _SendCommands;

        public ObservableCollection<SendCommand> SendCommands
        {
            get { return _SendCommands; }
            set
            {
                _SendCommands = value;
                RaisePropertyChanged();
            }
        }
        private long _CurrentProjectID;

        public long CurrentProjectID
        {
            get { return _CurrentProjectID; }
            set
            {
                _CurrentProjectID = value;
                RaisePropertyChanged();
            }
        }





        private bool _ShowCurrentSendCommand;

        public bool ShowCurrentSendCommand
        {
            get { return _ShowCurrentSendCommand; }
            set
            {
                _ShowCurrentSendCommand = value;
                RaisePropertyChanged();
            }
        }

        private bool _RunningCommands;
        public bool RunningCommands
        {
            get { return _RunningCommands; }
            set
            {
                _RunningCommands = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<SideComPort> _SideComPorts;
        public ObservableCollection<SideComPort> SideComPorts
        {
            get { return _SideComPorts; }
            set { _SideComPorts = value; RaisePropertyChanged(); }
        }

        public MainWindow Main { get; set; }

        public VieModel_AdvancedSend()
        {
            Init();
        }

        static VieModel_AdvancedSend()
        {
            mapper = new SqliteMapper<AdvancedSend>(ConfigManager.SQLITE_DATA_PATH);
        }


        private void Init()
        {
            Projects = new ObservableCollection<AdvancedSend>();
            SendCommands = new ObservableCollection<SendCommand>();
            // �����ݿ��ж�ȡ
            if (mapper != null)
            {
                List<AdvancedSend> advancedSends = mapper.SelectList();
                foreach (var item in advancedSends)
                {
                    Projects.Add(item);
                }
            }
            foreach (Window window in App.Current.Windows)
            {
                if (window.Name.Equals("mainWindow"))
                {
                    Main = (MainWindow)window;
                    break;
                }
            }
            LoadSideComports();
        }

        private void LoadSideComports()
        {
            SideComPorts = new ObservableCollection<SideComPort>();
            foreach (var item in Main?.vieModel.SideComPorts)
            {
                SideComPorts.Add(item);
            }
        }

        public void SaveProjects()
        {

        }

        public void UpdateProject(AdvancedSend send)
        {
            int count = mapper.UpdateById(send);
            if (count <= 0)
            {
                System.Console.WriteLine($"���� {send.ProjectName} ʧ��");
            }
        }

        public void RenameProject(AdvancedSend send)
        {
            bool result = mapper.UpdateFieldById("ProjectName", send.ProjectName, send.ProjectID);
            if (!result)
            {
                System.Console.WriteLine($"���� {send.ProjectName} ʧ��");
            }
        }

        public void DeleteProject(AdvancedSend send)
        {
            int count = mapper.DeleteById(send.ProjectID);
            if (count <= 0)
            {
                System.Console.WriteLine($"ɾ�� {send.ProjectName} ʧ��");
            }
            else
            {
                ShowCurrentSendCommand = false;
            }
        }

        public void AddProject(string projectName)
        {
            if (string.IsNullOrEmpty(projectName)) return;
            AdvancedSend send = new AdvancedSend();
            send.ProjectName = projectName;
            bool success = mapper.Insert(send);
            if (success)
                Projects.Add(send);
        }

        public void SetCurrentSendCommands()
        {

        }

    }
}
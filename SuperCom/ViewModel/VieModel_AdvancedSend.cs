using GalaSoft.MvvmLight;
using SuperCom.Config;
using SuperCom.Entity;
using SuperCom.Log;
using SuperUtils.Common;
using SuperUtils.Framework.ORM.Mapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;

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
            //for (int i = 0; i < 10; i++)
            //{
            //    ProjectNames.Add(new AdvancedSend(i, $"��Ŀ {i}"));
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    SendCommand sendCommand = new SendCommand();
            //    sendCommand.Order = i;
            //    sendCommand.Delay = i * 1000;
            //    sendCommand.Command = $"AT^PHYNUM={i}";
            //    SendCommands.Add(sendCommand);
            //}

            // �����ݿ��ж�ȡ
            if (mapper != null)
            {
                List<AdvancedSend> advancedSends = mapper.SelectList();
                foreach (var item in advancedSends)
                {
                    Projects.Add(item);
                }
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

        public void DeleteProject(AdvancedSend send)
        {
            int count = mapper.DeleteById(send.ProjectID);
            if (count <= 0)
            {
                System.Console.WriteLine($"ɾ�� {send.ProjectName} ʧ��");
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
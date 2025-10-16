using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Supabase;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    class InfoPageViewModel : INotifyPropertyChanged
    {
        private readonly IDbService _db;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand UpdateInfoCommand { get; }

        private ObservableCollection<Models.Contact> _contacts = new ObservableCollection<Models.Contact>();

        private ObservableCollection<Models.Info> _infoList = new ObservableCollection<Models.Info>();
        public ObservableCollection<Models.Contact> Contacts { get { return _contacts; } 
            set
            {
                _contacts = value;
                OnPropertyChanged();
            } 
        }
        private string _info;
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<Models.Info> InfoList
        {
            get { return _infoList; }
            set
            {
                _infoList = value;
                OnPropertyChanged();
            }
        }

        public InfoPageViewModel(IDbService dbService)
        {
            _db = dbService;
            _ = FillContacts();
            _ = FillInfoAsync();
            UpdateInfoCommand = new Command(async () => await UpdateInfoDBAsync());
            //SetInfoProperty();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Models.Contact>> GetAllContacts()
        {
            var listContacts = await _db.GetAllContactsAsync();
            return listContacts;
        }

        public async Task FillContacts()
        {
            var getContacts = await GetAllContacts();
            Contacts.Clear();
            foreach(var x in getContacts)
            {
                Contacts.Add(x);
            }
        }

        public async Task<List<Models.Info>> FillInfoAsync()
        {
            var listInfo = await _db.GetAllInfoAsync();
            InfoList.Clear();
            foreach (var x in listInfo)
            {
                InfoList.Add(x);
            }
            return listInfo;
        }
        //private async Task<string> GetThisInfo()
        //{
        //    var data = await _db.InfoCollection().Find(Builders<Models.Info>.Filter.Empty).ToListAsync();
        //    var thisData = data.FirstOrDefault();
        //    var infoStringData = thisData.InfoString;
        //    return infoStringData;
        //}
        private async Task UpdateInfoDBAsync()
        {
            Models.Info info = new Models.Info()
            {
                Id = Guid.NewGuid(),
                InfoString = Info,
                CreatedAt = DateTime.Now
            };
            await _db.UpdateInfoAsync(info, info.Id);
            //await _db.InfoCollection().ReplaceOneAsync(filter: Builders<Models.Info>.Filter.Eq(x => x.Id, "unique_id"),
            //    replacement: info, options: new ReplaceOptions { IsUpsert = true });

        }

        private async void SetInfoProperty()
        {
            var allInfoStringData = await _db.GetAllInfoAsync();
            string infoStringData = allInfoStringData.FirstOrDefault().InfoString;
            Info = infoStringData;
        }
    }
}

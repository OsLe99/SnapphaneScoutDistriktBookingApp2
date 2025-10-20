using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services;
using ScoutContact = SnapphaneScoutDistriktBookingApp.Models.Contact;
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

        private ObservableCollection<ScoutContact> _contacts = new ObservableCollection<ScoutContact>();

        private ObservableCollection<Info> _infoList = new ObservableCollection<Info>();
        public ObservableCollection<ScoutContact> Contacts { get { return _contacts; } 
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

        public ObservableCollection<Info> InfoList
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

        public async Task<List<ScoutContact>> GetAllContacts()
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

        public async Task<List<Info>> FillInfoAsync()
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
            Info info = new()
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

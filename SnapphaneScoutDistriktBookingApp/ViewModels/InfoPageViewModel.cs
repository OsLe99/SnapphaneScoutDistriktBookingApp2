using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    class InfoPageViewModel : INotifyPropertyChanged
    {
        private readonly IDbService _db = new DbService();
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand UpdateInfoCommand { get; }

        private ObservableCollection<Models.Contact> _contacts = new ObservableCollection<Models.Contact>();
        public ObservableCollection<Models.Contact> Contacts { get { return _contacts; } 
            set
            {
                _contacts = value;
                OnPropertyChanged();
            } 
        }
        private string _info;
        public string Info { get { return _info; }
            set 
            {
                _info = value;
                OnPropertyChanged();

            }
        }
        public InfoPageViewModel()
        {
            _ = FillContacts();
            UpdateInfoCommand = new Command(async () => await UpdateInfoDBAsync());
            SetInfoProperty();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Models.Contact>> GetAllContacts()
        {
            var listContacts = await _db.ContactCollection().Find(Builders<Models.Contact>.Filter.Empty).ToListAsync();
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
        private async Task<string> GetThisInfo()
        {
            var data = await _db.InfoCollection().Find(Builders<Models.Info>.Filter.Empty).ToListAsync();
            var thisData = data.FirstOrDefault();
            var infoStringData = thisData.InfoString;
            return infoStringData;
        }
        private async Task UpdateInfoDBAsync()
        {
            Models.Info info = new Models.Info()
            {
                Id = "unique_id",
                InfoString = Info
            };
            await _db.InfoCollection().ReplaceOneAsync(filter: Builders<Models.Info>.Filter.Eq(x => x.Id, "unique_id"),
                replacement: info, options: new ReplaceOptions { IsUpsert = true });

        }
        private async void SetInfoProperty()
        {
            var allInfoStringData = await _db.InfoCollection().Find(Builders<Models.Info>.Filter.Empty).ToListAsync();
            string infoStringData = allInfoStringData.FirstOrDefault().InfoString;
            Info = infoStringData;

            
        }
    }
}

using Newtonsoft.Json;
using Richasy.Helper.UWP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFaces.NET.Models;
using UIFaces.NET.Services;
using Windows.ApplicationModel;

namespace CompositionAnimationDemo
{
    public class PersonViewModel
    {
        private Instance _instance;
        public ObservableCollection<Person> PersonCollection;

        private UIFacesService _service;

        public static PersonViewModel Current;

        private const string LocalFileName = "LocalFile.json";

        public PersonViewModel()
        {
            PersonCollection = new ObservableCollection<Person>();
            Current = this;
            _instance = new Instance(nameof(Package.Current.DisplayName));
            _service = new UIFacesService("E5DDF433-C5D540F3-A3C92C7B-2DB79881");
        }

        public async Task InitPersonCollection()
        {
            if (!await GetDataFromLocal())
                await GetDataFromWeb();
        }

        private async Task GetDataFromWeb()
        {
            var person = await _service.GetFaces(20);
            person.ForEach(p => PersonCollection.Add(p));
            await _instance.IO.SetLocalDataAsync(LocalFileName, JsonConvert.SerializeObject(person));
        }

        private async Task<bool> GetDataFromLocal()
        {
            var data = await _instance.IO.GetLocalDataAsync<List<Person>>(LocalFileName);
            if (data.Count > 0)
            {
                data.ForEach(p => PersonCollection.Add(p));
            }
            return data.Count > 0;
        }
    }
}

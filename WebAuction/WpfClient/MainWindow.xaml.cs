using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClient.Models;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int id { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            using (WebClient webClient = new WebClient())
            {
                //var app = App.Current as IGetConfiguration;
                //var serverUrl = app.Configuration.GetSection("ServerUrl").Value;
                //webClient.DownloadDataCompleted += AsyncDownloadDataCompleted;
                Uri uri = new Uri("http://localhost:1782/api/Lot/search");
                webClient.DownloadDataAsync(uri);
            }
        }
        public void AsyncDownloadDataCompleted(Object sender, DownloadDataCompletedEventArgs e)
        {
            string result = Encoding.Default.GetString(e.Result);
            var lots = JsonConvert.DeserializeObject<List<LotVM_Client>>(result);
            dgSimple.ItemsSource = lots;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            AddLotWindow add = new AddLotWindow();
            add.ShowDialog();
        }

        

        private void btnEdt_Click(object sender, RoutedEventArgs e)
        {
            LotVM_Client model = dgSimple.SelectedItem as LotVM_Client;
            id = model.Id;
            Edt_Window edit = new Edt_Window(id);
            edit.ShowDialog();
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
       private async  void btnDell_Click(object sender, RoutedEventArgs e)
        {
            //await Task.Run(() => Dell_Carr());

            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    LotVM_Client model = dgSimple.SelectedItem as LotVM_Client;
                    //  Ініціалізація обєкта, який відправлятиме запит на веб-сервіс
                    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp($"http://localhost:1782/api/Lot/del");
                    //  Встановлення методу відправки данних
                    request.Method = "DELETE";
                    //  Встановлення типу відправки данних
                    request.ContentType = "application/json";
                    //  Ініціалізація обєкту, який відправляє дані на веб-сервіс
                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {
                        //  Відправка данних
                        sw.Write(JsonConvert.SerializeObject(model.Id));
                    }
                    //  Отримання результату запиту
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //  Зчитування результату запиту
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd();
                    }

                    //Переініціалізація ДатаГріда
                    Dispatcher.Invoke(async () =>
                    {
                        List<LotVM_Client> cars = new List<LotVM_Client>();
                        //cars.AddRange(models.Where(x => x.IsNew).ToList());
                        cars.AddRange(await GetCars());
                        //models = cars;
                        this.dgSimple.ItemsSource = cars;
                    });

                });
            });

        }
       
        private async Task<List<LotVM_Client>> GetCars()
        {
            //  Отримання данних з БД
            return await Task.Run(() => {
                try
                {

                    //  Формування нової колекції яка повертатиметься
                    List<LotVM_Client> models_ = new List<LotVM_Client>();
                    //  Створення обєкта WebClient, який отримує запити з веб-сервісу
                    WebClient client = new WebClient();
                    //  Десеріалізація з формата json у тип List<CarModel>
                    models_ = JsonConvert.DeserializeObject<List<LotVM_Client>>(client.DownloadString("http://localhost:1782/api/Lot/search"));
                    //  Повернення колекції
                    return models_;
                }
                catch
                {
                    return null;
                }
            });
        }
    }
}

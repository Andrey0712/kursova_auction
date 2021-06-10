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
using UiHelper.Constants;
using WpfClient.Models;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int id { get; set; }

        public static string token;
        public MainWindow()
        {
            InitializeComponent();
            
            using (WebClient webClient = new WebClient())
            {
                
                webClient.DownloadDataCompleted += AsyncDownloadDataCompleted;
                Uri uri = new Uri(UriConstant.Get);
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
           
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    LotVM_Client model = dgSimple.SelectedItem as LotVM_Client;
                    
                    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(UriConstant.Del);
                        request.Method = "DELETE";
                        request.ContentType = "application/json";
                    request.PreAuthenticate = true;//тест при авторизации
                    request.Headers.Add("Authorization", $"Bearer {token}");

                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {

                        sw.Write(JsonConvert.SerializeObject(model.Id));
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd();
                    }

                    Dispatcher.Invoke(async () =>
                    {
                        List<LotVM_Client> lot = new List<LotVM_Client>();
                        lot.AddRange(await GetLots());
                        dgSimple.ItemsSource = lot;
                    });

                });
            });

        }
       
        private async Task<List<LotVM_Client>> GetLots()
        {
            //  Отримання данних з БД
            return await Task.Run(() => {
                try
                {
                    List<LotVM_Client> getLot = new List<LotVM_Client>();
                       WebClient client = new WebClient();
                    getLot = JsonConvert.DeserializeObject<List<LotVM_Client>>(client.DownloadString(UriConstant.Get));
                             return getLot;
                }
                catch
                {
                    return null;
                }
            });
        }




        private  void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
                    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(UriConstant.Account);
                      request.Method = "POST";
                          request.ContentType = "application/json";
                    
                    string json = JsonConvert.SerializeObject(new
                    {
                        Email = tbUser.Text,
                        Password = tbPassword.Text
                    });
                    byte[] bytes = Encoding.UTF8.GetBytes(json);

                    using (Stream stream = request.GetRequestStreamAsync().Result)
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    try
                    {
                        var response = request.GetResponseAsync().Result;
                        using (var stream = new StreamReader(response.GetResponseStream()))
                        {
                           var tok = JsonConvert.DeserializeObject<Get_Token>(stream.ReadToEnd());
                             token = tok.Token;
                             MessageBox.Show("Авторизация прошла успешно");
                             return;
                        }
                    }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }

        private async void btnRate_Click(object sender, RoutedEventArgs e)
        {
            // _=Rate();
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    LotVM_Client model = dgSimple.SelectedItem as LotVM_Client;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(UriConstant.Rate);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
                    request.PreAuthenticate = true;//тест при авторизации
                    request.Headers.Add("Authorization", $"Bearer {token}");
                    string json = JsonConvert.SerializeObject(new
                    {
                        Id = model.Id,
                        Prise = int.Parse(tbNewPrise.Text)


                    });
                    if(int.Parse(tbNewPrise.Text) > model.Prise)
                    {
                      byte[] byteArray = Encoding.UTF8.GetBytes(json);

                        using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(byteArray, 0, byteArray.Length);
                    }


                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd();
                    }
                    }
                    else
                    {
                        MessageBox.Show("Ваша ставка не принята.");
                    }

                    Dispatcher.Invoke(async () =>
                    {
                        List<LotVM_Client> lot = new List<LotVM_Client>();
                        lot.AddRange(await GetLots());
                        dgSimple.ItemsSource = lot;
                    });

                });
            });
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadDataCompleted += AsyncDownloadDataCompleted;
                Uri uri = new Uri(UriConstant.Get);
                webClient.DownloadDataAsync(uri);
            }

        }

        public async Task<bool> Rate()
        {
            //if (!string.IsNullOrEmpty(New_FileName))
            //{
            //    var extension = System.IO.Path.GetExtension(New_FileName);
            //    var imageName = System.IO.Path.GetRandomFileName() + extension;
            //    var dir = Directory.GetCurrentDirectory();
            //    var saveDir = System.IO.Path.Combine(dir, "foto");
            //    if (!Directory.Exists(saveDir))
            //        Directory.CreateDirectory(saveDir);

            //    var fileSave = System.IO.Path.Combine(saveDir, imageName);
            //    File.Copy(New_FileName, fileSave);
            //    foto = fileSave;
            //}
            LotVM_Client model = dgSimple.SelectedItem as LotVM_Client;
            // отправляем запрос по вебу
            WebRequest request = WebRequest.Create(UriConstant.Rate);
            // метод
            request.Method = "PUT";

            // тип даных
            request.ContentType = "application/json";
            request.PreAuthenticate = true;//тест при авторизации
            request.Headers.Add("Authorization", $"Bearer {MainWindow.token}");
            // формируется запрос и отправляються в масив с кодировкой
            
            string json = JsonConvert.SerializeObject(new
            {
                //Id = model.Id,
                Id = model.Id,
                //Name = model.Name,
                //End = model.End,
                //Description = model.Description,
                Prise = int.Parse(tbNewPrise.Text),
                //Image=model.Image
                

            });

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }
            try
            {
                await request.GetResponseAsync();
                return true;
            }

            catch (WebException e)
            {
                using WebResponse response = e.Response;
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                //MessageBox.Show("Error code: " + httpResponse.StatusCode);
                using Stream data = response.GetResponseStream();
                using var reader = new StreamReader(data);

                string text = reader.ReadToEnd();
                var errors = JsonConvert.DeserializeObject<AddLotValid>(text);
                //MessageBox.Show(text);

               
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); return false;
            }

        }
    }
}

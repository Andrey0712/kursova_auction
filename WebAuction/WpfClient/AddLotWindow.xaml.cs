using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using UiHelper;
using UiHelper.Constants;
using WpfClient.Models;
using Path = System.IO.Path;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для AddLotWindow.xaml
    /// </summary>
    public partial class AddLotWindow : Window
    {
        private string file_selected = string.Empty;
        public string file_name { get; set; }
        public static string New_FileName { get; set; }
        public AddLotWindow()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                New_FileName = openFileDialog.FileName;
            }
        }

        //Кнопка зберігання
        //private void btnSaveChangs_Click(object sender, RoutedEventArgs e)
        //{
        //    _ = PostRequest();
            
        //    //Close();
        //}

        private async void btnSaveChangs_Click(object sender, RoutedEventArgs e)
        {

            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    
                    HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(UriConstant.Add);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.PreAuthenticate = true;//тест при авторизации
                    request.Headers.Add("Authorization", $"Bearer {MainWindow.token}");

                    string base64 = ImageHelper.ImageConvertToBase64(New_FileName);
                    string json = JsonConvert.SerializeObject(new
                    {
                        Name = tbName.Text.ToString(),
                        End = int.Parse(tbEnd.Text),
                        Description = tbDescription.Text.ToString(),
                        Prise = int.Parse(tbPrise.Text),
                        Image = base64
                        
                    });
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    using (Stream stream = request.GetRequestStream())
                    {

                        stream.Write(bytes, 0, bytes.Length);
                    }
                                        
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            var result = sr.ReadToEnd();
                        }
                        //request.GetResponseAsync();

                        //WebResponse response = request.GetResponse();
                        //using (Stream stream = response.GetResponseStream())
                        //{
                        //    StreamReader reader = new StreamReader(stream);
                        //    string responseFromServer = reader.ReadToEnd();
                           
                        //}

                        //response.Close();
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

                        if (errors.Errors.Name != null)
                            lbName.Content = errors.Errors.Name[0].ToString();

                        if (errors.Errors.Prise != null)
                            lbPrise.Content = errors.Errors.Prise[0].ToString();

                        if (errors.Errors.End != null)
                            lbEnd.Content = errors.Errors.End[0].ToString();

                        if (errors.Errors.Description != null)
                            lbDescription.Content = errors.Errors.Description[0].ToString();

                        return false;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                        return false;
                    }

                });
            });

        }

        //public async Task<bool> PostRequest()
        //{
            
        //    WebRequest request = WebRequest.Create(UriConstant.Add);
        //    {
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.PreAuthenticate = true;//тест при авторизации
        //        request.Headers.Add("Authorization", $"Bearer {MainWindow.token}");
                
        //    };
        //    string base64 = ImageHelper.ImageConvertToBase64(New_FileName);
        //    string json = JsonConvert.SerializeObject(new
        //    {
        //        Name = tbName.Text.ToString(),
        //        End = int.Parse(tbEnd.Text),
        //        Description = tbDescription.Text.ToString(),
        //        Prise = int.Parse(tbPrise.Text),
        //        Image = base64
        //        //Image= file_name
        //    });
        //    byte[] bytes = Encoding.UTF8.GetBytes(json);

        //    using (Stream stream = await request.GetRequestStreamAsync())
        //    {
        //        stream.Write(bytes, 0, bytes.Length);
        //    }
        //    try
        //    {
        //        await request.GetResponseAsync();
        //        return true;
        //    }
        //    catch (WebException e)
        //    {

        //        using WebResponse response = e.Response;
        //        HttpWebResponse httpResponse = (HttpWebResponse)response;
        //        //MessageBox.Show("Error code: " + httpResponse.StatusCode);
        //        using Stream data = response.GetResponseStream();
        //        using var reader = new StreamReader(data);

        //        string text = reader.ReadToEnd();
        //        var errors = JsonConvert.DeserializeObject<AddLotValid>(text);
        //        //MessageBox.Show(text);

        //        if (errors.Errors.Name != null)
        //            lbName.Content = errors.Errors.Name[0].ToString();

        //        if (errors.Errors.Prise != null)
        //            lbPrise.Content = errors.Errors.Prise[0].ToString();

        //        if (errors.Errors.End != null)
        //            lbEnd.Content = errors.Errors.End[0].ToString();

        //        if (errors.Errors.Description != null)
        //            lbDescription.Content = errors.Errors.Description[0].ToString();

        //        return false;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //        return false;
        //    }
            
        //}
    }
}

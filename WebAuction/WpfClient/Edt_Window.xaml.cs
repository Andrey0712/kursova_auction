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
using UiHelper.Constants;
using WpfClient.Models;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для Edt_Window.xaml
    /// </summary>
    public partial class Edt_Window : Window
    {
        public static string New_FileName { get; set; }
        public string foto { get; set; }
        public long _id { get; set; }
        public Edt_Window(int id)
        {
            InitializeComponent();
            _id = id;
        }
        private void btnSaveChangs_Click(object sender, RoutedEventArgs e)
        {
            _ = Edit();
        }
        public async Task<bool> Edit()
        {
            if (!string.IsNullOrEmpty(New_FileName))
            {
                var extension = System.IO.Path.GetExtension(New_FileName);
                var imageName = System.IO.Path.GetRandomFileName() + extension;
                var dir = Directory.GetCurrentDirectory();
                var saveDir = System.IO.Path.Combine(dir, "foto");
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);

                var fileSave = System.IO.Path.Combine(saveDir, imageName);
                File.Copy(New_FileName, fileSave);
                foto = fileSave;
            }
            
            // отправляем запрос по вебу
            WebRequest request = WebRequest.Create(UriConstant.Edit);
            // метод
            request.Method = "PUT";

            // тип даных
            request.ContentType = "application/json";
            // формируется запрос и отправляються в масив с кодировкой

            string json = JsonConvert.SerializeObject(new
            {
                Id = _id,
                Name = tbName.Text.ToString(),
                End = int.Parse(tbEnd.Text),
                Description = tbDescription.Text.ToString(),
                Prise = int.Parse(tbPrise.Text),
                Image = foto

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

                if (errors.Errors.Name != null)
                    lbName.Content = errors.Errors.Name[0].ToString();

                if (errors.Errors.Prise != null)
                    lbPrise.Content = errors.Errors.Prise[0].ToString();

                if (errors.Errors.End != null)
                    lbEnd.Content = errors.Errors.End[0].ToString();

                if (errors.Errors.Description != null)
                    lbDescription.Content = errors.Errors.Description[0].ToString();
                return false;
                //using WebResponse response = e.Response;
                //HttpWebResponse httpResponse = (HttpWebResponse)response;
                //MessageBox.Show("Error code: " + httpResponse.StatusCode);
                //using Stream data = response.GetResponseStream();
                //using var reader = new StreamReader(data);

                //string text = reader.ReadToEnd();
                //var errors = JsonConvert.DeserializeObject<AddLotValid>(text);
                //MessageBox.Show(text);

                //if (errors.Errors.Name != null)
                //    lbName.Content = errors.Errors.Name[0].ToString();

                //if (errors.Errors.Prise != null)
                //    lbPrise.Content = errors.Errors.Prise[0].ToString();

                //if (errors.Errors.End != null)
                //    lbEnd.Content = errors.Errors.End[0].ToString();

                //if (errors.Errors.Description != null)
                //    lbDescription.Content = errors.Errors.Description[0].ToString();

                //return false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); return false;
            }

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
    }
}

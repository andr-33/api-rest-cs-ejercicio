using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Net.Http.Headers;

namespace FormContact
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonAddContact_ClickAsync(object sender, EventArgs e)
        {
            try {
                string urlPost = "https://localhost:44367/contact/add";

                int idForm = int.Parse(textBoxId.Text);
                string nameForm = textBoxName.Text;
                string phoneNumberForm = textBoxPhoneNumber.Text;

                var newContact = new Contact {
                    Id = idForm,
                    Name = nameForm,
                    PhoneNumber = phoneNumberForm,
                };

                string jsonBody;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Contact));
                    serializer.WriteObject(memoryStream, newContact);
                    jsonBody = Encoding.UTF8.GetString(memoryStream.ToArray());
                }

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var req = new HttpRequestMessage(HttpMethod.Post, urlPost);
                    req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var res = await client.SendAsync(req);

                    if (res.IsSuccessStatusCode)
                    {
                       
                        MessageBox.Show("Contacto creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        
                        MessageBox.Show($"Error: {res.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonGetContacts_Click(object sender, EventArgs e)
        {
            try
            {
                string apiUrl = "https://localhost:44367/contact/getall";

           
                List<Contact> contacts = await Task.Run(() => GetContactsAsync(apiUrl));

         
                dataGridView1.DataSource = contacts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<Contact>> GetContactsAsync(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        // Deserializar la respuesta a una lista de Contact
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Contact>));
                        List<Contact> contacts = (List<Contact>)serializer.ReadObject(responseStream);
                        return contacts;
                    }
                }
                else
                {
                    // Manejar el error si la solicitud no fue exitosa
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
    }
}

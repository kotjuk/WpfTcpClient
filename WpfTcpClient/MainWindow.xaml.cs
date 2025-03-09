using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace WpfTcpClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string message = TxtInput.Text;

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Введите сообщение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string response = SendMessageToServer(message);
            LblResponse.Content = $"Ответ: {response}";
        }

        private string SendMessageToServer(string message)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("127.0.0.1", 12345);
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                stream.Flush();

                data = new byte[1000];
                int bytes = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytes);

                return response.Trim();
            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
}

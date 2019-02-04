using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace StarWarsQuote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var isDark = DarkSideRB.IsChecked != null && (bool) DarkSideRB.IsChecked;
            SWQuoteBox.Text= RequestHandler.GetQuote(isDark);
        }
    }

    public class RequestHandler : WebRequest
    {
        public static string GetQuote(bool side)
        {
            var forceSideQuery = "";
            string Url = "http://swquotesapi.digitaljedi.dk/api/SWQuote/RandomStarWarsQuoteFromFaction";

            forceSideQuery = side ? "/1" : "/0";

            var request = (HttpWebRequest)Create(Url + forceSideQuery);

            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            string content = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                }
            }

            var result = JObject.Parse(content).SelectToken("starWarsQuote").ToString(); ;
            return result;
        }
    }
}

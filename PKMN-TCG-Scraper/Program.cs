using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PKMN_TCG_Scraper
{
    public class Program
    {
        static int maxPage = 679;
        static string baseUrl = "https://pkmncards.com/page/";
        static string urlParameters = "/?s&display=full&sort=date&order";

        static void Main(string[] args)
        {
            System.IO.Directory.CreateDirectory("images");

            int itemsDownloaded = 0;

            for(int pageIndex = 36; pageIndex < maxPage; pageIndex++)
            {
                var url = baseUrl + pageIndex + urlParameters;
                var web = new HtmlWeb();
                var doc = web.Load(url);

                // get div entry-content
                // get a card-image-link
                // get a card-title-link

                ////div[contains(@class,'myclass')]
                Console.WriteLine("--- PAGE: " + pageIndex);
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//article[contains(@class, 'type-pkmn_card entry')]"))
                {
                    string imageLinkDirect = node.FirstChild.FirstChild.FirstChild.GetAttributeValue("href", "notfound");
                    string cardName = node.FirstChild.LastChild.FirstChild.LastChild.FirstChild.FirstChild.FirstChild.InnerHtml;
                    Console.WriteLine("### DOWNLOAD CARD: " + cardName + "  FROM: " + imageLinkDirect);
                    SaveImage(imageLinkDirect, "images/" + cardName + ".jpeg", ImageFormat.Jpeg);
                    itemsDownloaded++;
                }

            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("=== FINISHED DOWNLOADING " + itemsDownloaded + " ITEMS");

            Console.ReadLine();


        }
        static void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
    }

    
}

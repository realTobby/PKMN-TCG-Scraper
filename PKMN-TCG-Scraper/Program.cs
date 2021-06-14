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
            if(args.Contains("-c"))
            {
                System.IO.Directory.CreateDirectory("images/cutouts");

                var files = System.IO.Directory.GetFiles("images");

                Bitmap CroppedImage;
                Bitmap source;
                foreach (string imagePath in files)
                {
                    int x = 58, y = 100, width = 616, height = 383;
                    source = new Bitmap(imagePath);
                    CroppedImage = source.Clone(new System.Drawing.Rectangle(x, y, width, height), PixelFormat.Format24bppRgb);
                    string filename = System.IO.Path.GetFileNameWithoutExtension(imagePath) + "cut.jpeg";
                    Console.WriteLine("### CUTTING: " + filename);
                    CroppedImage.Save("images/cutouts/" + filename);
                    CroppedImage.Dispose();
                    source.Dispose();
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory("images");

                int itemsDownloaded = 0;

                for (int pageIndex = 51; pageIndex < maxPage; pageIndex++)
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
            }

            

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

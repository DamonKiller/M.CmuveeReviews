using Newtonsoft.Json;
using SpiderMovies.Spider;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CmuveeReviews.Spider
{
    class Program
    {
        private static List<Movies> list = new List<Movies>();

        static void Main(string[] args)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            X.ORMData.ORMBase.CreateDataSource(X.ORMData.DataSourceType.Sqlite, $"Data Source={path}\\movies.db");
            list = Movies.GetList<Movies>();

            for (int j = 0; j < 5; j++)
            {
                Thread thread = new Thread(() => {
                    int jj = j;
                    for (int i = 0; i < 6000; i++)
                    {
                        int id = 6000 * jj + i;
                        //Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " ==>>" + id);
                        if (list.Find(r => r.SourceId == id.ToString()) != null) continue;

                        string url = $"http://www.yongjiuzy.cc/?m=vod-detail-id-{id}.html";
                        Console.WriteLine("开始抓取--> start " + url);
                        string result = HttpGet(url);
                        //Console.WriteLine(result);
                        if (result.IndexOf("获取数据失败，请勿非法传递参数") > 0) continue;

                        Console.WriteLine("开始解析--> start ");
                        ParseMovie(id.ToString(), result);

                        //Thread.Sleep(1000);
                    }
                });
                thread.Start();
                Thread.Sleep(2000);
            }
            Console.ReadKey();
        }

        public static void ParseMovie(string id, string content)
        {
            try
            {
                Movies m = new Movies();
                m.SourceId = id;
                m.Name = RegexContent(content, "<!--片名开始-->(.*)<!--片名结束-->");
                m.AliasName = RegexContent(content, "<!--别名开始-->(.*)<!--别名开始-->");
                m.Remark = RegexContent(content, "<!--备注开始-->(.*)<!--备注开始-->");
                m.Starring = RegexContent(content, "<!--主演开始-->(.*)<!--主演结束-->");
                m.Director = RegexContent(content, "<!--导演开始-->(.*)<!--导演结束-->");
                m.Category = RegexContent(content, "<!--栏目开始-->(.*)<!--栏目结束-->");
                m.Type = RegexContent(content, "<!--类型开始-->(.*)<!--类型结束-->");
                m.Language = RegexContent(content, "<!--语言开始-->(.*)<!--语言结束-->");
                m.Region = RegexContent(content, "<!--地区开始-->(.*)<!--地区结束-->");
                m.Status = RegexContent(content, "<!--连载开始-->(.*)<!--连载结束-->");
                m.Release = RegexContent(content, "<!--年代开始-->(.*)<!--年代结束-->");
                m.DoubanScore = RegexContent(content, "<!--豆瓣ID开始-->(.*)<!--豆瓣ID结束-->");
                m.Description = RegexContent(content, "<!--简介开始-->(.*)<!--简介结束-->");
                m.PicsAddress = RegexContent(content, "<!--图片开始-->(.*)<!--图片结束-->");
                m.PlayAddress = RegexContent(content, "<!--海洋CMS地址开始>(.*)<海洋CMS地址结束-->");
                m.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Console.WriteLine(JsonConvert.SerializeObject(m));
                Movies.Insert(m);
                list.Add(m);
            }
            catch (Exception)
            {

            }
        }

        public static string RegexContent(string content, string reg)
        {
            var links = Regex.Matches(content, reg, RegexOptions.IgnoreCase);
            foreach (Match match in links)
            {
                //Console.WriteLine(match.Groups[1].Value);
                return match.Groups[1].Value;
            }
            return "";
        }

        public static void ParsePage(string content)
        {
            //<td class="l"><a href="/?m=vod-detail-id-27233.html" target="_blank">空降利刃DVD版<font color="red"> [第48集]</font></a></td>
            var links = Regex.Matches(content, "<td class=\"l\"><a href=\"(.*)\" target=\"_blank\">(.*)</font>", RegexOptions.IgnoreCase);
            foreach (Match match in links)
            {
                Console.WriteLine(match.Groups[1].Value);
                Console.WriteLine(match.Groups[2].Value);
            }
        }

        public static string HttpGet(string url)
        {
            string result = "";
            string tempUrl = url;
            do
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        var response = client.UploadValues(tempUrl, values);
                        var responseStr = Encoding.UTF8.GetString(response);
                        result = responseStr;
                        if (responseStr.IndexOf(";;window.location") >= 0)
                        {
                            responseStr = responseStr.Substring(responseStr.IndexOf("\">") + 2, responseStr.IndexOf(";;window") - responseStr.IndexOf("\">"));
                            string pasteurl = "";
                            foreach (var item in responseStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                pasteurl = item.Substring(item.IndexOf("'") + 1, item.LastIndexOf("'") - item.IndexOf("'") - 1) + pasteurl;
                            }
                            tempUrl = url + pasteurl;
                            Console.WriteLine("重新抓取-->" + tempUrl);
                        }
                    }
                }
                catch (Exception)
                {

                }
            } while (result.IndexOf(";;window.location") >= 0);
            return result;
        }
    }
}

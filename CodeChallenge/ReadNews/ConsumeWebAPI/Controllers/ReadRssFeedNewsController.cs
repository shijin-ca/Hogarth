using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsumeWebAPI.Controllers
{
    public class ReadRssFeedNewsController : Controller
    {
        private readonly ILogger<ReadRssFeedNewsController> _logger;

         public string url = "https://localhost:44388/Api/RSSRepositories";
       public ReadRssFeedNewsController(ILogger<ReadRssFeedNewsController> logger)
        {
            _logger = logger;
        }
        
        public async Task<IActionResult> Index(ReadRssFeedNewsListViewModel model)
        {
            using (var client = new HttpClient())
            {
                //set up client+
                var baseUrl = "https://localhost:44388";
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUri = new Uri($"/Api/RSSRepositories", UriKind.Relative);
                var content = new StringContent($"\"{model.RssFeedUrl}\"", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUri, content);
                var result = await response.Content.ReadAsStringAsync();
            }
            return View();
        }

        public async Task<IActionResult> NewsItems(ReadRssFeedNewsListViewModel model)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var newsFeedList = JsonConvert.DeserializeObject<List<RssFeedListDTO>>(JsonConvert.DeserializeObject<string>(apiResponse));
                        Func<RssFeedListDTO, object> orderBy;

                        switch (model.SortBy)
                        {
                            case 0:
                                orderBy = x => x.Title;
                                break;
                            case 1:
                            default:
                                orderBy = x => x.PublishingDate;
                                break;
                        }

                        if (model.Desc)
                        {
                            newsFeedList = newsFeedList.OrderByDescending(orderBy).ToList();
                        }
                        else
                        {
                            newsFeedList = newsFeedList.OrderBy(orderBy).ToList();
                        }

                        model.RssFeedList = newsFeedList;
                        return View(model);
                    }
                }
            }

            return View();
        }

        public async Task<IActionResult> ReadNewsDetails(int id)
        {
            string urlDetails = url + "/" + id;

            var model = new ReadRssFeedNewsDetailsViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(urlDetails))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var newsFeedItem = JsonConvert.DeserializeObject<RssFeedItemDTO>(JsonConvert.DeserializeObject<string>(apiResponse));
                        model.RssFeedItem = newsFeedItem;
                    }
                }
            }
            return View(model);

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

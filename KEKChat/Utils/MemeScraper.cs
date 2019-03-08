using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using RedditSharp;
using RedditSharp.Things;
using System.Web.Mvc;
using KEKChat.Models;
using System.Threading.Tasks;

namespace KEKChat.Utils
{
    public class MemeScraper
    {
        public string SavePath { get; private set; }
        public Subreddit Subreddit { get; private set; }
        public uint Amount { get; private set; }
        Reddit reddit;

        public MemeScraper(string savepath, string sub, uint amount)
        {
            SavePath = savepath;
            Amount = amount;

            reddit = new Reddit();
            Subreddit = reddit.GetSubreddit(sub);
        }

        public async Task<List<MemeModel>> DownloadImagesAsync()
        {
            List<Task<MemeModel>> tasks = new List<Task<MemeModel>>((int)Amount);
            List<MemeModel> memes = new List<MemeModel>();

            foreach (var post in Subreddit.Hot.Take((int)Amount))
            {
                tasks.Add(DownloadImageAsync(post));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
                memes.Add(task.Result);

            return memes;
        }

        private async Task<MemeModel> DownloadImageAsync(Post post)
        {
            string postURL = Convert.ToString(post.Url);
            if (post.IsStickied || post.IsSelfPost || postURL.Contains("reddituploads") || postURL.Contains("gfycat") || postURL.Contains(".gifv") || postURL.Contains(".mp4")) continue;

            string[] splitURL;

            if (postURL.Contains("i.redd.it"))
            {
                splitURL = postURL.Split('/');

                int index = splitURL.Length - 1;
                string fileName = splitURL[index];

                WebClient client = new WebClient();

                await client.DownloadFileTaskAsync(post.Url, SavePath + fileName);

                return new MemeModel();
            }

            return null;
        }
    }
}
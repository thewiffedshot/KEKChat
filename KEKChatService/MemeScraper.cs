using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using KEKCore.Contexts;
using KEKCore.Entities;

using RedditSharp;
using RedditSharp.Things;

namespace KEKChatService
{
    public class MemeScraper
    {
        public string SavePath { get; private set; }
        public string[] Subreddits { get; private set; }
        public uint Amount { get; private set; }

        public MemeScraper(string savepath, string[] subs, uint amount)
        {
            SavePath = savepath;
            Amount = amount;
            Subreddits = subs;
        }

        public async Task GetMemesFromSubsAsync()
        {
            List<Task> memeSubCollections = new List<Task>(Subreddits.Length);

            foreach (string sub in Subreddits)
                memeSubCollections.Add(DownloadImagesAsync(sub));

            await Task.WhenAll(memeSubCollections);
        }

        public async Task DownloadImagesAsync(string sub)
        {
            Reddit reddit = new Reddit();
            var subreddit = reddit.GetSubreddit(sub);

            List<Task> tasks = new List<Task>((int)Amount);

            foreach (var post in subreddit.Hot.Take((int)Amount))
            {
                tasks.Add(DownloadImageAsync(post));
            }

            await Task.WhenAll(tasks);
        }

        private async Task DownloadImageAsync(Post post)
        {
            string postURL = Convert.ToString(post.Url);

            if (post.IsStickied || 
                post.IsSelfPost || 
                postURL.Contains("reddituploads") || 
                postURL.Contains("gfycat") || 
                postURL.Contains(".gifv") || 
                postURL.Contains(".mp4"))
                return;

            string[] splitURL;

            if (postURL.Contains("i.redd.it"))
            {
                splitURL = postURL.Split('/');

                int index = splitURL.Length - 1;
                string fileName = splitURL[index];

                WebClient client = new WebClient();

                await client.DownloadFileTaskAsync(post.Url, SavePath + fileName);

                decimal price = post.Score * 1e-2m;

                price *= (decimal)Math.Pow(1.10, post.Gilded);

                int batch = 50;

                //if (post.NSFW)
                //    return;

                var meme = new MemeEntry { ImagePath = "Memes\\" + fileName,
                                           Price = price,
                                           VendorAmount = batch,
                                           Subreddit = post.Subreddit.Name,
                                           GoldCount = post.Gilded,
                                           NSFW = post.NSFW
                };

                using (UsersDB db = new UsersDB())
                {
                    var memes = db.MemeStash
                                  .Where(m => m.ImagePath == meme.ImagePath)
                                  .ToList();

                    if (memes.Count == 0)
                    {
                        db.MemeStash.Add(meme);

                        KEKCore.Store.InitializeWeights(meme, db);

                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
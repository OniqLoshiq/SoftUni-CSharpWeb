using IRunesModels;
using SIS.HTTP.Common;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace SIS_IRunes.Controllers
{
    public class AlbumsController : BaseController
    {
        private const string NoAlbumsMessage = "There are currently no albums.";

        private const string NoTracksMessage = "There are currently no tracks.";

        public IHttpResponse All(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                string cookieData = request.Cookies.GetCookie(GlobalConstants.AuthenticationCookieKey).Value;
                string username = this.UserCookieService.GetUserData(cookieData);

                var albumList = new StringBuilder();
                var userAlbums = this.DbContext.Users.First(u => u.Username == username).Albums;

                if (!userAlbums.Any())
                {
                    albumList.AppendLine($@"<h4>{NoAlbumsMessage}</h4>");
                }
                else
                {
                    foreach (var album in userAlbums)
                    {
                        albumList.AppendLine($@"<h4><a href=""/Albums/Details?id={album.Id}"">{album.Name}</a></h4>");
                    }
                }

                this.ViewBag["albumList"] = albumList.ToString();

                return this.View("All");
            }

            return new RedirectResult("/Home/Index");
        }

        public IHttpResponse CreateGet(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return this.View("Create");
            }

            return new RedirectResult("/Home/Index");
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                string cookieData = request.Cookies.GetCookie(GlobalConstants.AuthenticationCookieKey).Value;
                string username = this.UserCookieService.GetUserData(cookieData);

                var user = this.DbContext.Users.First(u => u.Username == username);

                string albumName = WebUtility.UrlDecode(request.FormData["albumName"].ToString().Trim());

                //The Cover URL works with web links to a picture
                //Example: https://upload.wikimedia.org/wikipedia/en/thumb/1/1f/The_Rolling_Stones_-_Blue_&_Lonesome.png/220px-The_Rolling_Stones_-_Blue_&_Lonesome.png
                string albumCoverUrl = WebUtility.UrlDecode(request.FormData["coverUrl"].ToString());

                if (user.Albums.Any(a => a.Name == albumName))
                {
                    return new RedirectResult("/Albums/Create");
                }

                string albumId = Guid.NewGuid().ToString();

                var album = new Album
                {
                    Id = albumId,
                    Name = albumName,
                    CoverUrl = albumCoverUrl,
                    UserId = user.Id
                };

                this.DbContext.Albums.Add(album);

                try
                {
                    this.DbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return new BadRequestResult("Something went wrong :)...", SIS.HTTP.Enums.HttpResponseStatusCode.InternalServerError);
                }

                return new RedirectResult("/Albums/All");
            }

            return new RedirectResult("/Home/Index");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                string cookieData = request.Cookies.GetCookie(GlobalConstants.AuthenticationCookieKey).Value;
                string username = this.UserCookieService.GetUserData(cookieData);

                var albumId = request.QueryData["id"].ToString();

                var album = this.DbContext.Albums.FirstOrDefault(a => a.User.Username == username && a.Id == albumId);

                if (album == null)
                {
                    return this.View("All");
                }

                var trackList = new StringBuilder();

                if (!album.Tracks.Any())
                {
                    trackList.AppendLine($@"<h4>{NoTracksMessage}</h4>");
                }
                else
                {
                    trackList.AppendLine(@"<ul>");

                    int counter = 1;

                    foreach (var track in album.Tracks)
                    {
                        trackList.AppendLine($@"<li><strong>{counter++}. </strong><em><a href=""/Tracks/Details?albumId={album.Id}&trackId={track.Id}"">{track.Name}</a></em></h4></li>");
                    }

                    trackList.AppendLine(@"</ul>");
                }

                this.ViewBag["albumCoverUrl"] = album.CoverUrl;
                this.ViewBag["albumNamePictureText"] = $"{album.Name} picture format";
                this.ViewBag["albumName"] = album.Name;
                this.ViewBag["albumPrice"] = album.Price.ToString("f2");
                this.ViewBag["albumId"] = album.Id;
                this.ViewBag["trackList"] = trackList.ToString();

                return this.View("Details");
            }

            return new RedirectResult("/Home/Index");
        }
    }
}

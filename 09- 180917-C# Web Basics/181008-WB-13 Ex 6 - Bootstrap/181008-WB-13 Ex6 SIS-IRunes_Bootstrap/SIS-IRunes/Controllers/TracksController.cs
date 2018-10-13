using IRunesModels;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Linq;
using System.Net;

namespace SIS_IRunes.Controllers
{
    public class TracksController : BaseController
    {
        public IHttpResponse CreateGet(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                string albumId = request.QueryData["albumId"].ToString();

                this.ViewBag["albumId"] = albumId;

                return this.View("Create", true);
            }

            return new RedirectResult("/Home/Index");
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                string albumId = request.QueryData["albumId"].ToString();

                var album = this.DbContext.Albums.FirstOrDefault(a => a.Id == albumId);

                if(album == null)
                {
                    return new RedirectResult("/Albums/All");
                }

                string trackName = WebUtility.UrlDecode(request.FormData["trackName"].ToString());

                //The track link URL works with youtube embed links
                //Example: https://www.youtube.com/embed/8CdcCD5V-d8
                string trackLinkUrl = WebUtility.UrlDecode(request.FormData["linkUrl"].ToString());

                decimal trackPrice = decimal.Parse(request.FormData["trackPrice"].ToString());

                string trackId = Guid.NewGuid().ToString();

                var track = new Track
                {
                    Id = trackId,
                    Name = trackName,
                    LinkUrl = trackLinkUrl,
                    Price = trackPrice,
                    AlbumId = album.Id
                };

                this.DbContext.Tracks.Add(track);

                try
                {
                    this.DbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return new BadRequestResult("Something went wrong :)...", SIS.HTTP.Enums.HttpResponseStatusCode.InternalServerError);
                }

                return new RedirectResult($"/Albums/Details?id={album.Id}");
            }

            return new RedirectResult("/Home/Index");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var albumId = request.QueryData["albumId"].ToString();
                var trackId = request.QueryData["trackId"].ToString();

                if(!this.DbContext.Tracks.Any(t => t.Id == trackId && t.AlbumId == albumId))
                {
                    return new RedirectResult($"/Albums/Details?id={albumId}");
                }

                var track = this.DbContext.Tracks.First(t => t.Id == trackId);

                this.ViewBag["youtubeTrackUrl"] = track.LinkUrl;
                this.ViewBag["trackName"] = track.Name;
                this.ViewBag["trackPrice"] = track.Price.ToString("f2");
                this.ViewBag["albumId"] = albumId;

                return this.View("Details", true);
            }

            return new RedirectResult("/Home/Index");
        }
    }
}

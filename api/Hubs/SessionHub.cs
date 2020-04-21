using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Plex.Api;
using Plex.Api.Models.Server;
using PlexPoster.Api.ResourceModels;
using PlexPoster.Api.Services;

namespace PlexPoster.Api.Hubs
{
    public class SessionHub : Hub
    {
        private readonly IPlexClient _plexClient;
        private readonly PlexService _plexService;

        public SessionHub(PlexService plexService, IPlexClient plexClient)
        {
            _plexClient = plexClient;
            _plexService = plexService;
        }
        
        public decimal GetProgressPercent(decimal duration, decimal offset)
        {
            int durint = (int) duration;
            int time = durint / 1000;
            decimal days = Math.Floor((decimal) (time / (24 * 60 * 60)));

            var hours = Math.Floor((time - (days * 24 * 60 * 60)) / (60 * 60));
            var minutes = Math.Floor((time - (days * 24 * 60 * 60) - (hours * 60 * 60)) / 60);
            var seconds = (time - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60)) % 60;

            //offSet = $clients['viewOffset'];
            var offint = (int) offset;
            var timeoff = offint / 1000;
            var daysoff = Math.Floor((decimal) timeoff / (24 * 60 * 60));
            var hoursoff = Math.Floor((timeoff - (daysoff * 24 * 60 * 60)) / (60 * 60));
            var minutesoff = Math.Floor((timeoff - (daysoff * 24 * 60 * 60) - (hoursoff * 60 * 60)) / 60);
            var secondsoff = (timeoff - (daysoff * 24 * 60 * 60) - (hoursoff * 60 * 60) - (minutesoff * 60)) % 60;

            decimal percentComplete = (timeoff / time) * 100;

            return percentComplete;
        }

        public async Task<SessionResponseModel> InitiateSession(string authKey, string serverName, string playerId)
        {
            List<Server> servers = await _plexClient.GetServers(authKey);

            if (servers == null || servers.Count == 0)
            {
                throw new ApplicationException("Invalid Server");
            }
            
            Server server = servers.FirstOrDefault(c=> string.Equals(serverName, c.Name, StringComparison.OrdinalIgnoreCase));
            if (server == null)
            {
                throw new ApplicationException("Invalid Server");
            }
            
            Uri fullUri = server.Host.ReturnUriFromServerInfo(server);
        
            while (true)
            {
                SessionResponseModel sessionModel = null;

                var session = await _plexService.GetActiveSession(authKey, fullUri.ToString(), playerId);

                if (session?.Player != null && (string.Equals(session.Type, "movie", StringComparison.OrdinalIgnoreCase) ||
                                                string.Equals(session.Type, "episode", StringComparison.OrdinalIgnoreCase)))
                {
                    sessionModel = new SessionResponseModel
                    {
                        PlayerState = session.Player.State,
                        Duration = session.Duration,
                        PercentComplete = decimal.Parse(session.ViewOffset) / decimal.Parse(session.Duration),
                        Year = session.Year,
                        Type = session.Type
                    };

                    if (string.Equals(session.Type, "movie", StringComparison.OrdinalIgnoreCase))
                    {
                        sessionModel.Title = session.Title;
                        sessionModel.ArtUrl = Path.Join(fullUri.ToString().TrimEnd('/'), session.Art.TrimEnd('/'), "?X-Plex-Token=" + authKey);
                        sessionModel.PosterUrl = Path.Join(fullUri.ToString().TrimEnd('/'), session.Thumb.TrimEnd('/'), "?X-Plex-Token=" + authKey);
                    }
                    else
                    {
                        sessionModel.Title = session.GrandparentTitle + " : " + session.ParentTitle + " : " + session.Title;
                        sessionModel.ArtUrl = Path.Join(fullUri.ToString().TrimEnd('/'), session.GrandparentArt.TrimEnd('/'), "?X-Plex-Token=" + authKey);
                        sessionModel.PosterUrl = Path.Join(fullUri.ToString().TrimEnd('/'), session.GrandparentThumb.TrimEnd('/'), "?X-Plex-Token=" + authKey);
                    }
                    
                    await Clients.Caller.SendAsync("ReceiveSession", sessionModel);
                    Thread.Sleep(5000);
                }
                else
                {
                    // Pull random movie poster from Plex
                    await Clients.Caller.SendAsync("ReceiveSession", sessionModel);
                    Thread.Sleep(30000);
                }
                
            }
        }
    }
}
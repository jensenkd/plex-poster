using System;

namespace PlexPoster.Api.ResourceModels
{
    public class SessionResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PlayerState { get; set; }
        public string Duration { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public string ArtUrl { get;set; }
        public decimal PercentComplete { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
    }
}
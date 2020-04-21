# Plex Digital Movie Poster

![Example Poster](./docs/poster_example.png)

This app generates a web interface for displaying movie posters digitally on a wall mounted TV/Montior. 

## Features
- Display Movie Posters from Plex Media Server
- When Movie / Episode are playing, show Poster and details of currently playing item

## Overview of Stack
- Server
  - ASP.NET Core
- Client
  - Vue.js
  - Webpack for asset bundling and HMR (Hot Module Replacement)
  - CSS Modules
  - Fetch API for REST requests
- Testing
  - xUnit for .NET Core

## Deployment

### Coming Soon

## Development Setup

### Api
1. cd ./api
1. dotnet build
2. dotnet run

### Web
1. cd ./client-web
2. copy .env.production -> .env.local
3. edit .env.local with correct Plex Server values
4. Run `npm install && npm run serve-local`
5. Open browser and navigate to [http://localhost:8080](http://localhost:8080).

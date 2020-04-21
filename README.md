# Plex Digital Movie Poster

This app generates a web interface for displaying movie posters digitally on a wall mounted TV/Montior. 

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

## Setup

1. Install the following:
   - [.NET Core 3.1](https://www.microsoft.com/net/core)
   - [Node.js >= v8.11.0](https://nodejs.org/en/download/)
   - [Docker](https://docs.docker.com/engine/installation/)
2. Run `npm install && npm start`
3. Open browser and navigate to [http://localhost:8080](http://localhost:8080).

## Scripts

### `npm install`

When first cloning the repo or adding new dependencies, run this command.  This will:

- Install Node dependencies from package.json
- Install .NET Core dependencies from api/api.csproj and api.test/api.test.csproj (using dotnet restore)

### `npm start`

To start the app for development, run this command.  This will:

- Run `docker-compose up` to ensure the Docker images are up and running
- Run dotnet watch run which will build the app (if changed), watch for changes and start the web server on http://localhost:5000
- Run Webpack dev middleware with HMR via [ASP.NET JavaScriptServices](https://github.com/aspnet/JavaScriptServices)

### `npm test`

This will run the xUnit tests in api.test/ and the Vue.js tests in client-web.test/.

### `npm run deploy:prod`

_Before running this script, you need to create a ops/hosts file first.  See the [ops README](ops/) for instructions._

This script will:
 - Build release Webpack bundles
 - Package the .NET Core application in Release mode (dotnet publish)
  - Copies the build assets to the remote host(s)
  - Restarts the app so that changes will be picked up

## Visual Studio Code config

This project has [Visual Studio Code](https://code.visualstudio.com/) tasks and debugger launch config located in .vscode/.

### Tasks

- **Command+Shift+B** - Runs the "build" task which builds the api/ project
- **Command+Shift+T** - Runs the "test" task which runs the xUnit tests in api.test/ and Mocha/Enzyme tests in client-web.test/.

### Debug Launcher

With the following debugger launch configs, you can set breakpoints in api/ or the the Mocha tests in client-web.test/ and have full debugging support.

- **Debug api/ (server)** - Runs the vscode debugger (breakpoints) on the api/ .NET Core app
- **Debug client-web.test/ (Mocha tests)** - Runs the vscode debugger on the client-web.test/ Mocha tests

## Credit

The following resources were helpful in setting up this template:



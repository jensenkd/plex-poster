<template>
  <div id="#container" v-if="session">
    <div id="top">
      <div v-if="!isMovieSession" class="row" style="overflow: hidden;" align="center">
        <div class="col-md-12 title-container">
          <h1 class="title-header">COMING SOON</h1>
        </div>
      </div>
      <div v-if="isMovieSession" class="row" style="overflow: hidden;" align="center">
        <div class="col-md-3">
          <div class="time-box-top">START TIME</div>
          <div class="time-box-bottom">3:43 PM</div>
        </div>
        <div class="col-md-6 title-container">
          <h1 class="title-header">NOW PLAYING</h1>
        </div>
        <div class="col-md-3">
          <div class="time-box-top">END TIME</div>
          <div class="time-box-bottom">5:43 PM</div>
        </div>
      </div>
    </div>
    <div id="middle">
      <transition name="fade">
        <img :src="session.posterUrl" style="width: 100%" />
      </transition>
    </div>
    <div id="bottom"></div>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";
let delayInSeconds = 30;

export default {
  name: "Poster",
  props: {
    msg: String
  },
  data() {
    return {
      connection: null,
      session: null,
      plexAuthKey: process.env.VUE_APP_PLEX_AUTH_KEY,
      plexServerHost: process.env.VUE_APP_PLEX_SERVER_HOST,
      plexPlayerMachineId: process.env.VUE_APP_PLEX_PLAYER_MACHINE_ID,
      plexMovieLibraries: process.env.VUE_APP_PLEX_MOVIE_LIBRARIES.split(",")
    };
  },
  created() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(process.env.VUE_APP_API_URI + "hubs/session")
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.onreconnecting(error => {
      console.error("Connection lost due to: " + error + ".  Reconnecting!");
    });

    this.connection.onclose(error => {
      console.assert(
        this.connection.state === signalR.HubConnectionState.Disconnected
      );
      console.error(
        "Connection closed due to: " +
          error +
          ". Try refressing this page to restart the connection."
      );
    });

    this.connection
      .start()
      .then(() =>
        this.connection
          .invoke(
            "InitiateSession",
            this.plexAuthKey,
            this.plexServerHost,
            this.plexPlayerMachineId,
            this.plexMovieLibraries,
            delayInSeconds
          )
          .catch(function(err) {
            return console.error(err.toString());
          })
      )
      .catch(function(err) {
        return console.error(err.toString());
      });
  },
  mounted() {
    var me = this;
    me.connection.on("ReceiveSession", function(statusModel) {
      me.session = statusModel;
    });
  },
  methods: {
    getServerConfig() {}
  },
  computed: {
    isMovieSession: function() {
      return this.session && this.session.playerState != "none";
    }
  }
};
</script>

<style>
#container {
  min-height: 100%;
  position: relative;
}

h1.title-header {
  font-size: 50px;
  font-weight: bold;
  color: yellow;
}

.title-container {
  margin-top: 10px;
}

.time-box-top {
  border: solid 1px yellow;
  padding: 10px;
  border-bottom: none;
  font-size: 30px;
  font-weight: bold;
  color: yellow;
}

.time-box-bottom {
  border: solid 1px yellow;
  padding: 10px;
  font-size: 30px;
  font-weight: bold;
  color: white;
}

#middle {
  vertical-align: top;
}

#bottom {
  background-color: black;
  bottom: 0;
  height: 100px;
  left: 15;
  position: absolute;
  right: 10;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
  opacity: 0;
}
</style>

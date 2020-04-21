<template>
  <div id="#container">
    <div class="row" id="top" style="overflow: hidden;" align="center">
      <div
        class="col-md-3"
        style="margin-top: 0;
          font-size: 30px;
          font-weight: bold;
          color: yellow;"
      >
        <div
          style="border: solid 1px yellow; padding: 10px; border-bottom: none;"
        >
          START TIME
        </div>
        <div style="color: white; border: solid 1px yellow; padding: 10px;">
          3:43 PM
        </div>
      </div>
      <div
        id="title-header"
        class="col-md-6"
        style="padding-top:10px;"
      >
        <span v-if="session">NOW PLAYING</span>
        <span v-else>COMING SOON</span>
      </div>
      <div
        class="col-md-3"
        style="margin-top: 0;
          font-size: 30px;
          font-weight: bold;
          color: yellow;"
      >
        <div
          style="border: solid 1px yellow; padding: 10px; border-bottom: none;"
        >
          END TIME
        </div>
        <div style="color: white; border: solid 1px yellow; padding: 10px;">
          5:43 PM
        </div>
      </div>
    </div>

    <div id="middle" class="middle">
      <img
        v-if="session && session.posterUrl"
        :src="session.posterUrl"
        style="width: 100%"
      />
    </div>

    <div class="row" style="overflow: hidden;" align="center">
      <div
        class="col-md-3"
        style="margin-top: 30px;
          font-size: 40px;
          v-align: middle;
          font-weight: bold;"
      >
        <div style="color: white;">PG 13</div>
      </div>
      <div
        class="col-md-3"
        style="margin-top: 20px;
          font-size: 30px;
          font-weight: bold;
          color: yellow;"
      >
        <span>Year</span>
        <div style="color: white;">2001</div>
      </div>
      <div
        class="col-md-3"
        style="margin-top: 20px;
          font-size: 30px;
          font-weight: bold;
          color: yellow;"
      >
        <div>END TIME</div>
        <div style="color: white;">5:43 PM</div>
      </div>
      <div
        class="col-md-3"
        style="margin-top: 20px;
          font-size: 30px;
          font-weight: bold;
          color: yellow;"
      >
        <div>PG 13</div>
      </div>
    </div>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";

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
      plexPlayerMachineId: process.env.VUE_APP_PLEX_PLAYER_MACHINE_ID
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
          ".  Try refressing this page to restart the connection."
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
            this.plexPlayerMachineId
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
      if (statusModel != null) {
        me.session = statusModel;
      } else {
        me.session = null;
        console.log("No Session Available");
      }
    });
  },
  methods: {}
};
</script>

<style>
html,
body {
  height: 100%;
  overflow-y: hidden;
}

body {
  background-color: black;
  color: white;
  font-size: large;
  font-family: Arial, Helvetica, sans-serif;
}

#title-header span {
  font-size: 70px;
  font-weight: normal;
  color: yellow;
}

#container {
  min-height: 100%;
  position: relative;
}

.title {
  font-size: 20px;
}

#middle {
  color: white;
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

.newsimage {
  margin-right: 10px;
}
</style>

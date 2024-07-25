import { HubConnectionBuilder, type HubConnection, LogLevel } from "@microsoft/signalr";
import { readable } from "svelte/store";

const URL = "";

export const signalRConnection = readable({} as HubConnection, set => {
    const connection = new HubConnectionBuilder()
        .configureLogging(LogLevel.Trace)
        .withUrl(URL, { skipNegotiation: true })
        .withAutomaticReconnect()
        .build();

    connection.start();
    set(connection);
})

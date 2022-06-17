import React, {useEffect, useState} from 'react';
import {HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";
import axios from "axios";

export default function SignalRMessage() {
    const [toggleSignalR, setToggleSignalR] = useState<boolean>(false);

    const [temps, setTemps] = useState<any>([]);
    useEffect(() => {
        axios.get("https://localhost:5001/WeatherForecast").then(res => setTemps(res.data));
    }, []);

    const [messages, setMessages] = useState<any[]>([]);
    useEffect(() => {
        const notifications: any[] = []
        if (toggleSignalR) {
            console.log("Ceva!");
            try {
                const hubConn = new HubConnectionBuilder()
                    .withUrl("https://localhost:5001/MessageHub")
                    .withAutomaticReconnect()
                    .configureLogging(LogLevel.Information)
                    .build();

                if (hubConn?.state !== HubConnectionState.Connected) {
                    hubConn?.start().then(() => {
                        if (hubConn.state === HubConnectionState.Connected) {
                            hubConn
                                .invoke("JoinRoom", {user: "test", userId: "testId"})
                                .then(() => console.info("User-Joined Room"))
                                .catch((err) => console.error("Could not join room: JoinRoom" + err))
                        }

                        hubConn.on("ReceiveMessage", (message: any) => {
                            console.info("Received message", message);
                            debugger;
                            notifications.push(message);
                            setMessages([...notifications]);
                        })

                        hubConn.onclose(() => setMessages([]));
                    })
                }
            } catch (e) {
                console.error("Error while connecting to Hub: ", e)
            }
        }
    }, [toggleSignalR]);

    function handleMessageClick(item: string) {
        console.info("handleMessageClick", item);
    }

    return (
        <React.Fragment>
            <hr/>
            <h3>Temperatures:</h3>
            <hr/>
            <div>
                {
                    temps.map((item: any, index: number) => (
                        <div key={index}>
                            {JSON.stringify(item)}
                        </div>
                    ))
                }
            </div>
            <hr/>
            <h3>SignalR Messages:</h3>
            <hr/>
            <button onClick={() => setToggleSignalR(!toggleSignalR)}>Toggle</button>
            <label>Cors is: {toggleSignalR ? "On" : "Off"}</label>
            {toggleSignalR &&
                <>
                    <br/>
                    <b>
                        {JSON.stringify(messages)}
                    </b>
                    {/*<p>{messages[0].id}</p>*/}
                    {/*<p>{messages[0].messageText}</p>*/}
                    {/*<p>{messages[0].messageReadStatus}</p>*/}
                    {/*<p>{messages[0].messageType}</p>*/}
                    {/*<p>{messages[0].createdAt}</p>*/}
                </>
            }
        </React.Fragment>
    )
}

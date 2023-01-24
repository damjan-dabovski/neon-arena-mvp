import { React, useState, useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { HubConnectionBuilder } from '@microsoft/signalr';
import SignalRTest from './components/SignalRTest';

function App(){
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [playerId, setPlayerId] = useState("");
  const [lobbies, setLobbies] = useState([]);

  useEffect(() =>{
    let cookies = document.cookie.split(';');
    let playerIdCookieValue = cookies.find(cookie => cookie.includes('playerId'));
    if(playerIdCookieValue){
      setPlayerId(playerIdCookieValue.split('=')[1]);
    }
  }, [playerId]);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7245/gameHub')
      // .withAutomaticReconnect()
      .build();

      setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection){
      connection.start()
        .then(result => {
          console.log('Connected!');

          connection.on('ReceiveMessage', message => {
            setMessages(oldState => [...oldState, message]);
          });

          connection.on('ReceiveLobbyList', lobbyList => {
            setLobbies(lobbyList);
          });

          connection.on('ReceiveLobbyData', lobbyDto => {
            console.log(lobbyDto);
          })

          connection.on('ReceiveIdentityData', data => {
            console.log(data);
            document.cookie = `playerId = ${data.id}`
            setPlayerId(data.id);
          })
        });
    }
  }, [connection]);

  const sendMessage = async (message) => {
    if(connection){
      try{
        await connection.send('Broadcast', message);
      }
      catch(exception){
        console.error(exception);
      }
    }
  }

  const createLobby = async () => {
    if(connection){
      try{
        await connection.send('CreateLobby', playerId);
      }
      catch(exception){
        console.error(exception);
      }
    }
  }

  const joinLobby = async (e) => {
    if(connection){
      try{
        let lobbyId = e.target.innerText;
        await connection.send('JoinLobby', playerId, lobbyId);
      }
      catch(exception){
        console.error(exception);
      }
    }
  }

    return (
      <Layout>
        <SignalRTest sendMessage={sendMessage}/>
        <button onClick={createLobby}>Create Lobby</button>
        <div>{lobbies.map((lobby, index) => {
          return (
            <div key={index} style={{border: "1px solid black", cursor:"pointer"}} onClick={joinLobby}>
              {lobby}
            </div>
          );
        })}
        </div>
      </Layout>
    );

}

export default App;
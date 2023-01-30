import { React, useState, useEffect, createContext, useContext } from 'react';
import { Layout } from './components/Layout';
import './custom.css';
import { HubConnectionBuilder } from '@microsoft/signalr';
import SignalRTest from './components/SignalRTest';
import LobbyList from './components/LobbyList';
import LobbyView from './components/LobbyView';

export const AppContext = createContext();

function App(){
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [playerId, setPlayerId] = useState("");
  const [lobbies, setLobbies] = useState([]);
  const [currentLobby, setCurrentLobby] = useState(null);

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
            setCurrentLobby(lobbyDto);
            console.log(lobbyDto);
          })

          connection.on('ReceiveIdentityData', data => {
            // console.log(data);
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

  const joinSeat = async (seatIndex) => {
    if(connection){
      try{
        await connection.send('JoinSeat', playerId, currentLobby.id, seatIndex);
      }
      catch(exception){
        console.error(exception);
      }
    }
  }

    return (
      <Layout>
        <AppContext.Provider value={{lobbies: lobbies, currentLobby: currentLobby}}>
          <SignalRTest sendMessage={sendMessage}/>
          <LobbyList joinLobby={joinLobby} createLobby={createLobby}/>
          <LobbyView joinSeat={joinSeat}/>
        </AppContext.Provider>
      </Layout>
    );

}

export default App;
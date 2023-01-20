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

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7245/gameHub')
      .withAutomaticReconnect()
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

          connection.on('ReceiveIdentityData', data => {
            console.log(data);
            document.cookie = `playerId = ${data.id}`
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

    return (
      <Layout>
        {/* <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes> */
        <SignalRTest sendMessage={sendMessage}/>
        }
      </Layout>
    );

}

export default App;
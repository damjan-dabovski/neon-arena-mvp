import {React, useContext} from 'react';
import {AppContext} from '../App';

const LobbyList = (props) => {
    const context = useContext(AppContext);

    return(
        <div>
            <button onClick={props.createLobby}>Create Lobby</button>
            <div>{context.lobbies.map((lobby, index) => {
                return (
                  <div key={index} style={{border: "1px solid black", cursor:"pointer"}} onClick={props.joinLobby}>
                    {lobby}
                  </div>
                );
              })}
            </div>
        </div>
    )
}

export default LobbyList;
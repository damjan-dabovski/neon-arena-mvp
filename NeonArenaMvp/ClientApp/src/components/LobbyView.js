import {React, useContext} from 'react';
import {AppContext} from '../App';
import {seats} from '../helpers/Enums'

const LobbyView = (props) => {
    var context = useContext(AppContext);

    const seatsMappedToPlayers = {}; 
    let seatedUsers = [];
    
    if (context.currentLobby){
        seatedUsers = context.currentLobby.users.filter(user => user.selectedSeatIndex !== null);
        for (let user of seatedUsers)
        {
            seatsMappedToPlayers[seats[user.selectedSeatIndex]] = user;
        }
    }

    return(
        <div>
            {context.currentLobby !== null || <h2>You haven't joined any lobby!</h2>}
            {context.currentLobby !== null &&
                <div>
                    <button onClick={props.runMatch}>Run Match!</button>
                    <button onClick={props.leaveLobby} style={{color:"red", fontWeight:"bold"}}>Leave</button>
                    {seats.map((seat, index) => {
                        return(
                            <div key={index}>
                                <div style={{display: "inline-block", width: "20px", height:"20px", borderRadius:"50%", backgroundColor:`${seat}`}}></div>
                                <div style={{display: "inline-block"}}>
                                    {
                                        seatsMappedToPlayers.hasOwnProperty(seat)
                                        ?   <span>
                                                {seatsMappedToPlayers[seat].name}
                                                <select value={seatsMappedToPlayers[seat].selectedCharacterIndex} onChange={props.selectCharacter}>
                                                    {context.currentLobby.characters.map((character, idx) =>
                                                        {
                                                            return (
                                                                <option key={idx} value={idx}>{character}</option>
                                                            );
                                                        })}
                                                </select>
                                                <select value={seatsMappedToPlayers[seat].selectedTeamIndex} onChange={props.selectTeam}>
                                                    {[...Array(seatedUsers.length).keys()].map(item => {
                                                        return (
                                                            <option key={item} value={item + 1}>Team {item + 1}</option>
                                                        );
                                                    })}
                                                </select>
                                                <button onClick={() => props.leaveSeat(index)} style={{color:"red", fontWeight:"bold"}}>Leave Seat</button>
                                            </span>
                                        :   <span>
                                                <span>{"empty"}</span>
                                                <button onClick={() => props.joinSeat(index)}>Join Seat</button>
                                            </span>
                                    }
                                </div>
                            </div>
                        );
                    })}
                </div>
            }
        </div>
    )
}

export default LobbyView;
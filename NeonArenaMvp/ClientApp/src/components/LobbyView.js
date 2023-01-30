import {React, useContext} from 'react';
import {AppContext} from '../App';

const LobbyView = (props) => {
    var context = useContext(AppContext);

    const seats = [
        {color: "red", userName: ""},
        {color: "orange", userName: ""},
        {color: "yellow", userName: ""},
        {color: "lime", userName: ""},
        {color: "cyan", userName: ""},
        {color: "blue", userName: ""},
        {color: "purple", userName: ""},
    ]

    return(
        <div>
            {context.currentLobby !== null || <h2>You haven't joined any lobby!</h2>}
            {context.currentLobby !== null &&
                <div>
                    {seats.map((seat, index) => {
                        return(
                            <div key={index}>
                                <div style={{display: "inline-block", width: "20px", height:"20px", borderRadius:"50%", backgroundColor:`${seat.color}`}}></div>
                                {context.currentLobby.seatSelections.hasOwnProperty(index)
                                ?   <div>
                                        <span>{context.currentLobby.userNames[context.currentLobby.seatSelections[index]]}</span>
                                    </div>
                                :   <div>
                                        <span>{"empty"}</span>
                                        <button onClick={() => props.joinSeat(index)}>Join Seat</button>
                                    </div>
                                }
                            </div>
                        );
                    })}
                </div>
            }
        </div>
    )
}

export default LobbyView;
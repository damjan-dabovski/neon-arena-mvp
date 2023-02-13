import {React, useState, useContext} from 'react';
import {AppContext} from '../App';
import {seats} from '../helpers/Enums'

const MatchView = (props) => {
    var context = useContext(AppContext);

    const [commandString, setCommandString] = useState("");

    const handleInputChange = (e) => {
        setCommandString(e.target.value);
    }

    const sendCommand = () => {
        props.sendCommand(commandString);
    }

    return (
        <div style={{border: "2px solid red"}}>
            {context.currentStep !== null &&
                <span>
                    <input type="text" value={commandString} onChange={handleInputChange}></input>
                    <button onClick={sendCommand}>Send Command</button>
                    {context.currentStep?.playerDtos.map((player, index) => {
                        return (
                            <span style={{color: seats[player.teamIndex - 1]}} key={index}>
                                {player.name}
                            </span>
                        )
                    })}
                </span>
            }
        </div>
    )
};

export default MatchView;
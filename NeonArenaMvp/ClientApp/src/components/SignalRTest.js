import { React, useState } from 'react';

const SignalRTest = (props) => {
    const [message, setMessage] = useState("");

    const sendMessage = () => {
        props.sendMessage(message);
    }

    const handleInputChange = (e) => {
        setMessage(e.target.value);
      };

    return (
        <div>
            <input type="text" value={message} onChange={handleInputChange}></input>
            <button onClick={sendMessage}>Send</button>
        </div>
    )
}

export default SignalRTest;
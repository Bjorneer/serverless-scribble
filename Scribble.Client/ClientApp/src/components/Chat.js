import React, {useEffect, useState} from 'react';
import ChatMessage from './ChatMessage';
import classes from './css/Chat.module.css';

const Chat = props => {
    /*
    const [messages, setMessages] = useState([
        {user: 'Alfred', message: 'This is a message'},
        {user: 'Filip', message: 'Air Conditioner'},
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'GAME_EVENT', message: 'Mattias guessed correctly'}, 
         {user: 'GAME_EVENT', message: '-------------------------'},
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Mattias', message: 'Hello World!'}, 
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Alfred', message: 'This is a message'},
         {user: 'Alfred', message: 'This is a message'},
        ]);
    */
   const [messages, setMessages] = useState([]);

    useEffect(() => {
        if(props.hubConnection !== null){
            props.hubConnection.on('inMessage', (newMessage) => {
                setMessages(prev => {
                    const ne = [newMessage, ...prev];
                    return ne;
                })
            });
        }
    }, [props.hubConnection]);

    let mess = null;
    if(messages.length > 0){
        mess = messages.map(m => {
            return <ChatMessage message={m.message} user={m.user} isFriendly={m.user === props.user}/>
        });
    }



    return (
        <div className={classes.Chat}>
            {mess}
        </div>
    )
}

export default Chat;
import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface Message {
    id: number;
    user: string;
    content: string;
    timestamp: string;
}

const Chat: React.FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState<string>("");

    useEffect(() => {
        const fetchMessages = async () => {
            const result = await axios.get<Message[]>('http://localhost:5240/api/chat');
            setMessages(result.data);
        };
        fetchMessages();

        // Set up SSE
        const eventSource = new EventSource('http://localhost:5240/api/chat/stream');
        eventSource.onmessage = (event) => {
            const [user, content] = event.data.split(": ");
            setMessages(prevMessages => {
                console.table(prevMessages);
                const newResponse = { id: 445, user, content, timestamp: new Date().toISOString() };
                console.log(newResponse);
                return [...prevMessages, newResponse];
            });
            console.log(`Received sse from server ${content}`);
        };

        return () => {
            eventSource.close();
        };
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const postMessage: Omit<Message, 'id'> = {
            user: "User",
            content: newMessage,
            timestamp: new Date().toISOString()
        };

        const response = await axios.post<Message>('http://localhost:5240/api/chat', postMessage);
        setMessages(prevMessages => [...prevMessages, response.data]);
        setNewMessage("");
        console.log(`Received response from server`);
    };

    return (
        <div>
            <h1>Chat Application</h1>
            <div>
                {messages.map(message => (
                    <div key={message.id}>
                        <strong>{message.user}</strong>: {message.content} <small>({new Date(message.timestamp).toLocaleTimeString()})</small>
                    </div>
                ))}
            </div>
            <form onSubmit={handleSubmit}>
                <input 
                    type="text" 
                    value={newMessage} 
                    onChange={e => setNewMessage(e.target.value)} 
                />
                <button type="submit">Send</button>
            </form>
        </div>
    );
};

export default Chat;
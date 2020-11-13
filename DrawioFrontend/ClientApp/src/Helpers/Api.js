
const defaultUrl = 'http://localhost:7071/api'

export const APIUrls = {
    create: '/CreateGame',
    join: '/JoinGame',
    getGameState: '/GetGameState',
    startGame: '/StartGame',
    makeGuess: '/MakeGuess'
};

const postRequest = (url, data) => {
    return fetch(defaultUrl + url, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
};

const getRequest = (url) => {
    return fetch(defaultUrl + url);
};

export const GameAPI = {
    create: (data) => {
        return postRequest(APIUrls.create, data);
    },
    join: (data) => {
        return postRequest(APIUrls.join, data);
    },
    getGameState: (data) => {
        return getRequest(APIUrls.getGameState + '?token=' + data.token + '&gamecode=' + data.gamecode);
    },
    startGame: (data) => {
        return getRequest(APIUrls.startGame + '?token=' + data.token + '&gamecode=' + data.gamecode);
    },
    makeGuess: (data) => {
        return postRequest(APIUrls.makeGuess, data);
    }
};